using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OkBlog.Data.Repository;
using OkBlog.Models.Db.Comments;
using OkBlog.ViewModels;

namespace OkBlog.Controllers
{
    public class CommentsController : Controller
    {
        private IRepository _repo;
        private ICommentRepository _commentRepo;

        public CommentsController(IRepository repo, ICommentRepository commentRepoepo)
        {
            _repo = repo;
            _commentRepo = commentRepoepo;
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var comment = _commentRepo.GetComment(id);
            //var postId = comment.PostId;
            return View(comment);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MainComment comment)
        {
            if (comment.Id > 0)
                _commentRepo.UpdateComment(comment);
            else
                _commentRepo.AddComment(comment);

            if (await _commentRepo.SaveChangesAsync())
            {
                return RedirectToAction("Post", "Posts", new { id = comment.PostId });
            }
            else
            {
                return View(comment);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            // получаем комментарий для определения id поста, чтобы после удаления комментария перенаправляться на ту же страницу с постом
            var comment = _commentRepo.GetComment(id);
            var postId = comment.PostId;

            _commentRepo.RemoveComment(id);
            await _commentRepo.SaveChangesAsync();
            return RedirectToAction("Post", "Posts", new{id= postId });
        }
    }
}
