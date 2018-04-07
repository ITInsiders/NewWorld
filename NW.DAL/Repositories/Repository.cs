using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

using NW.DAL.Interfaces;
using NW.DAL.EF;

namespace NW.DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private Context db;

        public Repository(Context context)
        {
            this.db = context;
        }
        public IEnumerable<T> GetAll()
        {
            return db.Set<T>();
        }
        public IEnumerable<T> Find(Func<T, Boolean> predicate)
        {
            return db.Set<T>().Where(predicate);
        }
        public T Get(int id)
        {
            return db.Set<T>().Find(id);
        }
        public void Create(T t)
        {
            db.Set<T>().Add(t);
        }
        public void Update(T t)
        {
            db.Entry(t).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            db.Set<T>().Remove(Get(id));
        }

    }
}
