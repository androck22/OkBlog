using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
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
        private readonly ILogger<PostsController> _logger;
        UserManager<ApplicationUser> _userManager;

        public PostsController(IRepository repo, ITagRepository tagRepo, ILogger<PostsController> logger, UserManager<ApplicationUser> userManager)
        {
            _repo = repo;
            _tagRepo = tagRepo;
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var posts = _repo.GetAllPosts();
            _logger.LogInformation("PostsController Invoked");
            try
            {
                var val = 1;
                var i = val / 0;
            }
            catch (Exception)
            {
                _logger.LogError("Exception throw...");
            }
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
                Author = post.Author,
                Tags = postTags,
                MainComments = post.MainComments,
                CurrentUserName = userEmail
            };
            _logger.LogTrace("Выборка прошла успешно. Выбрана статья с id: " + post.Id);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Comment(CommentViewModel model)
        {
            var userEmail = _userManager.GetUserName(HttpContext.User);


            if (!ModelState.IsValid)
                return RedirectToAction("Post", new { id = model.PostId });

            var post = _repo.GetPost(model.PostId);
            if(model.MainCommentId == 0)
            {
                post.MainComments = post.MainComments ?? new List<MainComment>();

                post.MainComments.Add(new MainComment
                {
                    Message = model.Message,
                    Created = DateTime.Now,
                    UserName = userEmail,
                });

                PostViewModel vm = new PostViewModel();
                vm.MainComments.Add(new MainComment
                {
                    Id = model.PostId,
                    Message = model.Message,
                    Created = DateTime.Now,
                    UserName = userEmail,
                });

                _repo.UpdatePost(post);                
            }
            else
            {
                var comment = new SubComment
                {
                    MainCommentId = model.MainCommentId,
                    Message = model.Message,
                    Created = DateTime.Now,
                    UserName = userEmail,
                };
                _repo.AddSubComment(comment);
            }

            await _repo.SaveChangesAsync();

            return RedirectToAction("Post", new { id = model.PostId });
        }
    }
}
