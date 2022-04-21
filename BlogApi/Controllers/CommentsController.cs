using BlogApi.Contracts.Models.Comments;
using BlogApi.Data.Models;
using BlogApi.Data.Models.Comments;
using BlogApi.Data.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogApi.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("[controller]")]
    public class CommentsController : Controller
    {
        private IRepository _repo;
        private ITagRepository _tagRepo;
        UserManager<ApplicationUser> _userManager;

        public CommentsController(IRepository repo, ITagRepository tagRepo, UserManager<ApplicationUser> userManager)
        {
            _repo = repo;
            _tagRepo = tagRepo;
            _userManager = userManager;
        }

        /// <summary>
        /// Добавление комментария к статье
        /// </summary>
        /// <remarks>
        /// POST /Comments/Add
        /// </remarks>
        /// <param name="model">AddCommentRequest object</param>
        /// <returns>Добавляет новый комментарий к статье</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpPost]
        [Route("Add")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Comment(AddCommentModel model)
        {
            var userEmail = _userManager.GetUserName(HttpContext.User);

            int mainCommentId = 0;

            var post = _repo.GetPostById(model.PostId);
            if (mainCommentId == 0)
            {
                post.MainComments = post.MainComments ?? new List<MainComment>();

                post.MainComments.Add(new MainComment
                {
                    Message = model.Message,
                    Created = DateTime.Now,
                    UserName = userEmail,
                });

                PostModel vm = new PostModel();
                vm.MainComments.Add(new MainComment
                {
                    Id = model.PostId,
                    Message = model.Message,
                    Created = DateTime.Now,
                    UserName = userEmail,
                });

                await _repo.UpdatePost(post);
            }
            else
            {
                var comment = new SubComment
                {
                    MainCommentId = mainCommentId,
                    Message = model.Message,
                    Created = DateTime.Now,
                    UserName = userEmail,
                };
                await _repo.AddSubComment(comment);
            }

            await _repo.SaveChangesAsync();

            return StatusCode(200, $"Комментарий к посту с id: \"{model.PostId}\", автора {userEmail}, добавлен!");
        }
    }
}
