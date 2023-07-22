using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingListApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingController : Controller
    {
        private static List<ShoppingItem> ShoppingItems = new List<ShoppingItem>()
    {
        new ShoppingItem { Id = 1, Name = "Milk" , IsImportant = true, Amount = 2},
        new ShoppingItem { Id = 2, Name = "Bread", IsImportant = false, Amount = 1 }
    };

        private static List<BoughtItem> BoughtItems = new List<BoughtItem>();

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(ShoppingItems);
        }

        [HttpPost]
        public IActionResult Post(ShoppingItem item)
        {
            ShoppingItems.Add(item);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = ShoppingItems.Find(i => i.Id == id);
            if (item == null)
                return NotFound();

            ShoppingItems.Remove(item);
            return Ok();
        }

        [HttpGet("bought")]
        public IActionResult GetBoughtItems()
        {
            return Ok(BoughtItems);
        }

        // New endpoint to add an item to the "Previously Bought" list
        [HttpPost("bought")]
        public IActionResult AddBoughtItem(BoughtItem item)
        {
            BoughtItems.Add(item);
            return Ok();
        }

        // New endpoint to move an item from "To Buy" to "Previously Bought"
        [HttpPost("move/{id}")]
        public IActionResult MoveToBought(int id)
        {
            var itemToMove = ShoppingItems.Find(i => i.Id == id);
            if (itemToMove == null)
                return NotFound();

            ShoppingItems.Remove(itemToMove);
            BoughtItems.Add(new BoughtItem { Id = itemToMove.Id, Name = itemToMove.Name });
            return Ok();
        }

        // New endpoint to mark an item as important
        [HttpPost("important/{id}")]
        public IActionResult MarkAsImportant(int id)
        {
            var item = ShoppingItems.Find(i => i.Id == id);
            if (item == null)
                return NotFound();

            item.IsImportant = true;
            return Ok();
        }

        // New endpoint to sort items
        [HttpGet("sorted")]
        public IActionResult GetSortedItems()
        {
            return Ok(ShoppingItems.OrderBy(i => i.Name).ToList());
        }

    }
}
