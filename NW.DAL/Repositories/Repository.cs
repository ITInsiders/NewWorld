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
        private static Object Lock = new Object();

        public Repository(Context context)
        {
            this.db = context;
        }
        public List<T> GetAll()
        {
            lock (Lock)
            {
                return db.Set<T>().ToList();
            }
        }
        public List<T> Find(Func<T, Boolean> predicate)
        {
            lock (Lock)
            {
                return db.Set<T>().Where(predicate).ToList();
            }
        }
        public T Get(int id)
        {
            lock(Lock)
            {
                return db.Set<T>().Find(id);
            }
        }
        public void Create(T t)
        {
            lock (Lock)
            {
                db.Set<T>().Add(t);
            }
        }
        public void Update(T t)
        {
            lock (Lock)
            {
                db.Entry(t).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            lock (Lock)
            {
                db.Set<T>().Remove(Get(id));
            }
        }
    }
}
