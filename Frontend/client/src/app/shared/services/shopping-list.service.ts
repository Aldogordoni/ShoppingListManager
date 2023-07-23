// Import the necessary libraries.
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ShoppingItem } from '../models/shopping-item.model'; // Importing the shopping item model.

// The Injectable decorator marks it as a service that can be injected into other components.
@Injectable({
  providedIn: 'root',
})

// Define the service.
export class ShoppingListService {

  // URL to the shopping API endpoint.
  private baseUrl = 'https://localhost:5001/api/shopping';

  // Injecting HttpClient into the service via the constructor.
  constructor(private http: HttpClient) {}

  // Method to move a shopping item up.
  moveUp(name: string): Observable<any> {
    return this.http.post(`${this.baseUrl}/${name}/up`, {});
  }

  // Method to move a shopping item down.
  moveDown(name: string): Observable<any> {
    return this.http.post(`${this.baseUrl}/${name}/down`, {});
  }

  // Method to update an item.
  updateItem(name: string, item: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/${name}`, item);
  }

  // Method to get all items.
  getItems(): Observable<ShoppingItem[]> {
    return this.http.get<ShoppingItem[]>(this.baseUrl);
  }

  // Method to add a new item.
  addItem(name: string): Observable<any> {
    return this.http.post(this.baseUrl, { name });
  }

  // Method to add a new item (alternative method).
  addNewItem(name: any): Observable<any> {
    return this.http.post(this.baseUrl, { name: name });
  }

  // Method to delete an item.
  deleteItem(name: String): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${name}`);
  }

  // Method to mark an item as important.
  markAsImportant(name: String): Observable<any> {
    return this.http.post(`${this.baseUrl}/important/${name}`, {});
  }

  // Method to move an item to the bought list.
  moveToBought(name: String): Observable<any> {
    return this.http.post(`${this.baseUrl}/move/${name}`, {});
  }

  // Method to get all bought items.
  getBoughtItems(): Observable<ShoppingItem[]> {
    return this.http.get<ShoppingItem[]>(`${this.baseUrl}/bought`);
  }

  // Method to update item quantity.
  updateItemAmount(item: ShoppingItem): Observable<any> {
    return this.http.put(`${this.baseUrl}/updateItemAmount`, item);
  }

  // Method to add item to the shopping list.
  addToShoppingList(item: ShoppingItem): Observable<any> {
    return this.http.post(`${this.baseUrl}/addToShoppingList`, item);
  }

  // Method to add an item with a specific quantity.
  addItemWithQuantity(name: string, quantity: number): Observable<any> {
    return this.http.post(this.baseUrl, { name, amount: quantity });
  }
}
