using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using src.Family.Accounts.Management.Infrastructure.Enums;

namespace Family.Accounts.Management.Infrastructure.Requests
{
    public class UserRequest
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        public Guid? ProfileId { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public StatusEnum? Status { get; set; }
    }
}