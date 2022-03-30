using Microsoft.AspNetCore.Identity;
using OkBlog.Models.Db;
using System;
using System.Threading.Tasks;

namespace OkBlog
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            string adminEmail = "admin@gmail.com";
            string password = "_Aa123456";
            if (await roleManager.FindByNameAsync("User") == null)
            {
                await roleManager.CreateAsync(new ApplicationRole("User", "Стандартная роль приложения, присваивается всем пользователям по умолчанию"));
            }
            if (await roleManager.FindByNameAsync("Admin") == null)
            {
                await roleManager.CreateAsync(new ApplicationRole("Admin", "Роль с максимальными возможностями в приложении. Данная роль позволяет выполнять управление, редактирование, удаление ролей и пользователей в приложении"));
            }
            if (await roleManager.FindByNameAsync("Moderator") == null)
            {
                await roleManager.CreateAsync(new ApplicationRole("Moderator", "Данная роль позволяет выполнять редактирование, удаление комментариев, тегов и статей в приложении"));
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                DateTime dateTime = DateTime.Now;
                ApplicationUser admin = new ApplicationUser { Email = adminEmail, UserName = adminEmail, FirstName = "Admin", Created = dateTime };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                    await userManager.AddToRoleAsync(admin, "Moderator");
                    await userManager.AddToRoleAsync(admin, "User");
                }
            }
        }
    }
}
