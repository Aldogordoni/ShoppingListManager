// Import the necessary libraries and services.
import { Component, OnInit } from '@angular/core';
import { ShoppingListService } from '../shared/services/shopping-list.service'; // Importing the shopping list service.


@Component({
  selector: 'app-shopping-list',
  templateUrl: './shopping-list.component.html',
  styleUrls: ['./shopping-list.component.css']
})

// The ShoppingListComponent class that implements the OnInit lifecycle hook.
export class ShoppingListComponent implements OnInit {

  // Declare variables.
  items : any[] = []; // An array to hold the shopping items.
  boughtItems : any[] = []; // An array to hold the bought items.
  quantity : number = 1; // The quantity of the item to be added.
  newItem = ''; // The new item to be added.

  // Inject the ShoppingListService into the component via the constructor.
  constructor(private shoppingListService: ShoppingListService) { }

  // The ngOnInit method that is called when the component is initialized.
  ngOnInit() {
    this.getItems(); // Get the shopping items.
    this.getBoughtItems(); // Get the bought items.
  }

  // Method to get all items from the service.
  getItems() {
    this.shoppingListService.getItems().subscribe(
      items => {
        this.items = items; // Set the items.
      },
      error => console.error(error) // Log errors.
    );
  }

  // Method to move an item up in the list.
  moveUp(name: string): void {
      this.shoppingListService.moveUp(name).subscribe(result => {
        this.getItems(); // Update the list after moving the item.
      }, error => console.error(error)); // Log errors.
  }

  // Method to move an item down in the list.
  moveDown(order: number): void {
    let currentIndex = this.items.findIndex(item => item.order === order); // Find the current index of the item.
    if (currentIndex !== -1 && currentIndex < this.items.length - 1 && this.items[currentIndex]) {
      this.shoppingListService.moveDown(this.items[currentIndex].name).subscribe(result => {
        this.getItems(); // Update the list after moving the item.
      }, error => console.error(error)); // Log errors.
    } else {
      console.error('Unable to move item down: item is undefined or last in the list'); // Log errors.
    }
  }

  // Method to add a new item to the list.
  addItem() {
    if (this.newItem !== '' && this.newItem !== null) {
      this.shoppingListService.addNewItem(this.newItem).subscribe(
        response => {
          this.getItems(); // Update the list after adding the new item.
          this.newItem = ''; // Reset the new item.
          this.quantity = 1; // Reset the quantity.
        },
        error => console.error(error) // Log errors.
      );
    }
  }

  // Method to delete an item from the list.
  deleteItem(Name: String) {
    this.shoppingListService.deleteItem(Name).subscribe(result => {
      this.getItems(); // Update the list after deleting the item.
    }, error => console.error(error)); // Log errors.
  }

  // Method to mark an item as important.
  markAsImportant(Name: String) {
    this.shoppingListService.markAsImportant(Name).subscribe(result => {
      this.getItems(); // Update the list after marking the item.
    }, error => console.error(error)); // Log errors.
  }

  // Method to move an item to the bought list.
  moveToBought(Name: String) {
    this.shoppingListService.moveToBought(Name).subscribe(result => {
      this.getItems(); // Update the list after moving the item.
      this.getBoughtItems(); // Update the bought items list.
    }, error => console.error(error)); // Log errors.
  }

  // Method to get all bought items from the service.
  getBoughtItems() {
    this.shoppingListService.getBoughtItems().subscribe(result => {
      this.boughtItems = result; // Set the bought items.
    }, error => console.error(error)); // Log errors.
  }

  // Method to add an item to the shopping list.
  addToBuy(item: any) {
    let existingItem = this.items.find(i => i.name === item.name); // Check if the item already exists.
    if (existingItem) {
      existingItem.amount += 1; // Increase the amount if the item already exists.
      this.shoppingListService.updateItemAmount(existingItem).subscribe(() => {
        this.getItems(); // Update the items after increasing the amount.
      }, error => console.error(error)); // Log errors.
    } else {
      this.shoppingListService.addToShoppingList(item).subscribe(result => {
        this.items.push({ name: item.name, amount: 1 }); // Add the new item to the list.
        this.getItems(); // Update the items after adding the new item.
      }, error => console.error(error)); // Log errors.
    }
  }

  // Method to add an item with a specific quantity to the list.
  addItemWithQuantity() {
    if (this.quantity < 1) {
      this.quantity = 1; // Ensure that the quantity cannot be less than 1.
      console.error("item quantity cannot be less than 1"); // Log errors.
    }
    if (this.newItem !== '' && this.newItem !== null) {
      this.shoppingListService.addItemWithQuantity(this.newItem, this.quantity).subscribe(result => {
        this.getItems(); // Update the list after adding the new item.
        this.newItem = ''; // Reset the new item.
        this.quantity = 1; // Reset the quantity.
      }, error => console.error(error)); // Log errors.
    }
  }
}
