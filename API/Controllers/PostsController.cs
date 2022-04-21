using API.Contracts;
using API.Contracts.Posts;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Text;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostsController : ControllerBase
    {
        private IOptions<Post> _options;
        private IMapper _mapper;

        public PostsController(IOptions<Post> options, IMapper mapper)
        {
            _options = options;
            _mapper = mapper; 
        }

        /// <summary>
        /// Просмотр списка статей
        /// </summary>
        [HttpGet]
        [Route("")]
        public IActionResult Get()
        {
            return StatusCode(200, "Статьи отсутствуют");
        }

        /// <summary>
        /// Добавление новой статьи
        /// </summary>
        [HttpPost]
        [Route("Add")]
        public IActionResult Add(
            [FromBody]
            AddPostRequest request)
        {
            return StatusCode(200, $"Статья: \"{request.Title}\", добавлена!");
        }

        /// <summary>
        /// Метод для получения информации о статьях
        /// </summary>
        [HttpGet] // Для обслуживания Get-запросов
        [Route("info")] // Настройка маршрута с помощью атрибутов
        public IActionResult Info()
        {
            // Получим запрос, "смапив" конфигурацию на модель запроса
            var postResponse = _mapper.Map<Post, PostRequest>(_options.Value);
            // Вернём ответ

            return StatusCode(200, postResponse);
        }
    }
}
