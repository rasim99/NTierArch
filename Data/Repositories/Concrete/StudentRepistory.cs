using Core.Entities;
using Data.Contexts;
using Data.Repositories.Abstract;
using Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Concrete
{
    public class StudentRepistory : Repistory<Student>, IStudentRepistory
    {
        private readonly AppDbContext _context;

        public StudentRepistory(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public Student GetByIdWithGroup(int id)
        {
            return _context.Students.Include(s => s.Group).FirstOrDefault(x=>x.Id==id);
        }

   
    }
}
