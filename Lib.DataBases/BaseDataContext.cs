using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.DataBases;

public abstract class BaseDataContext<T> : DbContext where T : DbContext
{
    public BaseDataContext(DbContextOptions<T> options) : base(options)
    {
    }

    protected BaseDataContext()
    {
    }
}