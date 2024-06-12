using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1717_Lab01_ProductManagement.Repository.Entities
{
    public interface ISoftDelete
    {
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public void Undo()
        {
            IsDeleted = false;
            DeletedAt = null;
        }
    }
}
