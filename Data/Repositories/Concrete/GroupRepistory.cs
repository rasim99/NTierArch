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
    public class GroupRepistory : Repistory<Group>, IGroupRepistory
    {
        private readonly AppDbContext _context;
        public GroupRepistory(AppDbContext context) :base(context)
        {
            _context=context;
        }

        public Group GetByNameWithStudents(string name)
        {
           return _context.Groups.Include(x => x.Students).FirstOrDefault(x=>x.Name==name);
        }


        public Group GetByName(string groupName)
        {
            return _context.Groups.FirstOrDefault(g=>g.Name.ToLower()==groupName.ToLower());
        }
    }
}
