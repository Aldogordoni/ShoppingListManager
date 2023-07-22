﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingListApi
{
    public class ShoppingItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsImportant { get; set; }
        public int Amount { get; set; }
    }
}