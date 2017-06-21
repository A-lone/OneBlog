using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace One.AspNetCore.TimedJob
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class NonJobAttribute : Attribute
    {
    }
}
