using Microsoft.AspNetCore.Mvc;
using OkBlog.Data.Repository;
using OkBlog.Models.Db;
using OkBlog.ViewModels;
using System.Threading.Tasks;

namespace OkBlog.Controllers
{
    public class TagsController : Controller
    {
        private IRepository _repo;
        private ITagRepository _tagRepo;

        public TagsController(IRepository repo, ITagRepository tagRepo)
        {
            _repo = repo;
            _tagRepo = tagRepo;
        }

        public IActionResult Index()
        {
            var tags = _tagRepo.GetAllTags();
            return View(tags);
        }

        public IActionResult Tag(int id)
        {
            var tag = _tagRepo.GetTag(id);

            return View(tag);
        }

        [HttpGet]
        public IActionResult Create(int? id)
        {
            if (id == null)
                return View(new Tag());
            else
            {
                var tag = _tagRepo.GetTag((int)id);
                return View(tag);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(Tag tag)
        {         
            if (ModelState.IsValid)
            {
                if (tag.Id > 0)
                    _tagRepo.UpdateTag(tag);
                else
                    _tagRepo.AddTag(tag);

                if (await _tagRepo.SaveChangesAsync())
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(tag);
                }
            }

            return View(tag);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return View(new Tag());
            else
            {
                var tag = _tagRepo.GetTag((int)id);
                return View(tag);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Tag tag)
        {
            if (tag.Id > 0)
                _tagRepo.UpdateTag(tag);
            else
                _tagRepo.AddTag(tag);

            if (await _tagRepo.SaveChangesAsync())
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(tag);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            _tagRepo.RemoveTag(id);
            await _tagRepo.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
