using AutoMapper;
using BlogVerse.Web.Data;
using BlogVerse.Web.IRepository;
using BlogVerse.Web.Models.Domain;
using BlogVerse.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogVerse.Web.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ITagRepository tagRepository;

        public AdminTagsController(ApplicationDbContext dbContext,
            IMapper mapper,
            ITagRepository tagRepository)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.tagRepository = tagRepository;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddTagRequest model)
        {
            //Mapping AddTagRequest to Tag Domain Model
            var tag = mapper.Map<Tag>(model);
            await tagRepository.AddAsync(tag);
            return RedirectToAction(nameof(List));
        }  


        [HttpGet]
        public async Task<IActionResult> List()
        {
            var tags = await tagRepository.GetAllAsync();
            var tagsVM = mapper.Map<List<TagRequest>>(tags);
            return View(tagsVM);

        }
        
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var tag = await tagRepository.GetAsync(id);

            if(tag is not null)
            {
                var editTagRequest = mapper.Map<EditTagRequest>(tag);                
                return View(editTagRequest);
            }

            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, EditTagRequest model)
        {
            var tag = mapper.Map<Tag>(model);
            var tagDomainModel = await tagRepository.UpdateAsync(id, tag);

            if(tagDomainModel is not null)
            {
                return RedirectToAction(nameof(List));
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest model)
        {
            var tag = await tagRepository.DeleteAsync(model.Id);
            if(tag is not null)
            {                
                return RedirectToAction(nameof(List));
            }
            return View();
        }
    }
}
