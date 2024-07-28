using Data.Contexts;
using Data.Repositories.Concrete;
using Data.UnitOfWork.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.UnitOfWork.Concrete
{
    public class UnitOfWork :IUnitOfWork
    {
       public readonly GroupRepistory Groups;
       public readonly StudentRepistory Students;
        private readonly AppDbContext _context;
        public UnitOfWork()
        {
            _context = new();
            Groups = new(_context); 
            Students = new(_context); 
        }

        public void Commit()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {

                Console.WriteLine("Errror ocured");
            }
        }
    }
}
