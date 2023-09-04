using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.DataBases
{
    public interface IDataContext : IDisposable
    {
        IRepository Repository { get; }
    }

}
