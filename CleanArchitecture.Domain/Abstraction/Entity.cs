using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Abstraction
{
    public abstract class Entity
    {
        public Entity()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
