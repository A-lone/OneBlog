using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace OneBlog.Helpers
{
    public static class TypeExtension
    {
   
        // FindInterfaces
        // This method will filter the interfaces supported the class
        public static Type[] FindInterfaces(this Type type, TypeFilter filter, Object filterCriteria)
        {
            if (filter == null)
                throw new ArgumentNullException("filter");
            Contract.EndContractBlock();
            Type[] c = type.GetInterfaces();
            int cnt = 0;
            for (int i = 0; i < c.Length; i++)
            {
                if (!filter(c[i], filterCriteria))
                    c[i] = null;
                else
                    cnt++;
            }
            if (cnt == c.Length)
                return c;

            Type[] ret = new Type[cnt];
            cnt = 0;
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] != null)
                    ret[cnt++] = c[i];
            }
            return ret;
        }
    }
}
