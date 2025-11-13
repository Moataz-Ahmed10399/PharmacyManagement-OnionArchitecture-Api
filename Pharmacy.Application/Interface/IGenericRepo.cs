using Pharmacy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Application.Interface
{
    public interface IGenericRepo<T>  where T : BaseEntity
    {
        public Task<IQueryable<T>> GetAll();
        public Task<T> GetById();
        public Task<T> Create(T entity);
        public Task<T> Update(T entity);
        public Task<T> Delete(T entity);

    }
}
