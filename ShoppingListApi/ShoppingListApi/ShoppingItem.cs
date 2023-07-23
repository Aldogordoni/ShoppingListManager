using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ShoppingListApi
{
    public class ShoppingItem
    {
        [Key]
        public string Name { get; set; }
        public bool IsImportant { get; set; }
        public int Amount { get; set; }
        public int Order { get; set; }
    }
}
