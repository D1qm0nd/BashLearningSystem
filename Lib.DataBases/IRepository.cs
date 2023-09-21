using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.DataBases.Exceptions;

namespace Lib.DataBases
{
    public interface IRepository
    {
        public DbSet<T>? GetEntity<T>() where T : Entity
        {
            var type = this.GetType();
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(DbSet<T>))
                {
                    var prop = (property.GetValue(this) as DbSet<T>);
                    return prop;
                }
            }
            return null;
        }
        
        public int SaveRepositoryChanges();

        public void Migrate();

        public void Drop();
    }


}
