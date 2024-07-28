using Core.Entities.Base;
using Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Base
{
    public class Repistory<T> : IRepistory<T> where T : BaseEntity
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbTable;

        public Repistory(AppDbContext context)
        {
            //_context = new();
            _context = context;
            _dbTable = _context.Set<T>();
        }

        public List<T> GetAll()
        {
            return _dbTable.ToList();
        }
        public T Get(int id)
        {
            return _dbTable.FirstOrDefault(x=>x.Id==id);
        }


        public void Add(T item)
        {
            item.CreateAt = DateTime.Now;
            _dbTable.Add(item);
        }

        public void Update(T item)
        {
            item.ModifyAt = DateTime.Now;
            _dbTable.Update(item);
        }
        public void Delete(T item)
        {
            //_dbTable.Remove(item);
            _dbTable.Update(item);
        }

    }
}
