using Moujam.Casiher.Comman.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.Entities
{
    public class Customer: AuditEntity<int>
    {
    
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public string? MappedId { get; set; }

    }
}
