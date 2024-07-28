using Application.Services.Concrete;
using Core.Constants;

namespace Presentation
{
    internal static class Program
    {
        private static readonly GroupService _groupService;
        private static readonly StudentService _studentService;
        static Program()
        {
            _groupService = new();
            _studentService = new();
        }
        static void Main(string[] args)
        {
            while (true)
            {
            ChoiceIput: Messages.InputMessage("choice");
                ShowMenu();
                string choiceInput = Console.ReadLine();
                int choice;
                bool isSuccedded = int.TryParse(choiceInput, out choice);
                if (!isSuccedded)
                {
                    Messages.InvalidInputMessage("choice");
                    goto ChoiceIput;
                }

                switch ((Operations)choice)
                {
                    case Operations.Exit:
                        return;
                    case Operations.AllGroup:
                        _groupService.GetAllGroup();
                        break;
                    case Operations.GetDetailsGroup:
                        _groupService.GetDetailsGroup();
                        break;
                    case Operations.GetStudentsOfGroup:
                        _groupService.GetStudentsOfGroup();
                        break;
                    case Operations.AddGroup:
                        _groupService.AddGroup();
                        break;
                    case Operations.UpdateGroup:
                        _groupService.UpdateGroup();
                        break;

                    case Operations.DeleteGroup:
                        _groupService.DeleteGroup();
                        break;
                    case Operations.AllStudent:
                        _studentService.GetAllstudents();
                        break;
                        case Operations.GetDetailsStudent:
                            _studentService.GetDetailsStudent();
                        break;
                    case Operations.AddStudent:
                        _studentService.AddStudent();
                        break;
                        case Operations.UpdateStudent:
                        _studentService.UpdateStudent();
                        break;
                        case Operations.DeleteStudent:
                            _studentService.DeletedStudent();
                        break;
                    default:
                        Messages.InvalidInputMessage("Choice");
                        goto ChoiceIput;

                }
            }

        }

        private static void ShowMenu()
        {
            Console.WriteLine(" --- MENU --- ");
            Console.WriteLine(" 0 - Exit");
            Console.WriteLine(" 1 - All Group");
            Console.WriteLine(" 2 - Get Details  Group");
            Console.WriteLine(" 3  - Get Students Of Group");
            Console.WriteLine(" 4  - Add Group");
            Console.WriteLine(" 5  - Update Group");
            Console.WriteLine(" 6  - Delete Group");
            Console.WriteLine(" 7 - All  Student");
            Console.WriteLine(" 8 - Get Details  Student");
            Console.WriteLine(" 9 - Add  Student");
            Console.WriteLine(" 10 - Update  Student");
            Console.WriteLine(" 11 - Delete  Student");
        }
    }
}
