using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paragraph.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Paragraph.Data
{
    public class DbRepository<TEntity> : IRepository<TEntity>, IDisposable
        where TEntity : class
    {
        private readonly ParagraphContext context;

        private DbSet<TEntity> dbSet;



        public DbRepository(ParagraphContext context)
        {
            this.context = context;

            this.dbSet = this.context.Set<TEntity>();

        }

        public void AddAsync(TEntity entity)
        {
            this.dbSet.Add(entity);
        }

        public void AddMany(IEnumerable<TEntity> entities)
        {
            this.dbSet.AddRange(entities);
        }

        public IQueryable<TEntity> All()
        {
            return this.dbSet;
        }

        public void Delete(TEntity entity)
        {
            this.dbSet.Remove(entity);
        }

        public void Dispose()
        {
            this.context.Dispose();
        }

        public void SaveChangesAsync()
        {
            this.context.SaveChanges();
        }
    }
}
 