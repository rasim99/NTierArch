using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extentions
{
    public static class CharExtentions
    {
        public static bool isValidChoice(this char symbol)
        {
            if (symbol.ToString().ToLower()=="y" || symbol.ToString().ToLower()=="n")
            {
                return true;
            }  
            return false;
        }
    }
}
