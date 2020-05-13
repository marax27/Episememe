<<<<<<< HEAD
﻿namespace Episememe.Application.Interfaces
{
    public interface IWritableApplicationContext : IApplicationContext, IWritableContext
    {
    }
}
=======
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Episememe.Application.Interfaces
{
    public interface IWritableApplicationContext : IApplicationContext, IWritableContext
    {
        DatabaseFacade Database { get; }
    }
}
>>>>>>> Misc
