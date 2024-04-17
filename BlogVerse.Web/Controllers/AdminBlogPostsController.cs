using AutoMapper;
using BlogVerse.Web.IRepository;
using BlogVerse.Web.Models.Domain;
using BlogVerse.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogVerse.Web.Controllers
{
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagRepository tagRepository;
        private readonly IMapper mapper;
        private readonly IBlogPostRepository blogPostRepository;

        public AdminBlogPostsController(ITagRepository tagRepository,
            IMapper mapper,
            IBlogPostRepository blogPostRepository)
        {
            this.tagRepository = tagRepository;
            this.mapper = mapper;
            this.blogPostRepository = blogPostRepository;
        }
        public async Task<IActionResult> Index()
        {
            var blogPosts = await blogPostRepository.GetAllAsync();
            //var blogPostsVM = mapper.Map<BlogPostRequest>(blogPosts);
            return View(blogPosts);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            // get tags from repository
            var tags = await tagRepository.GetAllAsync();

            var model = new AddBlogPostRequest
            {
                Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest model)
        {
            //var blogPost = new BlogPost
            //{
            //    Heading = model.Heading,
            //    PageTitle = model.PageTitle,
            //    Content = model.Content,
            //    ShortDescription = model.ShortDescription,
            //    FeaturedImageUrl = model.FeaturedImageUrl,
            //    UrlHandle = model.UrlHandle,
            //    PublishedDate = model.PublishedDate,
            //    Author = model.Author,
            //    Visible = model.Visible,
            //};
            var blogPost = mapper.Map<BlogPost>(model);

            var selectedTags = new List<Tag>();
            foreach (var selectedTagId in model.SelectedTags)
            {
                var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
                var existingTag = await tagRepository.GetAsync(selectedTagIdAsGuid);

                if (existingTag != null)
                {
                    selectedTags.Add(existingTag);
                }
            }
            blogPost.Tags = selectedTags;

            await blogPostRepository.AddAsync(blogPost);
            //var blogPostVM = mapper.Map<AddBlogPostRequest>(blogDomain);


            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var blogPost = await blogPostRepository.GetAsync(id);
            if (blogPost == null)
            {
                return View(null);
            }


            var tags = await tagRepository.GetAllAsync();

            var model = mapper.Map<EditBlogPostRequest>(blogPost);

            model.Tags = tags.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });

            model.SelectedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray();

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest model)
        {
            var blogPost = mapper.Map<BlogPost>(model);


            var selectedTags = new List<Tag>();

            foreach (var selectedTag in model.SelectedTags)
            {
                if (Guid.TryParse(selectedTag, out var tag))
                {
                    var foundTag = await tagRepository.GetAsync(tag);
                    if (foundTag != null)
                    {
                        selectedTags.Add(foundTag);
                    }
                }
            }
            blogPost.Tags = selectedTags;

            var updatedBlogPost = await blogPostRepository.UpdateAsync(blogPost);
            if (updatedBlogPost != null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditBlogPostRequest model)
        {
            var blogPost = await blogPostRepository.DeleteAsync(model.Id);
            if (blogPost != null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

    }
}
