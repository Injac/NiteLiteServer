using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiteLite.Plugins.Base
{
   

    public abstract class NiteLitePluginBase<T> : IDisposable
    {
        public virtual string PluginName { get; set; }
        public virtual string PluginDescription { get; set; }
        public virtual string PluginAuthor { get; set; }

        public virtual Task<T> Execute<TU>(TU parameter)
        {
            return default(Task<T>);
        }
        public virtual Task<T> Execute()
        {
            return default(Task<T>);
        }
        public virtual Task<T> Execute<T1, T2>(T1 param1, T2 param2)
        {
            return default(Task<T>);
        }
        public virtual Task<T> Execute<T1, T2, T3>(T1 param1, T2 param2, T3 param3)
        {
            return default(Task<T>);
        }

        public virtual void Dispose()
        {
            throw new NotImplementedException();
        }
    }

   
   
}
