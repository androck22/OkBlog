using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using OkBlog.Data.FileManager;
using OkBlog.Data.Repository;
using OkBlog.Models.Db;
using OkBlog.Models.Db.Comments;
using OkBlog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OkBlog.Controllers
{
    public class PostsController : Controller
    {
        private IRepository _repo;
        private ITagRepository _tagRepo;
        private IFileManager _fileManager;
        private readonly ILogger<PostsController> _logger;
        UserManager<ApplicationUser> _userManager;
        private ICommentRepository _commentRepo;

        public PostsController(IRepository repo, ITagRepository tagRepo, IFileManager fileManager, ILogger<PostsController> logger, UserManager<ApplicationUser> userManager, ICommentRepository commentRepo)
        {
            _repo = repo;
            _tagRepo = tagRepo;
            _fileManager = fileManager;
            _logger = logger;
            _userManager = userManager;
            _commentRepo = commentRepo;
        }

        public IActionResult Index(string category)
        {
            var posts = string.IsNullOrEmpty(category) ? _repo.GetAllPosts() : _repo.GetAllPosts(category);
            _logger.LogInformation("PostsController Invoked");
            //_logger.LogError("Exception throw...");

            _logger.LogDebug("Произведена выборка всех статей");
            return View(posts);
        }

        public IActionResult Post(int id)
        {
            _logger.LogTrace("Запрашиваемый id статьи: " + id);
            var post = _repo.GetPost(id);
            if (post is null)
            {
                _logger.LogError("Ошибка. Не найдена статья с указанным id: " + id);
                return RedirectToAction("Index");
            }
            var tagsId = post.Tags.Select(x => x.Id).ToList();
            var postTags = _tagRepo.GetAllTags().Select(t => new TagViewModel { Id = t.Id, Name = t.Name, IsSelected = tagsId.Contains(t.Id) }).ToList();
            var userEmail = _userManager.GetUserName(HttpContext.User);

            PostViewModel model = new PostViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                Category = post.Category,
                Image = post.Image,
                Author = post.Author,
                Tags = postTags,
                MainComments = post.MainComments,
                CurrentUserName = userEmail
            };
            _logger.LogTrace("Выборка прошла успешно. Выбрана статья с id: " + post.Id);

            return View(model);
        }

        [HttpGet("/Image/{image}")]
        public IActionResult Image(string image)
        {
            var mime = image.Substring(image.LastIndexOf('.') + 1);
            return new FileStreamResult(_fileManager.ImageStream(image), $"image/{mime}");
        }

        [HttpPost]
        public async Task<IActionResult> Comment(CommentViewModel model)
        {
            var userEmail = _userManager.GetUserName(HttpContext.User);
            var userId = _userManager.GetUserId(HttpContext.User);


            if (!ModelState.IsValid)
                return RedirectToAction("Post", new { id = model.PostId });

            var post = _repo.GetPost(model.PostId);
            if(model.MainCommentId == 0)
            {
                post.MainComments = post.MainComments ?? new List<MainComment>();
                var comment = _commentRepo.GetComment(model.MainCommentId);

                post.MainComments.Add(new MainComment
                {
                    Message = model.Message,
                    Created = DateTime.Now,
                    Author = userEmail,
                    PostId = post.Id,
                    UserId = userId
                });

                PostViewModel vm = new PostViewModel();
                vm.MainComments.Add(new MainComment
                {
                    Id = model.PostId,
                    Message = model.Message,
                    Created = DateTime.Now,
                    Author = userEmail,
                    PostId = post.Id,
                    UserId = userId
                });
                vm.CurrentUserName = userEmail;

                _repo.UpdatePost(post);                
            }
            else
            {
                var comment = new SubComment
                {
                    MainCommentId = model.MainCommentId,
                    Message = model.Message,
                    Created = DateTime.Now,
                    Author = userEmail,
                    PostId = post.Id,
                    UserId = userId
                };
                _repo.AddSubComment(comment);
            }

            await _repo.SaveChangesAsync();

            return RedirectToAction("Post", new { id = model.PostId });
        }

        [HttpGet]
        public async Task<IActionResult> RemoveComment(int id)
        {
            var post = _repo.GetPost(id);

            if (post != null)
            {
                var comment = post.MainComments.FirstOrDefault(x => x.Id == 1);

                var comment2 = post.MainComments.Remove(comment);

                if (comment2)
                {
                    _repo.UpdatePost(post);
                }
            }

            await _repo.SaveChangesAsync();
            return RedirectToAction("Post", new { id = id });

        }
    }
}
