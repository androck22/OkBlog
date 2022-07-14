using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OkBlog.Data.FileManager;
using OkBlog.Data.Repository;
using OkBlog.Models.Db;
using OkBlog.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OkBlog.Controllers
{
    public class EditPostController : Controller
    {
        private IRepository _repo;
        private ITagRepository _tagRepo;
        private IFileManager _fileManager;
        UserManager<ApplicationUser> _userManager;
        private readonly ILogger<PostsController> _logger;

        public EditPostController(IRepository repo, ITagRepository tagRepo, IFileManager fileManager, UserManager<ApplicationUser> userManager, ILogger<PostsController> logger)
        {
            _repo = repo;
            _tagRepo = tagRepo;
            _fileManager = fileManager;
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var posts = _repo.GetAllPosts();

            List<PostViewModel> models = new List<PostViewModel>();

            foreach (var post in posts)
            {
                var postId = post.Id;
                var postGet = _repo.GetPost(postId);
                var tagsId = post.Tags.Select(x => x.Id).ToList();
                var postTags = _tagRepo.GetAllTags().Select(t => new TagViewModel { Id = t.Id, Name = t.Name, IsSelected = tagsId.Contains(t.Id) }).ToList();

                models.Add(new PostViewModel
                {
                    Id = post.Id,
                    Title = post.Title,
                    Body = post.Body,
                    Category = post.Category,
                    Author = post.Author,
                    Tags = postTags
                });
            }

            return View(models);
        }

        public IActionResult GetUserPosts()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var userPosts = _repo.GetAllPosts().Where(x => x.UserId == userId);

            List<PostViewModel> models = new List<PostViewModel>();

            foreach (var post in userPosts)
            {
                var postId = post.Id;
                var postGet = _repo.GetPost(postId);
                var tagsId = post.Tags.Select(x => x.Id).ToList();
                var postTags = _tagRepo.GetAllTags().Select(t => new TagViewModel { Id = t.Id, Name = t.Name, IsSelected = tagsId.Contains(t.Id) }).ToList();

                models.Add(new PostViewModel
                {
                    Id = post.Id,
                    Title = post.Title,
                    Body = post.Body,
                    Category = post.Category,
                    Author = post.Author,
                    Tags = postTags
                });
                _logger.LogTrace("Запрашиваемая статья пользователя с id: " + post.Id);
            }
            return View(models);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var post = _repo.GetPost((int)id);
            var tagsId = post.Tags.Select(x => x.Id).ToList();
            var postTags = _tagRepo.GetAllTags().Select(t => new TagViewModel { Id = t.Id, Name = t.Name, IsSelected = tagsId.Contains(t.Id) }).ToList();
            CreatePostViewModel model = new CreatePostViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                Category = post.Category,
                Tags = postTags,
            };
            _logger.LogTrace("Редактирование статьи с id: " + post.Id);
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            Post post = new Post();
            // получем список всех тегов
            var allTags = _tagRepo.GetAllTags().Select(t => new TagViewModel() { Id = t.Id, Name = t.Name }).ToList();
            CreatePostViewModel model = new CreatePostViewModel
            {
                Id = post.Id,
                Title = post.Title = string.Empty,
                Body = post.Body = string.Empty,
                Category = post.Category = string.Empty,
                Tags = allTags
            };
            _logger.LogTrace("Создание статьи с id: " + post.Id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(CreatePostViewModel model)
        {
            var dbTags = new List<Tag>();

            if (model.Tags != null)
            {
                var postTags = model.Tags.Where(t => t.IsSelected == true).ToList();
                var tagsId = postTags.Select(t => t.Id).ToList();
                dbTags = _tagRepo.GetAllTags().Where(t => tagsId.Contains(t.Id)).ToList();
            }

            var userId = _userManager.GetUserId(HttpContext.User);

            var userEmail = _userManager.GetUserName(HttpContext.User);

            Post post = null;

            if (model.Id > 0)
            {
                post = _repo.GetPost(model.Id);
                post.Title = model.Title;
                post.Body = model.Body;
                post.Category = model.Category;
                post.Author = userEmail;
                post.Tags = dbTags;
                post.UserId = userId;
            }
            else
            {
                post = new Post
                {
                    Id = model.Id,
                    Title = model.Title,
                    Body = model.Body,
                    Category = model.Category,
                    Image = await _fileManager.SaveImage(model.Image),
                    Tags = dbTags,
                    Author = userEmail,
                    UserId = userId
                };
            }


            if (post.Id > 0)
                _repo.UpdatePost(post);
            else
                _repo.AddPost(post);

            if (await _repo.SaveChangesAsync())
            {
                if (User.IsInRole("Admin") || User.IsInRole("Moderator"))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("GetUserPosts");
                }
            }
            else
            {
                return View(post);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Remove(int id)
        {
            _repo.RemovePost(id);
            await _repo.SaveChangesAsync();
            _logger.LogTrace("Удаление статьи с id: " + id);
            return RedirectToAction("Index");
        }
    }
}
