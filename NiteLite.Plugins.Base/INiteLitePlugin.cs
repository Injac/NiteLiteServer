using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteLite.Plugins.Base
{
    public interface INiteLitePlugin<T>
    {
        string PluginName { get; set; }
        string PluginDescription { get; set; }
        string PluginAuthor { get; set; }

        Task<T> Execute(params object[] parameters);
        
    }
}
