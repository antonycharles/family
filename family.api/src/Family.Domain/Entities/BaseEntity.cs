using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Family.Domain.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}