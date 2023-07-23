import { Component, OnInit } from '@angular/core';
import { ShoppingItem } from '../shared/models/shopping-item.model';
import { ShoppingListService } from '../shared/services/shopping-list.service';

@Component({
  selector: 'app-shopping-list',
  templateUrl: './shopping-list.component.html',
  styleUrls: ['./shopping-list.component.css']
})
export class ShoppingListComponent implements OnInit {
  items : any[] = [];
  boughtItems : any[] = [];
  quantity : number = 1;
  newItem = '';

  constructor(private shoppingListService: ShoppingListService) { }

  ngOnInit() {
    this.getItems();
    this.getBoughtItems();
  }

  getItems() {
    this.shoppingListService.getItems().subscribe(
      items => {
        this.items = items;
      },
      error => console.error(error)
    );
  }

  moveUp(name: string): void {
      this.shoppingListService.moveUp(name).subscribe(result => {
        this.getItems();
      }, error => console.error(error));
  }
  
  moveDown(order: number): void {
    let currentIndex = this.items.findIndex(item => item.order === order);
    if (currentIndex !== -1 && currentIndex < this.items.length - 1 && this.items[currentIndex]) {
      this.shoppingListService.moveDown(this.items[currentIndex].name).subscribe(result => {
        this.getItems();
      }, error => console.error(error));
    } else {
      console.error('Unable to move item down: item is undefined or last in the list');
    }
  }
  
  addItem() {
    if (this.newItem !== '' && this.newItem !== null) {
      this.shoppingListService.addNewItem(this.newItem).subscribe(
        response => {
          this.getItems();
          this.newItem = '';
          this.quantity = 1;
        },
        error => console.error(error)
      );
    }
  }

  deleteItem(Name: String) {
    this.shoppingListService.deleteItem(Name).subscribe(result => {
      this.getItems();
    }, error => console.error(error));
  }

  markAsImportant(Name: String) {
    this.shoppingListService.markAsImportant(Name).subscribe(result => {
      this.getItems();
    }, error => console.error(error));
  }

  moveToBought(Name: String) {
    this.shoppingListService.moveToBought(Name).subscribe(result => {
      this.getItems();
      this.getBoughtItems();
    }, error => console.error(error));
  }

  getBoughtItems() {
    this.shoppingListService.getBoughtItems().subscribe(result => {
      this.boughtItems = result;
    }, error => console.error(error));
  }

  // ShoppingListComponent
addToBuy(item: any) {
  let existingItem = this.items.find(i => i.name === item.name);
  if (existingItem) {
    existingItem.amount += 1;
    this.shoppingListService.updateItemAmount(existingItem).subscribe(() => {
      // Reload items after updating amount
      this.getItems();
    }, error => {
      console.error(error);
    });
  } else {
    this.shoppingListService.addToShoppingList(item).subscribe(result => {
      this.items.push({ name: item.name, amount: 1 });
      this.getItems();
    }, error => {
      console.error(error);
    });
  }
}


  addItemWithQuantity() {
    if (this.quantity <1){
      this.quantity = 1;
      console.error("item quantity cannot be less than 1");
    }
    if (this.newItem !== '' && this.newItem !== null) {
      this.shoppingListService.addItemWithQuantity(this.newItem, this.quantity).subscribe(result => {
        this.getItems();
        this.newItem = '';
        this.quantity = 1;
      }, error => console.error(error));
    }
  }
}
