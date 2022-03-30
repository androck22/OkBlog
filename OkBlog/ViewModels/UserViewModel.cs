using OkBlog.Models.Db;
using System;
using System.Collections.Generic;

namespace OkBlog.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Created { get; set; }
        public List<string> Roles { get; set; }
    }
}
