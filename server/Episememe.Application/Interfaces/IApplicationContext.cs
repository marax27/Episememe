﻿using System.Threading.Tasks;
using Episememe.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Episememe.Application.Interfaces
{
    public interface IApplicationContext
    {
        DbSet<MediaInstance> MediaInstances { get; set; }
        DbSet<Tag> Tags { get; set; }

        void Update();
        Task UpdateAsync();
    }
}