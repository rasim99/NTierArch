using Core.Entities;
using Data.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Abstract
{
    public interface IGroupRepistory :IRepistory<Group>
    {
        Group GetByName(string groupName);

       Group GetByNameWithStudents(string name);
    }
}
