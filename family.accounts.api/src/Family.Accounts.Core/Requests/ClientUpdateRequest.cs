using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Family.Accounts.Core.Enums;

namespace Family.Accounts.Core.Requests
{
    public class ClientUpdateRequest
    {
        
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public Guid? ProfileId { get; set; }

        public string? Password { get; set; }
        public StatusEnum? Status { get; set; }
    }
}