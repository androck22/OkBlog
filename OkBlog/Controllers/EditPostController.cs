using Microsoft.AspNetCore.Mvc;
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

        public EditPostController(IRepository repo, ITagRepository tagRepo)
        {
            _repo = repo;
            _tagRepo = tagRepo;
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
                    Author = post.Author,
                    Tags = postTags
                });
            }
            return View(models);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var post = _repo.GetPost((int)id);
            var tagsId = post.Tags.Select(x => x.Id).ToList();
            var postTags = _tagRepo.GetAllTags().Select(t => new TagViewModel { Id = t.Id, Name = t.Name, IsSelected = tagsId.Contains(t.Id) }).ToList();
            EditPostViewModel model = new EditPostViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                Tags = postTags,
            };

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
                Author = post.Author = string.Empty,
                Tags = allTags
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(CreatePostViewModel model)
        {
            var postTags = model.Tags.Where(t => t.IsSelected == true).ToList();

            var tagsId = postTags.Select(t => t.Id).ToList();

            var dbTags = _tagRepo.GetAllTags().Where(t => tagsId.Contains(t.Id)).ToList();
            Post post = null;

            if (model.Id > 0)
            {
                post = _repo.GetPost(model.Id);
                post.Title = model.Title;
                post.Body = model.Body;
                post.Author = model.Author;
                post.Tags = dbTags;
            }
            else
            {
                post = new Post
                {
                    Id = model.Id,
                    Title = model.Title,
                    Body = model.Body,
                    Author = model.Author,
                    Tags = dbTags
                };
            }

            if (post.Id > 0)
                _repo.UpdatePost(post);
            else
                _repo.AddPost(post);

            if (await _repo.SaveChangesAsync())
            {
                return RedirectToAction("Index");
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
            return RedirectToAction("Index");
        }
    }
}
