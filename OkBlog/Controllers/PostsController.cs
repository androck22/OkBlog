using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OkBlog.Data;
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
        private readonly ILogger<PanelController> _logger;
        UserManager<ApplicationUser> _userManager;

        public PostsController(IRepository repo, ITagRepository tagRepo, ILogger<PanelController> logger, UserManager<ApplicationUser> userManager)
        {
            _repo = repo;
            _tagRepo = tagRepo;
            _logger = logger;
            _logger.LogDebug(1, "NLog injected into HomeController");
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Hello, this is the index!");

            var posts = _repo.GetAllPosts();
            return View(posts);
        }

        public IActionResult Post(int id)
        {
            var post = _repo.GetPost(id);

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
