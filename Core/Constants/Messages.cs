using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Constants
{
    public static class Messages
    {
        public static void InputMessage(string title) =>Console.WriteLine( $"Input {title}");
        public static void InvalidInputMessage(string title) =>Console.WriteLine( $" {title} is invalid!!");
        public static void AlreadyExistMessage(string title) =>Console.WriteLine($" {title} Already exist");
        public static void NotFoundMessage(string title) =>Console.WriteLine($" {title} not found");
        public static void FulledMessage(string title) =>Console.WriteLine($" {title}  is fulled");
        public static void SucceedMessage(string title,string operation) =>Console.WriteLine($" {title} successfully {operation}");
        public static void GreaterValueMessage(string title,string value) =>Console.WriteLine($" {title} cannot smaller  {value}");
        public static void WantToChangeMessage(string title) =>Console.WriteLine($" want to {title} change ?");

    }
}
