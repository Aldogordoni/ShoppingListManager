﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingListApi.Database
{
    //Database connection to a localhost system
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<ShoppingItem> ShoppingItems => Set<ShoppingItem>();
    }
}
