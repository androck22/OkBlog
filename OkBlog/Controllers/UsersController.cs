using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OkBlog.Models.Db;
using OkBlog.ViewModels;
using OkBlog.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OkBlog.Controllers
{
    public class UsersController : Controller
    {
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        RoleManager<ApplicationRole> _roleManager;
        private IRepository _postRepo;
        private readonly ILogger<PostsController> _logger;

        public UsersController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, IRepository postRepo, ILogger<PostsController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _postRepo = postRepo;
            _logger = logger;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(string searchingId)
        {
            _logger.LogInformation("UsersController Invoked");

            var users = _userManager.Users.ToList();
            if (searchingId is null)
            {
                _logger.LogDebug("Произведена выборка всех пользователей");
                users = _userManager.Users.ToList();
            }
            else
            {
                _logger.LogTrace("Запрашиваемый id пользователя: " + searchingId);
                users = _userManager.Users.Where(x => x.Id == searchingId).ToList();
            }

            List<UserViewModel> models = new List<UserViewModel>();

            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                List<string> roles = new List<string>();

                foreach (var role in userRoles)
                {
                    roles.Add(role.ToString());
                }

                models.Add(new UserViewModel { 
                    Id = user.Id, Email = user.Email, FirstName = user.FirstName, LastName = user.LastName, Created = user.Created, Roles = roles
                });
            }
            return View(models);
        }

        public IActionResult GetUser()
        {
            var userId = _userManager.GetUserId(HttpContext.User);            

            ApplicationUser user = _userManager.Users.FirstOrDefault(x => x.Id == userId);

            var model = new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Created = user.Created
            };

            return View(model);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser { FirstName = model.FirstName, LastName = model.LastName, Email = model.Email, UserName = model.Email, Created = DateTime.Now };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    _logger.LogTrace($"Добавление пользователя: {user.Email} c Id: {user.Id}");

                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [Route("UserSearch")]
        [HttpPost]
        public async Task<IActionResult> Search(string id)
        {
            var model = new SearchViewModel
            {
                UserSearch = await _userManager.FindByIdAsync(id)
            };

            return View("Search", model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            EditUserViewModel model = new EditUserViewModel { Id = user.Id, Email = user.Email, FirstName = user.FirstName, LastName = user.LastName };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {                   
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Email = model.Email;
                    user.UserName = model.Email;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        _logger.LogTrace($"Редактирование пользователя: {model.Email} с Id: " + model.Id);

                        if (User.IsInRole("Admin"))
                        {
                            return RedirectToAction("Index", "Users");
                        }

                        return RedirectToAction("GetUser", "Users");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            var userPosts = _postRepo.GetAllPosts().Where(p => p.UserId == id);
            if (user != null)
            {
                if (user.Posts != null)
                {
                    foreach (var post in user.Posts)
                    {
                        _postRepo.RemovePost(post.Id);
                        await _postRepo.SaveChangesAsync();
                        _logger.LogTrace("Удаление статьи с id: " + post.Id);
                    }
                }

                _logger.LogTrace($"Удаление пользователя: {user.Email} c Id: {id}");

                IdentityResult result = await _userManager.DeleteAsync(user);
            }

            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Users");
            }

            return RedirectToAction("Login", "Account");
        }
    }
}
