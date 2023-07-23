using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingListApi.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingListApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingController : Controller
    {
        private static List<ShoppingItem> ShoppingItems = new List<ShoppingItem>();

        private readonly DataContext _dataContext;

        public ShoppingController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        private static List<ShoppingItem> BoughtItems = new List<ShoppingItem>();

        [HttpGet]
        public IActionResult Get()
        {
            var orderedItems = ShoppingItems.OrderByDescending(i => i.IsImportant).ThenBy(i => i.Order).ToList();
            return Ok(orderedItems);
        }

        [HttpPost]
        public IActionResult Post(ShoppingItem item)
        {

            // Check if the item already exists
            var itemSearch = ShoppingItems.Find(i => i.Name == item.Name);
            if (itemSearch != null)
            {
                return BadRequest("Item already exists");
            }
            item.Order = -1;
            bool isAdded = false;
            // Add the new item to the list
            ShoppingItems.Add(item);

            foreach(ShoppingItem i in ShoppingItems)
            {
                if(item.Name.CompareTo(i.Name) < 0 && !isAdded)
                {
                    item.Order = i.Order-1;
                    i.Order += 1;
                    isAdded = true;
                } else if (isAdded)
                {
                    i.Order += 1;
                }

            }
            if (!isAdded)
            {
                item.Order = ShoppingItems.Max( i => i.Order)+1;
            }

            ShoppingItems = ShoppingItems.OrderBy(i => i.Order).ToList();

            return Ok();
        }

        [HttpPost("{name}/up")]
        public IActionResult MoveUp(String name)
        {
            var item = ShoppingItems.Find(i => i.Name == name);
            if (item == null || item.Order == 1)
            {
                return BadRequest("Item not found or already at the top");
            }

            var itemAbove = ShoppingItems.Find(i => i.Order == item.Order - 1);
            itemAbove.Order += 1;
            item.Order -= 1;
            ShoppingItems = ShoppingItems.OrderBy(i => i.Order).ToList();

            return Ok();
        }

        [HttpPost("{name}/down")]
        public IActionResult MoveDown(String name)
        {
            var item = ShoppingItems.Find(i => i.Name == name);
            if (item == null || item.Order == ShoppingItems.Count)
            {
                return BadRequest("Item not found or already at the bottom");
            }

            var itemBelow = ShoppingItems.Find(i => i.Order == item.Order + 1);
            itemBelow.Order -= 1;
            item.Order += 1;
            ShoppingItems = ShoppingItems.OrderBy(i => i.Order).ToList();

            return Ok();
        }



        [HttpDelete("{name}")]
        public IActionResult Delete(String name)
        {
            var itemSearch = ShoppingItems.Find(i => i.Name == name);
            if (itemSearch == null)
                return NotFound();

            ShoppingItems.Remove(itemSearch);
            return Ok();
        }

        [HttpGet("bought")]
        public IActionResult GetBoughtItems()
        {
            return Ok(BoughtItems);
        }

        // New endpoint to add an item to the "Previously Bought" list
        [HttpPost("bought")]
        public IActionResult AddBoughtItem(ShoppingItem item)
        {
            BoughtItems.Add(item);
            return Ok();
        }

        // New endpoint to move an item from "To Buy" to "Previously Bought"
        [HttpPost("move/{name}")]
        public IActionResult MoveToBought(String name)
        {
            var itemToMove = ShoppingItems.FirstOrDefault(i => i.Name == name);
            if (itemToMove == null)
                return NotFound();

            ShoppingItems.Remove(itemToMove);

            var boughtItem = new ShoppingItem { Name = itemToMove.Name };

            var itemSearch = BoughtItems.Find(i => i.Name == name);
            if (itemSearch == null)
            {
                BoughtItems.Add(boughtItem);
            }
            return Ok();
        }

        // New endpoint to add an item from "Previously Bought" back to "To Buy"
        [HttpPost("addToShoppingList")]
        public IActionResult AddToShoppingList([FromBody] ShoppingItem item)
        {
            var existingItem = ShoppingItems.FirstOrDefault(i => i.Name == item.Name);
            if (existingItem != null)
            {
                // If the item already exists in the shopping list, increase its quantity
                existingItem.Amount += item.Amount;
            }
            else
            {
                // If the item does not exist in the shopping list, add it
                ShoppingItems.Add(item);
                item.Amount = 1;

                // Re-order the list
                ShoppingItems = ShoppingItems
                        .OrderBy(i => i.Name)
                        .Select((item, index) =>
                        {
                            item.Order = index + 1;
                            return item;
                        })
                        .ToList();
            }

            return Ok();
        }

        [HttpPut("updateItemAmount")]
        public IActionResult UpdateItemAmount([FromBody] ShoppingItem updatedItem)
        {
            // Find the item to be updated
            var itemToUpdate = ShoppingItems.Find(i => i.Name == updatedItem.Name);
            if (itemToUpdate == null)
                return NotFound("Item not found");

            // Update the item's amount
            itemToUpdate.Amount = updatedItem.Amount;

            return Ok();
        }


        // New endpoint to mark an item as important
        [HttpPost("important/{name}")]
        public IActionResult MarkAsImportant(String name)
        {
            var item = ShoppingItems.Find(i => i.Name == name);
            if (item == null)
                return NotFound();


            item.IsImportant = !item.IsImportant;

            return Ok();
        }

        [HttpPut("{name}/order")]
        public IActionResult UpdateOrder(String name, [FromBody] Dictionary<string, int> data)
        {
            var order = data["order"];
            var item = ShoppingItems.Find(i => i.Name == name);
            if (item == null)
                return NotFound();

            item.Order = order;
            return Ok();
        }
    }
}
