using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using src.Family.Accounts.Management.Infrastructure.Enums;

namespace Family.Accounts.Management.Infrastructure.Requests
{
    public class AppRequest
    {
        [Required]
        public int? Code { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Slug { get; set; }
        public string? CallbackUrl { get; set; }
        [Required]
        public StatusEnum? Status { get; set; }
    }
}