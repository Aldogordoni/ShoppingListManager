import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ShoppingItem } from '../models/shopping-item.model';

@Injectable({
  providedIn: 'root',
})
export class ShoppingListService {

  private baseUrl = 'https://localhost:5001/api/shopping';

  constructor(private http: HttpClient) {}

  moveUp(name: string): Observable<any> {
    // replace with your actual API endpoint for moving item up
    return this.http.post(`${this.baseUrl}/${name}/up`, {});
}

moveDown(name: string): Observable<any> {
    // replace with your actual API endpoint for moving item down
    return this.http.post(`${this.baseUrl}/${name}/down`, {});
}


  updateItem(name: string, item: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/${name}`, item);
  }

  getItems(): Observable<ShoppingItem[]> {
    return this.http.get<ShoppingItem[]>(this.baseUrl);
  }

  addItem(name: string): Observable<any> {
    return this.http.post(this.baseUrl, { name });
  }

  addNewItem(name: any): Observable<any> {
    return this.http.post(this.baseUrl, { name: name });
  }

  deleteItem(name: String): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${name}`);
  }

  markAsImportant(name: String): Observable<any> {
    return this.http.post(`${this.baseUrl}/important/${name}`, {});
  }

  moveToBought(name: String): Observable<any> {
    return this.http.post(`${this.baseUrl}/move/${name}`, {});
  }

  getBoughtItems(): Observable<ShoppingItem[]> {
    return this.http.get<ShoppingItem[]>(`${this.baseUrl}/bought`);
  }

  // ShoppingListService
updateItemAmount(item: ShoppingItem): Observable<any> {
  return this.http.put(`${this.baseUrl}/updateItemAmount`, item);
}

    // ShoppingListService
  addToShoppingList(item: ShoppingItem): Observable<any> {
    return this.http.post(`${this.baseUrl}/addToShoppingList`, item);
  }


  addItemWithQuantity(name: string, quantity: number): Observable<any> {
    return this.http.post(this.baseUrl, { name, amount: quantity });
  }
}
