﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebAPI_GiftManagement_BaoTran.Data;

namespace WebAPI_GiftManagement_BaoTran.ReRepository
{
    public interface IRepositoryBase<T> where T : class
    {
        int CreateTransaction(T entity);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<int> Count(Expression<Func<T, bool>> expression = null);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression = null);
        Task<T?> GetSingleAsync(Expression<Func<T, bool>> expression = null);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> expression = null);
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression = null);


    }

    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly MyDbContext _db;
        public RepositoryBase(MyDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression = null)
        {
            if (expression == null)
            {
                return await _db.Set<T>().ToListAsync();
            }
            return await _db.Set<T>().Where(expression).ToListAsync();
        }

        public async Task<T?> GetSingleAsync(Expression<Func<T, bool>> expression = null)
        {
            return await _db.Set<T>().SingleOrDefaultAsync(expression);
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> expression = null)
        {
            return await _db.Set<T>().FirstOrDefaultAsync(expression);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression = null)
        {
            if (expression == null)
            {
                return await _db.Set<T>().AnyAsync();
            }
            else
            {
                return await _db.Set<T>().AnyAsync(expression);
            }
        }



        public void Create(T entity)
        {
            _db.Set<T>().Add(entity);
        }

        public int CreateTransaction(T entity)
        {
            _db.Set<T>().Add(entity);
            _db.SaveChanges();
            object? keyProperty = _db.Entry(entity).Property("IdTransaction").CurrentValue;
            return (int)keyProperty;
        }



        public void Update(T entity)
        {
            _db.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            _db.Set<T>().Remove(entity);
        }

        public async Task<int> Count(Expression<Func<T, bool>> expression = null)
        {
            return await _db.Set<T>().CountAsync(expression);
        }

        //public int Count(Expression<Func<T, bool>> expression = null)
        //{
        //    return _db.Set<T>().Count(expression);
        //}



    }
}
