using StokKontrolProje.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StokKontrolProje.Repository.Abstract
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        bool Add(T item);
        bool Add(List<T> items);
        bool Update(T item);
        bool Remove(T item);
        bool Remove(int id);
        bool RemoveAll(Expression<Func<T,bool>> exp);

        T GetById(int id);

        IQueryable<T> GetById(int id, params Expression<Func<T, object>>[] includes);

        T GetByDefault(Expression<Func<T, bool>> exp);

        List<T> GetActive();

        IQueryable<T> GetActive( params Expression<Func<T, object>>[] includes);

        List<T> GetDefault(Expression<Func<T, bool>> exp);

        List<T> GetAll();
        IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes);

        IQueryable<T> GetAll(Expression<Func<T, bool>> exp, params Expression<Func<T, object>>[] includes);

        bool Activate(int id);
        bool Any(Expression<Func<T, bool>> exp);
        int Save();

        void DetachEntity(T item);



    }
}
