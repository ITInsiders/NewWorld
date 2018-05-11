using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NW.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        List<T> GetAll();
        T Get(int id);
        List<T> Find(Func<T, Boolean> predicate);
        void Create(T item);
        void Update(T item);
        void Delete(int id);

        /*
        void Create(T item);
        void Update(T item);
        void Delete(T item);
        void Delete(Func<T, Boolean> predicate);
        IEnumerable<T> Find(Func<T, Boolean> predicate);
        IEnumerable<TResult> Select<TResult>(T t);
        */
    }
}
