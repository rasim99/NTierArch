using Application.Services.Abstract;
using Core.Constants;
using Core.Entities;
using Core.Extentions;
using Data.Contexts;
using Data.Repositories.Concrete;
using Data.UnitOfWork.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Concrete
{
    public class StudentService : IStudentService
    {
        private readonly UnitOfWork _unitOfWork;
        public StudentService()
        {
            _unitOfWork = new();
        }
        public void GetAllstudents()
        {
            if (GetAnyStudent()) return;

            foreach (var student in _unitOfWork.Students.GetAll())
            {
                Console.WriteLine($"Id : {student.Id} Name : {student.Name} " +
                    $" Surname : {student.Surname}");
            }
        }
        public void GetDetailsStudent()
        {
            if (GetAnyStudent()) return;
            GetAllstudents();
          StudentIdInput:  Messages.InputMessage("student id");
            string idInput= Console.ReadLine();
            int id;
            bool isSucceeded= int.TryParse(idInput, out id);
            if (!isSucceeded)
            {
                Messages.InvalidInputMessage("student id");
                goto StudentIdInput;
            }
            var student=_unitOfWork.Students.GetByIdWithGroup(id);
            if (student is null)
            {
                Messages.NotFoundMessage("student");
                return;
            }
            Console.WriteLine($" Name : {student.Name} Surname : {student.Surname} Group name {student.Group.Name}");
        }
        public void AddStudent()
        {
           if( GetAnyGroupOfStudent()) return;

        GroupNameOfStudentSect: Messages.InputMessage(" group name for add student");
            foreach (var gr in _unitOfWork.Groups.GetAll())
            {
                Console.WriteLine($" Name : {gr.Name} Limit {gr.Limit}");
            }
            string groupName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(groupName))
            {
                Messages.InvalidInputMessage("group name");
                goto GroupNameOfStudentSect;
            }
            var group = _unitOfWork.Groups.GetByName(groupName);
            if (group == null)
            {
                Messages.NotFoundMessage(groupName);
                goto GroupNameOfStudentSect;
            }

            if (group.Students?.Count >= group.Limit)
            {
                Messages.FulledMessage(groupName);
                return;
            }

        StudentNameInput: Messages.InputMessage("student name");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Messages.InvalidInputMessage("student name");
                goto StudentNameInput;
            }
        StudentSurnameInput: Messages.InputMessage("student surname");
            string surname = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(surname))
            {
                Messages.InvalidInputMessage("student surname");
                goto StudentSurnameInput;
            }

            var student = new Student
            {
                Name = name,
                Surname = surname,
                CreateAt = DateTime.Now,
                GroupId = group.Id,
            };
            _unitOfWork.Students.Add(student);
            _unitOfWork.Commit();
            Messages.SucceedMessage("student", "added");
        }
        public void UpdateStudent()
        {
            if (GetAnyStudent()) return;
            UpdateStudentIdInput:  Messages.InputMessage("student id");
            
            GetAllstudents();
            string idInput = Console.ReadLine();
            int id;
            bool isSucceeded = int.TryParse(idInput, out id);
            if (!isSucceeded)
            {
                Messages.InvalidInputMessage("student id");
                goto UpdateStudentIdInput;
            }
            var student = _unitOfWork.Students.Get(id);
            if (student is null)
            {
                Messages.NotFoundMessage("student");
                return;
            }
            string newName=string.Empty;
            string newSurname=string.Empty;
            int newGroupId=default;

         ChangeNameInput: Messages.WantToChangeMessage("name ? y or n");
            string choiceInput=Console.ReadLine();
            char choice;
            isSucceeded=char.TryParse(choiceInput, out choice);
            if (!isSucceeded || !choice.isValidChoice())
            {
                Messages.InvalidInputMessage("choice");
                goto ChangeNameInput;
            }
            if (choice=='y')
            {
               NewNameInput: Messages.InputMessage("new Name");
                newName=Console.ReadLine();
                if (string.IsNullOrWhiteSpace(newName))
                {
                    Messages.InvalidInputMessage("new name");
                    goto NewNameInput;
                }

            } 
        ChangeSurnameInput: Messages.WantToChangeMessage("surname ? y or n");
             choiceInput=Console.ReadLine();
            isSucceeded=char.TryParse(choiceInput, out choice);
            if (!isSucceeded || !choice.isValidChoice())
            {
                Messages.InvalidInputMessage("choice");
                goto ChangeSurnameInput;
            }
            if (choice=='y')
            {
               NewSurnameInput: Messages.InputMessage("new surname");
                newSurname=Console.ReadLine();
                if (string.IsNullOrWhiteSpace(newSurname))
                {
                    Messages.InvalidInputMessage("new surname");
                    goto NewSurnameInput;
                }

            }

            if (_unitOfWork.Groups.GetAll().Count>1)
            {
            ChangeGroupInput: Messages.WantToChangeMessage("group ? y or n");
                choiceInput = Console.ReadLine();
                isSucceeded = char.TryParse(choiceInput, out choice);
                if (!isSucceeded || !choice.isValidChoice())
                {
                    Messages.InvalidInputMessage("choice");
                    goto ChangeGroupInput;
                }

                if (choice=='y')
                {

                  GroupNameInput:   Messages.InputMessage(" group id");
                    foreach (var group in _unitOfWork.Groups.GetAll())
                    {
                        Console.WriteLine($"Id : {group.Id} Name : {group.Name} Limit: {group.Limit}");
                    }
                    string newGroupIdInput= Console.ReadLine();
                    isSucceeded = int.TryParse(newGroupIdInput,out newGroupId);
                    if (!isSucceeded)
                    {
                        Messages.InvalidInputMessage("group id");
                        goto GroupNameInput;
                    }
                    var existGroup = _unitOfWork.Groups.Get(newGroupId);

                    if (existGroup is null)
                    {
                        Messages.NotFoundMessage("group");
                        goto GroupNameInput ;
                    }
                    if (student.GroupId == newGroupId)
                    {
                        Messages.AlreadyExistMessage("student on group");
                        goto ChangeGroupInput;
                    }
                    if (existGroup.Students?.Count>=existGroup.Limit )
                    {
                        Messages.FulledMessage("group");
                        goto ChangeGroupInput;
                    }

                }
            }

            if (newName != string.Empty) student.Name = newName;
            if (newSurname != string.Empty) student.Surname = newSurname;
            if (newGroupId != default) student.GroupId = newGroupId;
            _unitOfWork.Students.Update(student);
            _unitOfWork.Commit();
            Messages.SucceedMessage("student", "updated");
        }
        public void DeletedStudent()
        {
            if (GetAnyStudent()) return;

        DeletedStudentIdInput: Messages.InputMessage(" student id");
            GetAllstudents();
            string idInput = Console.ReadLine();
            int id;
            bool isSucceeded = int.TryParse(idInput, out id);
            if (!isSucceeded)
            {
                Messages.InvalidInputMessage("student id");
                goto DeletedStudentIdInput;
            }
            var student = _unitOfWork.Students.Get(id);
            if (student is null)
            {
                Messages.NotFoundMessage("student");
                return;
            }
            student.IsDeleted = true;
            _unitOfWork.Students.Delete(student);
            _unitOfWork.Commit();
            Messages.SucceedMessage("student","deleted");
        }
    
        private bool  GetAnyGroupOfStudent()
        {
            if (_unitOfWork.Groups.GetAll().Count == 0)
            {
                Messages.NotFoundMessage("Pleace create group firstly! Any group");
                return true;
            }
            return false;
        }
        private bool GetAnyStudent()
        {
            if (_unitOfWork.Students.GetAll().Count == 0)
            {
                Messages.NotFoundMessage("any student");
                return true;
            }
            return false;
        }
    }
}
