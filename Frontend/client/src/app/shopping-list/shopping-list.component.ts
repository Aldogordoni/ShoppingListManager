import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

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
  baseUrl = 'https://localhost:5001/api/shopping';

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getItems();
    this.getBoughtItems();
  }

  getItems() {
    this.http.get<any[]>(this.baseUrl).subscribe(result => {
      this.items = result;
    }, error => console.error(error));
  }

  addItem() {
    this.http.post(this.baseUrl, { name: this.newItem }).subscribe(result => {
      this.getItems();
      this.newItem = '';
    }, error => console.error(error));
  }

  deleteItem(id: number) {
    this.http.delete(`${this.baseUrl}/${id}`).subscribe(result => {
      this.getItems();
    }, error => console.error(error));
  }

  // New method to mark an item as important
  markAsImportant(id: number) {
    this.http.post(`${this.baseUrl}/important/${id}`, {}).subscribe(result => {
      this.getItems();
    }, error => console.error(error));
  }

  // New method to move an item from "To Buy" to "Previously Bought"
  moveToBought(id: number) {
    this.http.post(`${this.baseUrl}/move/${id}`, {}).subscribe(result => {
      this.getItems();
      this.getBoughtItems();
    }, error => console.error(error));
  }

  // New method to get items from the "Previously Bought" list
  getBoughtItems() {
    this.http.get<any[]>(`${this.baseUrl}/bought`).subscribe(result => {
      this.boughtItems = result;
    }, error => console.error(error));
  }

  addToBuy(item: any) {
    let existingItem = this.items.find(i => i.name === item.name);
    if (existingItem) {
      existingItem.amount += 1;
    } else {
      this.items.push({ name: item.name, amount: 1 });
    }
  }

  // New method to add an item to the "To Buy" list with a quantity
  addItemWithQuantity() {
    this.http.post(this.baseUrl, { name: this.newItem, amount: this.quantity }).subscribe(result => {
      this.getItems();
      this.newItem = '';
      this.quantity = 1;
    }, error => console.error(error));
  }
}
