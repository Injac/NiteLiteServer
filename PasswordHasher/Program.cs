using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PasswordHasher
{
    class Program
    {
        static int Main(string[] args)
        {

            Console.WriteLine("Please enter your password and press [ENTER]:");
            var pass1 = Console.ReadLine();

            Console.WriteLine("Please confirm your password and press [ENTER]");
            var pass2 = Console.ReadLine();

            if (!pass1.Equals(pass2))
            {
                Console.WriteLine("Passwords do not match!");
                return 1;
            }

            string salt = BCrypt.Net.BCrypt.GenerateSalt(4);
            string passHash = BCrypt.Net.BCrypt.HashPassword(pass1, salt);

            Console.WriteLine("Here is your password-hash: \n {0} \n",passHash);

            Console.WriteLine("Veryfing password....");
            Console.WriteLine(
                BCrypt.Net.BCrypt.Verify(pass1, passHash) ? "OK" : "NOK");


            Console.WriteLine("Press a key to exit.");

            Console.ReadKey();

            return 0;


        }
    }
}
