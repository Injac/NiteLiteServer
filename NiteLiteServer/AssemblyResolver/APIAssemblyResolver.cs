using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Http.Dispatcher;

namespace NiteLiteServer.AssemblyResolver
{
    public class ApiAssemblyResolver: DefaultAssembliesResolver
    {
        public override ICollection<Assembly> GetAssemblies()
        {

            try
            {
                List<Assembly> assemblies = new List<Assembly>(base.GetAssemblies());

                string[] apiAssemblies;

                if (System.Environment.OSVersion.ToString().Contains("Unix"))
                {
                    
                    apiAssemblies = Directory.GetFiles(@"./API", "*NLite.API*.dll");
                }
                else
                {
                    apiAssemblies = Directory.GetFiles(@".\API", "*NLite.API*.dll");
                }
              

                assemblies.AddRange(apiAssemblies.Select(Assembly.LoadFrom));

                return assemblies;
            }
            catch (System.Exception ex)
            {

                Debug.WriteLine(ex.Message);
                Debug.WriteLine(System.Environment.OSVersion);

                return default(ICollection<Assembly>);

            }


        }
    }
}
