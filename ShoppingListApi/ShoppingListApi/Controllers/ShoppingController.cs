// Import required namespaces
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingListApi.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

// Define the namespace for the controller
namespace ShoppingListApi.Controllers
{
    // Set route and controller attributes
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingController : Controller
    {
        // Initialize a list of Shopping Items
        private static List<ShoppingItem> ShoppingItems = new List<ShoppingItem>();

        // Instance of DataContext for database operations
        private readonly DataContext _dataContext;

        // Constructor with DataContext injected - Can be used if needed
        public ShoppingController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // Initialize a list of Bought Items
        private static List<ShoppingItem> BoughtItems = new List<ShoppingItem>();

        // HTTP GET method to retrieve shopping items
        [HttpGet]
        public IActionResult Get()
        {
            // Order the items based on their importance and order
            var orderedItems = ShoppingItems.OrderByDescending(i => i.IsImportant).ThenBy(i => i.Order).ToList();
            return Ok(orderedItems);
        }

        // HTTP POST method to add a new item
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

            // Update the order of items in the list
            foreach (ShoppingItem i in ShoppingItems)
            {
                if (item.Name.CompareTo(i.Name) < 0 && !isAdded)
                {
                    item.Order = i.Order - 1;
                    i.Order += 1;
                    isAdded = true;
                }
                else if (isAdded)
                {
                    i.Order += 1;
                }

            }
            if (!isAdded)
            {
                item.Order = ShoppingItems.Max(i => i.Order) + 1;
            }

            // Order the items in the list
            ShoppingItems = ShoppingItems.OrderBy(i => i.Order).ToList();

            return Ok();
        }

        // HTTP POST method to move an item up
        [HttpPost("{name}/up")]
        public IActionResult MoveUp(String name)
        {
            // Find the item in the list
            var item = ShoppingItems.Find(i => i.Name == name);
            if (item == null || item.Order == 1)
            {
                return BadRequest("Item not found or already at the top");
            }

            // Change the order of the item and the item above it
            var itemAbove = ShoppingItems.Find(i => i.Order == item.Order - 1);
            itemAbove.Order += 1;
            item.Order -= 1;
            // Order the items in the list
            ShoppingItems = ShoppingItems.OrderBy(i => i.Order).ToList();

            return Ok();
        }

        // HTTP POST method to move an item down
        [HttpPost("{name}/down")]
        public IActionResult MoveDown(String name)
        {
            // Find the item in the list
            var item = ShoppingItems.Find(i => i.Name == name);
            if (item == null || item.Order == ShoppingItems.Count)
            {
                return BadRequest("Item not found or already at the bottom");
            }

            // Change the order of the item and the item below it
            var itemBelow = ShoppingItems.Find(i => i.Order == item.Order + 1);
            itemBelow.Order -= 1;
            item.Order += 1;
            // Order the items in the list
            ShoppingItems = ShoppingItems.OrderBy(i => i.Order).ToList();

            return Ok();
        }

        // HTTP DELETE method to delete an item
        [HttpDelete("{name}")]
        public IActionResult Delete(String name)
        {
            // Find the item in the list
            var itemSearch = ShoppingItems.Find(i => i.Name == name);
            if (itemSearch == null)
                return NotFound();

            // Remove the item from the list
            ShoppingItems.Remove(itemSearch);
            return Ok();
        }

        // HTTP GET method to retrieve bought items
        [HttpGet("bought")]
        public IActionResult GetBoughtItems()
        {
            return Ok(BoughtItems);
        }

        // HTTP POST method to add an item to the "Previously Bought" list
        [HttpPost("bought")]
        public IActionResult AddBoughtItem(ShoppingItem item)
        {
            BoughtItems.Add(item);
            return Ok();
        }

        // HTTP POST method to move an item from "To Buy" to "Previously Bought"
        [HttpPost("move/{name}")]
        public IActionResult MoveToBought(String name)
        {
            var itemToMove = ShoppingItems.FirstOrDefault(i => i.Name == name);
            if (itemToMove == null)
                return NotFound();

            // Remove the item from the ShoppingItems list
            ShoppingItems.Remove(itemToMove);

            // Add the item to the BoughtItems list
            var boughtItem = new ShoppingItem { Name = itemToMove.Name };

            var itemSearch = BoughtItems.Find(i => i.Name == name);
            if (itemSearch == null)
            {
                BoughtItems.Add(boughtItem);
            }
            return Ok();
        }

        // HTTP POST method to add an item from "Previously Bought" back to "To Buy"
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

        // HTTP PUT method to update the amount of an item
        [HttpPut("updateItemAmount")]
        public IActionResult UpdateItemAmount([FromBody] ShoppingItem updatedItem)
        {
            // Find the item in the list
            var itemToUpdate = ShoppingItems.Find(i => i.Name == updatedItem.Name);
            if (itemToUpdate == null)
                return NotFound("Item not found");

            // Update the item's amount
            itemToUpdate.Amount = updatedItem.Amount;

            return Ok();
        }

        // HTTP POST method to mark an item as important
        [HttpPost("important/{name}")]
        public IActionResult MarkAsImportant(String name)
        {
            // Find the item in the list
            var item = ShoppingItems.Find(i => i.Name == name);
            if (item == null)
                return NotFound();

            // Toggle the important status of the item
            item.IsImportant = !item.IsImportant;

            return Ok();
        }

        // HTTP PUT method to update the order of an item
        [HttpPut("{name}/order")]
        public IActionResult UpdateOrder(String name, [FromBody] Dictionary<string, int> data)
        {
            // Extract the order from the request data
            var order = data["order"];

            // Find the item in the list
            var item = ShoppingItems.Find(i => i.Name == name);
            if (item == null)
                return NotFound();

            // Update the item's order
            item.Order = order;
            return Ok();
        }
    }
}
