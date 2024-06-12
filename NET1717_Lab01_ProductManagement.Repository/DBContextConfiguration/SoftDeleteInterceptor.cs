using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using NET1717_Lab01_ProductManagement.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1717_Lab01_ProductManagement.Repository.DBContextConfiguration
{
    public class SoftDeleteInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            if (eventData.Context is null) return result;

            foreach (var entry in eventData.Context.ChangeTracker.Entries())
            {
                if (entry is not { State: EntityState.Deleted, Entity: ISoftDelete delete }) continue;
                entry.State = EntityState.Modified;
                delete.IsDeleted = true;
                delete.DeletedAt = DateTimeOffset.UtcNow;
            }
            return result;
        }
    }
}
