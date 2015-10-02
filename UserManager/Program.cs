using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using NiteLiteServer.Model;
using STSdb4;
using STSdb4.Database;
using UserManager.Options;

namespace UserManager
{
    internal class Program
    {
        private static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        private static void Main(string[] args)
        {
            string action = default(string);
            string username = default(string);
            string password = default(string);
            string passwordConfirm = default(string);
            string newPassword = default(string);

            var options = new CommandLineOptions();

            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                action = options.Action;
                username = options.UserName;
                password = options.Password;
                passwordConfirm = options.PasswordConfirmation;
                newPassword = options.NewPassword;

                switch (action)
                {
                    case "adduser":
                        AddUser(username, password,passwordConfirm);
                        break;
                    case "deleteuser":
                        DeleteUser(username);
                        break;
                    case "changepassword":
                        ChangePassword(username, password, newPassword, passwordConfirm);
                        break;
                }
            }


            string userDb = default(string);
        }

        private static string GetUserDBSetting()
        {
            string userDb = default(string);

            if (System.Environment.OSVersion.ToString().Contains("Unix"))
            {
                userDb = $@"{AssemblyDirectory}/{"Data"}/{"users.stsdb4"}";
            }
            else
            {
                userDb = $@"{AssemblyDirectory}\{"Data"}\{"users.stsdb4"}";
            }
            return userDb;
        }

        private static void ChangePassword(string username, string password, string newPassword, string passwordConfirm)
        {
            var userDb = GetUserDBSetting();

            try
            {
                using (IStorageEngine engine = STSdb.FromFile(userDb))
                {
                    var userTable = engine.OpenXTable<string, User>("users");


                    var md5UserName = CreateMD5(username);


                    if (newPassword.Equals(passwordConfirm))
                    {
                        var user = userTable[md5UserName];

                        if (user != null)
                        {
                            if (BCrypt.Net.BCrypt.Verify(password, user.Password))
                            {
                                user.Password =  BCrypt.Net.BCrypt.HashPassword(newPassword, 4);
                                
                                userTable[md5UserName] = user;

                                engine.Commit();

                                Console.WriteLine($"Password for user '{username}' was succesfully changed.");
                            }
                            else
                            {
                                Console.WriteLine("Current password is not valid.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("User could not be found.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("New password and new password-confirmation do not match.");
                    }

                  
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"The user '{username}' could not be found.");
            }
        }

        private static void DeleteUser(string username)
        {

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(username))
            {
                Console.WriteLine("Username is mandatory.");
                return;
            }

            var userDb = GetUserDBSetting();
            
            using (IStorageEngine engine = STSdb.FromFile(userDb))
            {
                try
                {
                    var userTable = engine.OpenXTable<string, User>("users");

                    var md5UserName = CreateMD5(username);
                    
                    userTable.Delete(md5UserName);

                    engine.Commit();
                }
                catch (Exception ex)
                {

                    Console.WriteLine("User could not be found and deleted.");
                }

                Console.WriteLine($"User {username} successfully deleted.");
            }
        }

        private static void AddUser(string username, string password,string passwordConfirmation)
        {
            var userDb = GetUserDBSetting();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(username))
            {
                Console.WriteLine("Username value is mandatory.");
                return;
            }


            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrEmpty(password))
            {
                Console.WriteLine("Password value is mandatory.");
                return;
            }

            if (string.IsNullOrWhiteSpace(passwordConfirmation) || string.IsNullOrEmpty(passwordConfirmation))
            {
                Console.WriteLine("Password-confirmation value is mandatory.");
                return;
            }

            if (!password.Equals(passwordConfirmation))
            {
                Console.WriteLine("Password and password confirmation do not match.");
                return;
            }


            try
            {
                using (IStorageEngine engine = STSdb.FromFile(userDb))
                {
                    var userTable = engine.OpenXTable<string, User>("users");

                    var md5UserName = CreateMD5(username);

                    var passwordHash = BCrypt.Net.BCrypt.HashPassword(password, 4);

                    var user = new User()
                    {
                        Created = DateTime.UtcNow,
                        Id = Guid.NewGuid().ToString(),
                        Password = passwordHash,
                        UserName = md5UserName
                    };

                    userTable.InsertOrIgnore(md5UserName, user);

                    engine.Commit();

                    Console.WriteLine($"User '{username}' was successfully added.");
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("User could not be added. Please check your input data, or you may have deleted the database file.");
            }
        }

        //Microsoft Sample. Works.
        private static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}