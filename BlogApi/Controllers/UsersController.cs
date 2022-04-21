using AutoMapper;
using BlogApi.Contracts.Models.Users;
using BlogApi.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApi.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IMapper _mapper;
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        RoleManager<IdentityRole> _roleManager;

        public UsersController(IMapper mapper, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Просмотр списка пользователей
        /// </summary>
        /// <remarks>
        /// GET /Users
        /// </remarks>
        /// <returns>Returns All Users</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpGet]
        [Route("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Get()
        {
            var users = _userManager.Users.ToList();

            var request = new GetUsersModel
            {
                UserAmount = users.Count,
                Users = _mapper.Map<List<ApplicationUser>, List<UserView>>(users)
            };

            return StatusCode(200, request);
        }

        /// <summary>
        /// Просмотр пользователя по id
        /// </summary>
        /// <remarks>
        /// POST /Users/id
        /// </remarks>
        /// <param name="id">User id (string)</param>
        /// <returns>Возвращает запрашиваемого пользователя</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpPost]
        [Route("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return StatusCode(400, $"Ошибка: Пользователь с данным id не найден. Проверьте корректность ввода!");

            return StatusCode(200, user);
        }

        /// <summary>
        /// Добавление нового пользователя
        /// </summary>
        /// <remarks>
        /// POST /Users/Add
        /// </remarks>
        /// <param name="model">AddUserModel object</param>
        /// <returns>Добавляет нового пользователя</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpPost]
        [Route("Add")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Add([FromBody] AddUserModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                    return StatusCode(400, $"Ошибка: Данный email {model.Email} уже зарегестрирован!");

                ApplicationUser newUser = new ApplicationUser { 
                    FirstName = model.FirstName, 
                    LastName = model.LastName, 
                    Email = model.Email, 
                    UserName = model.Email, 
                    Created = DateTime.Now };
                var result = await _userManager.CreateAsync(newUser, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, "User");
                    await _signInManager.SignInAsync(newUser, false);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return StatusCode(200, $"Пользователь: {model.FirstName} {model.LastName}, добавлен!");
        }

        /// <summary>
        /// Редактирование информации о пользователе
        /// </summary>
        /// <remarks>
        /// PATCH /Users/954c13b5-9e47-4670-a1ff-8cf9ecfdfc4b
        /// </remarks>
        /// <param name="id">User id (string)</param>      
        /// <param name="model">EditUserModel object</param>
        /// <returns>Возвращает отредактированного пользователя</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpPatch]
        [Route("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Edit([FromRoute] string id, [FromBody] EditUserModel model)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return StatusCode(400, $"Ошибка: Пользователь: {model.Email} не зарегестрирован. Сначало пройдите регистрацию!");

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.UserName = model.Email;

            await _userManager.UpdateAsync(user);

            return StatusCode(200, $"Информация о пользователе: {model.FirstName} {model.LastName}, обновлена!");
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <remarks>
        /// DELETE /User/954c13b5-9e47-4670-a1ff-8cf9ecfdfc4b
        /// </remarks>
        /// <param name="id">User id (string)</param>      
        /// <returns>Возвращает: NoContent</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpDelete]
        [Route("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return StatusCode(400, $"Ошибка: Пользователь с идентификатором: {id} не существует!");

            await _userManager.DeleteAsync(user);

            return StatusCode(200, $"Пользователь: \"{user.Email}\", с идентификатором: {user.Id} удален!");
        }
    }
}
