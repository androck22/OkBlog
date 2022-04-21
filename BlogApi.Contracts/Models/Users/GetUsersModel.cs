using System;
using System.Collections.Generic;

namespace BlogApi.Contracts.Models.Users
{
    public class GetUsersModel
    {
        public int UserAmount { get; set; }
        public List<UserView> Users { get; set; }
    }

    public class UserView
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Created { get; set; }
    }
}
