﻿using System.ComponentModel.DataAnnotations;

namespace AuthenticationJwt.Web.Models
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public DateTime DateofJoining { get; set; }
    }
}
