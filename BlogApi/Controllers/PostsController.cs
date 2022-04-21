using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Text;
using BlogApi.Contracts.Models.Posts;
using BlogApi.Data.Repository;
using BlogApi.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlogApi.Data.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using BlogApi.SwaggerExamples.Responses;
using BlogApi.Contracts.Models.Responses;

namespace BlogApi.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("[controller]")]
    public class PostsController : ControllerBase
    {
        private IMapper _mapper;
        private IRepository _repo;
        UserManager<ApplicationUser> _userManager;

        public PostsController(IMapper mapper, IRepository repo, UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _repo = repo;
            _userManager = userManager;

        }

        /// <summary>
        /// Просмотр списка статей
        /// </summary>
        /// <remarks>
        /// GET /Posts
        /// </remarks>
        /// <returns>Returns All Posts</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpGet]
        [Route("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetAll()
        {
            var posts = _repo.GetAllPosts();

            var request = new GetPostsModel
            {
                PostAmount = posts.Count,
                Posts = _mapper.Map<List<Post>, List<PostView>>(posts)
            };

            return StatusCode(200, request);
        }

        /// <summary>
        /// Просмотр статьи по названию
        /// </summary>
        /// <remarks>
        /// POST /Posts/title
        /// </remarks>
        /// <param name="title">Post title (string)</param>
        /// <returns>Возвращает запрашивоемую статью</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpPost]
        [Route("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetByTitle(string title)
        {
            var post = await _repo.GetPostByTitle(title);

            if(post == null)
                return StatusCode(400, $"Ошибка: Статья с названием: \"{title}\" не найдена. Проверьте корректность ввода!");

            return StatusCode(200, post);
        }

        /// <summary>
        /// Добавление новой статьи
        /// </summary>
        /// <remarks>
        /// POST /Posts/Add
        /// </remarks>
        /// <param name="model">AddPostRequest object</param>
        /// <returns>Добавляет новую статью</returns>
        /// <response code="201">Success</response>
        /// <response code="400">If the user is unauthorized</response>
        [HttpPost]
        [Route("Add")]
        //[Authorize]
        [ProducesResponseType(typeof(PostResponse), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> Add([FromBody] AddPostModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Author);

            if (user == null)
                return BadRequest(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel {FieldName = $"{ StatusCodes.Status400BadRequest }", Message = $"Ошибка: Пользователь {model.Author} не зарегестрирован. Сначало пройдите регистрацию!" } } });

            var newPost = _mapper.Map<AddPostModel, Post>(model);
            newPost.UserId = user.Id;
            newPost.Author = user.Email;

             await _repo.AddPost(newPost);

            return StatusCode(200, $"Статья: \"{model.Title}\", автора {user.Email}, добавлена!");
        }

        /// <summary>
        /// Редактирование статьи по id
        /// </summary>
        /// <remarks>
        /// PATCH /Posts/1
        /// </remarks>
        /// <param name="id">Post id (int)</param>      
        /// <param name="model">EditPostRequest object</param>
        /// <returns>Возвращает отредактированную статью</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpPatch]
        [Route("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] EditPostModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Author);
            if (user == null)
                return StatusCode(400, $"Ошибка: Пользователь {model.Author} не зарегестрирован. Сначало пройдите регистрацию!");

            var post = _repo.GetPostById(id);
            if (post == null)
                return StatusCode(400, $"Ошибка: Статья с идентификатором {id} не существует!");

            await _repo.UpdatePost(
                post,
                user,
                new UpdatePostQuery(model.Title, model.Body)
            );

            return StatusCode(200, $"Статья: \"{post.Title}\" обновлена!");
        }

        /// <summary>
        /// Удаление статьи по id
        /// </summary>
        /// <remarks>
        /// DELETE /Posts/1
        /// </remarks>
        /// <param name="id">Post id (int)</param>      
        /// <returns>Возвращает: NoContent</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpDelete]
        [Route("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var post = _repo.GetPostById(id);
            if (post == null)
                return StatusCode(400, $"Ошибка: Статья с идентификатором {id} не существует!");

            await _repo.RemovePost(id);

            return StatusCode(200, $"Статья: \"{post.Title}\", с идентификатором: {post.Id} удалена!");
        }
    }
}
