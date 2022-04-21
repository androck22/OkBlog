using AutoMapper;
using BlogApi.Contracts.Models.Responses;
using BlogApi.Contracts.Models.Tags;
using BlogApi.Data.Models;
using BlogApi.Data.Queries;
using BlogApi.Data.Repository;
using BlogApi.SwaggerExamples.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using static BlogApi.Contracts.Models.Tags.GetTagsModel;

namespace BlogApi.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("[controller]")]
    public class TagsController : Controller
    {
        private IMapper _mapper;
        private IRepository _repo;
        private ITagRepository _tagRepo;

        public TagsController(IMapper mapper, IRepository repo, ITagRepository tagRepo)
        {
            _mapper = mapper;
            _repo = repo;
            _tagRepo = tagRepo;
        }

        /// <summary>
        /// Просмотр списка тегов
        /// </summary>
        /// <remarks>
        /// GET /Tags
        /// </remarks>
        /// <returns>Returns All Tags</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpGet]
        [Route("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetAll()
        {
            var tags = _tagRepo.GetAllTags();

            var request = new GetTagsModel
            {
                TagAmount = tags.Count,
                Tags = _mapper.Map<List<Tag>, List<TagView>>(tags)
            };

            if (request is null)
            {

            }

            return StatusCode(200, request);
        }

        /// <summary>
        /// Просмотр тега по id
        /// </summary>
        /// <remarks>
        /// POST /Tags/1
        /// </remarks>
        /// <param name="id">Tag id (int)</param>
        /// <returns>Возвращает запрашиваемый тег</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpPost]
        [Route("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetById(int id)
        {
            var tag = _tagRepo.GetById(id);

            if (tag == null)
                return StatusCode(400, $"Ошибка: Тег с id: \"{id}\" не найден. Проверьте корректность ввода!");

            return StatusCode(200, tag);
        }

        /// <summary>
        /// Добавление нового тега
        /// </summary>
        /// <remarks>
        /// POST /Tags/Add
        /// </remarks>
        /// <param name="model">AddTagRequest object</param>
        /// <returns>Добавляет новый тег</returns>
        /// <response code="201">Create a tag in the system</response>
        /// <response code="400">Unable to create the tag due to validation error</response>
        [HttpPost]
        [Route("Add")]
        //[Authorize]
        [ProducesResponseType(typeof(TagResponse), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> Add([FromBody] AddTagModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Unable to create tag" } } });
            }

            var newTag = new Tag()
            {
                Name = model.Name
            };
            

            await _tagRepo.AddTag(newTag);

            return StatusCode(201, $"Тег: \"{model.Name}\", добавлен!");
        }

        /// <summary>
        /// Редактирование тега по id
        /// </summary>
        /// <remarks>
        /// PATCH /Tags/1
        /// </remarks>
        /// <param name="id">Tag id (int)</param>      
        /// <param name="model">EditTagRequest object</param>
        /// <returns>Возвращает обновленное название тега</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpPatch]
        [Route("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] EditTagModel model)
        {
            var tag = _tagRepo.GetById(id);
            if (tag == null)
                return StatusCode(400, $"Ошибка: Тег с идентификатором {id} не существует!");

            await _tagRepo.UpdateTag(
                tag,
                new UpdateTagQuery(model.Name)
            );

            return StatusCode(200, $"Тег: \"{tag.Name}\" обновлен!");
        }

        /// <summary>
        /// Удаление тега по id
        /// </summary>
        /// <remarks>
        /// DELETE /Tags/1
        /// </remarks>
        /// <param name="id">Tag id (int)</param>      
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
            var tag = _tagRepo.GetById(id);
            if (tag == null)
                return StatusCode(400, $"Ошибка: Тег с идентификатором {id} не существует!");

            await _tagRepo.RemoveTag(id);

            return StatusCode(200, $"Тег: \"{tag.Name}\", с идентификатором: {tag.Id} удален!");
        }
    }
}
