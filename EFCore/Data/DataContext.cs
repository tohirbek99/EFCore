﻿using EFCore.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Data
{
    public class DataContext :DbContext
    {
        public DataContext(DbContextOptions<DataContext>options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Category> Categories { get; set; }

    }
}
