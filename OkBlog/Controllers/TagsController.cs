using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<PostsController> _logger;

        public TagsController(IRepository repo, ITagRepository tagRepo, ILogger<PostsController> logger)
        {
            _repo = repo;
            _tagRepo = tagRepo;
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("TagsController Invoked");

            var tags = _tagRepo.GetAllTags();
            _logger.LogDebug("Произведена выборка всех тегов");

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
            _logger.LogTrace("Запрашиваемый id тега: " + id);

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
                _logger.LogTrace($"Добавление тега: {tag.Name} c Id: {tag.Id}");

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
            _logger.LogTrace($"Редактирование тега: {tag.Name} c Id: {tag.Id}");

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
            _logger.LogTrace($"Удаление тега c Id: {id}");

            _tagRepo.RemoveTag(id);
            await _tagRepo.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
