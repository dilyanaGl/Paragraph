using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
namespace Paragraph.Data.Common
{
    public interface IRepository<TEntity> 
        where TEntity : class
    {

        IQueryable<TEntity> All();

        void AddAsync(TEntity entity);

        void Delete(TEntity entity);

        void AddMany(IEnumerable<TEntity> entities);
               
        void SaveChangesAsync();
    }
}
