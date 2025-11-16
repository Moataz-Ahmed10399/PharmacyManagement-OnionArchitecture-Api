using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.Interface;
using Pharmacy.Context;
using Pharmacy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Infrastructure
{
    public class GenericRepo<T> : IGenericRepo<T> where T : BaseEntity
    {
        private readonly MyDbContext _context;
        private readonly DbSet<T> _dbSet;
        public GenericRepo(MyDbContext myDb )
        {
            _context = myDb;
            _dbSet = myDb.Set<T>(); 
        }


        public async Task<T> Create(T entity)
        {
           return (await _context.AddAsync(entity)).Entity;  //keda howa hyraga3 al enitty al at3mlH creat
        }

        public async Task<T> Delete(T entity)
        {
            return await Task.FromResult(_context.Remove(entity).Entity);
        }

        public  IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
                Console.WriteLine($"Applying include: {include}");

            }
            return query;   


            //return await Task.FromResult(_dbSet) ;


            //return _context.categories ده مش صح لامها جينارك و لو عملت T برضو هحتاج اقوله dbset اللي من نوع T 
            // ف ده الحل الوحيد عشان معملش كل مرا _Context.set<T> () انا عملتها فوق و بقيت انادمها بس تحت 
        }

        public async Task<T?> GetById(int id)
        {
            return await (_dbSet.FindAsync(id));  //findasynce بترجع valueoftask
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public Task<T> Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
