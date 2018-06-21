using System;
using System.Threading;
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
        private static Mutex mutexLock = new Mutex();

        public Repository(Context context)
        {
            this.db = context;
        }
        public List<T> GetAll()
        {
            mutexLock.WaitOne();
            List<T> list = db.Set<T>().ToList();
            mutexLock.ReleaseMutex();
            return list;
        }
        public List<T> Find(Func<T, Boolean> predicate)
        {
            mutexLock.WaitOne();
            List<T> list = db.Set<T>().Where(predicate).ToList();
            mutexLock.ReleaseMutex();
            return list;
        }
        public T Get(int id)
        {
            mutexLock.WaitOne();
            T element = db.Set<T>().Find(id);
            mutexLock.ReleaseMutex();
            return element;
        }
        public void Create(T t)
        {
            mutexLock.WaitOne();
            db.Set<T>().Add(t);
            mutexLock.ReleaseMutex();
        }
        public void Update(T t)
        {
            mutexLock.WaitOne();
            db.Entry(t).State = EntityState.Modified;
            mutexLock.ReleaseMutex();
        }

        public void Delete(int id)
        {
            mutexLock.WaitOne();
            db.Set<T>().Remove(Get(id));
            mutexLock.ReleaseMutex();
        }
    }
}
