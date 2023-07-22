import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ShoppingItem } from './shopping-item.model';

@Injectable({
  providedIn: 'root'
})
export class ShoppingListService {

  constructor(private http: HttpClient) { }

  getItems(): Observable<ShoppingItem[]> {
    return this.http.get<ShoppingItem[]>('/api/shopping');
  }

  getBoughtItems(): Observable<ShoppingItem[]> {
    return this.http.get<ShoppingItem[]>('/api/shopping/bought');
  }

  addItem(item: ShoppingItem): Observable<ShoppingItem> {
    return this.http.post<ShoppingItem>('/api/shopping', item);
  }

  deleteItem(id: string): Observable<void> {
    return this.http.delete<void>(`/api/shopping/${id}`);
  }

  markAsImportant(id: string): Observable<void> {
    return this.http.put<void>(`/api/shopping/markAsImportant/${id}`, {});
  }

  moveToBought(id: string): Observable<void> {
    return this.http.post<void>(`/api/shopping/moveToBought/${id}`, {});
  }

  addToBuy(id: string): Observable<void> {
    return this.http.post<void>(`/api/shopping/addToBuy/${id}`, {});
  }

  addItemWithQuantity(item: ShoppingItem, quantity: number): Observable<ShoppingItem> {
    return this.http.post<ShoppingItem>('/api/shopping/withQuantity', {item, quantity});
  }
}
