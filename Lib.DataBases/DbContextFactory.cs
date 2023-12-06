using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.DataBases;

public class DbContextFactory<T> where T : IDataContext, new()
{
    public static T CreateContext()
    {
        return new T();
    }
}