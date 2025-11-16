using Pharmacy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Application.Interface
{
    public interface IGenericRepo<T>  where T : BaseEntity
    {
        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes);
        public Task<T> GetById(int id);
        public Task<T> Create(T entity);
        public Task<T> Update(T entity);
        public Task<T> Delete(T entity);
        public Task<int> SaveChangesAsync();

    }
}
