using Application.Services.Abstract;
using Core.Constants;
using Core.Entities;
using Core.Extentions;
using Data.Contexts;
using Data.Repositories.Concrete;
using Data.UnitOfWork.Concrete;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Concrete
{
    public class GroupService : IGroupService
    {
        private readonly UnitOfWork _unitOfWork;
        public GroupService()
        {
            _unitOfWork = new();
        }

        public void GetAllGroup()
        {
            if (GetAnyGroup()) return;

            foreach (var group in _unitOfWork.Groups.GetAll())
            {
                Console.WriteLine($" Id : {group.Id}  Name : {group.Name} limit  : {group.Limit}");
            }
        }
        public void GetDetailsGroup()
        {
            if (GetAnyGroup()) return;
            GroupNameSect: Messages.InputMessage("group name");
            GetAllGroup();
            string name=Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Messages.InvalidInputMessage("name");
                goto GroupNameSect;
            }
            var group = _unitOfWork.Groups.GetByName(name);
            if (group == null)
            {
                Messages.NotFoundMessage("group");
                return;
            }
            Console.WriteLine($" Id : {group.Id}  Name : {group.Name} limit  : {group.Limit} " +
                $" CreateAtDate : {group.CreateAt.ToString("dd.MM.yyyy")}  ModifyDate : {group.ModifyAt?.ToString("dd.MM.yyyy")}");

        }
      public void GetStudentsOfGroup()
        {
            if (GetAnyGroup()) return;

           GroupNameSect: Messages.InputMessage("group name");
            GetAllGroup();
            string name= Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Messages.InvalidInputMessage("Group name");
                goto GroupNameSect;
            }
            var group=_unitOfWork.Groups.GetByNameWithStudents(name);
            if (group == null)
            {
                Messages.NotFoundMessage("group");
                return;
            }
            if (group.Students.ToList().Count==0)
            {
                Messages.NotFoundMessage("any student on group");
                return;
            }
            Console.WriteLine("group_s student");
            foreach (var student in group.Students)
            {
                Console.WriteLine($"Name : {student.Name} Surname : {student.Surname}");
            }
        }
        public void AddGroup()
        {
        GroupNameSection: Messages.InputMessage("group name");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Messages.InvalidInputMessage("name");
                goto GroupNameSection;
            }
            var existGroup = _unitOfWork.Groups.GetByName(name);
            if (existGroup is not null)
            {
                Messages.AlreadyExistMessage(name);
                return;
            }

        GroupLimitSection: Messages.InputMessage("group limit");
            string limitInput = Console.ReadLine();
            int limit;
            bool isSucceded = int.TryParse(limitInput, out limit);
            if (!isSucceded)
            {
                Messages.InvalidInputMessage("limit");
                goto GroupLimitSection;
            }
            if (limit < 4)
            {
                Messages.GreaterValueMessage("Limit", "4");
                goto GroupLimitSection;
            }

            var group = new Group
            {
                Name = name,
                Limit = limit,
                //CreateAt = DateTime.Now
            };
            _unitOfWork.Groups.Add(group);
            _unitOfWork.Commit();
            Messages.SucceedMessage("group","added");
        }

        public void UpdateGroup()
        {
            if (GetAnyGroup()) return;
        GroupNameInput: Messages.InputMessage(" group name");
            GetAllGroup();
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Messages.InvalidInputMessage("name");
                goto GroupNameInput;
            }
            var group = _unitOfWork.Groups.GetByName(name);
            if (group is null)
            {
                Messages.NotFoundMessage("group");
                return;
            }
        ChangeNameInput: Messages.WantToChangeMessage("name ? y or n");
            string choiceInput = Console.ReadLine();
            char choice;
            bool isSucceded = char.TryParse(choiceInput, out choice);
            if (!isSucceded || !choice.isValidChoice())
            {
                Messages.InvalidInputMessage("choice");
                goto ChangeNameInput;
            }
            string newName = string.Empty;
            int newLimit = default;

            if (choice == 'y')
            {
            GroupNewNameInput: Messages.InputMessage("new name");
                newName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(newName))
                {
                    Messages.InvalidInputMessage("new name");
                    goto GroupNewNameInput;
                }
                var existGroup = _unitOfWork.Groups.GetByName(newName);
                if (existGroup is not null)
                {
                    Messages.AlreadyExistMessage(newName);
                    goto ChangeNameInput;
                }
            }
        ChangeLimitInput: Messages.WantToChangeMessage("limit ? y or n");
            choiceInput = Console.ReadLine();
            isSucceded = char.TryParse(choiceInput, out choice);
            if (!isSucceded || !choice.isValidChoice())
            {
                Messages.InvalidInputMessage("choice");
                goto ChangeLimitInput;
            }

            if (choice == 'y')
            {
            GroupLimitInput: Messages.InputMessage("new limit");
                string newLimitInput = Console.ReadLine();
                isSucceded = int.TryParse(newLimitInput, out newLimit);
                if (!isSucceded)
                {
                    Messages.InvalidInputMessage(" new Limit");
                    goto GroupLimitInput;
                }
                if (newLimit < 4)
                {
                    Messages.GreaterValueMessage("limit", "4");
                    goto ChangeLimitInput;
                }
            }

            if (!string.IsNullOrWhiteSpace(newName)) group.Name = newName;
            if (newLimit != default) group.Limit = newLimit;

            //group.ModifyAt=DateTime.Now;
            _unitOfWork.Groups.Update(group);
            _unitOfWork.Commit();
            Messages.SucceedMessage("group", "updated");
        }
  
        public void DeleteGroup()
        {
            if (GetAnyGroup()) return;
        DeleteGroupSect: Messages.InputMessage(" group name");
            GetAllGroup();
            string name=Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Messages.InvalidInputMessage("name");
                goto DeleteGroupSect;
            }
            var group=_unitOfWork.Groups.GetByNameWithStudents(name);
            if (group is null)
            {
                 Messages.NotFoundMessage(name);
                return;
            }
            group.IsDeleted = true;
            foreach (var student in group.Students)
            {
                student.IsDeleted = true;
            }
            _unitOfWork.Groups.Delete(group);
            _unitOfWork.Commit();
            Messages.SucceedMessage("group","deleted");
        }
    
    
        private bool GetAnyGroup()
        {
            if (_unitOfWork.Groups.GetAll().ToList().Count == 0)
            {
                Messages.NotFoundMessage("Any group");
                return true;
            }
            return false;
        }
    }
}
