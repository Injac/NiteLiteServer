using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteLiteServer.Commands
{
    public static class EnumTools
    {
        public static string ReturnStringRepresentation(Enum myEnum, Type enumType)
        {
            return Enum.GetName(enumType, myEnum);
        }

        public static List<string> GetAllNames(Type enumType)
        {
            return Enum.GetNames(enumType).ToList();
        }

        public static bool ContainsName(Type enumType, string name)
        {
            return Enum.GetNames(enumType).ToList().Contains(name);
        }
    }
}
