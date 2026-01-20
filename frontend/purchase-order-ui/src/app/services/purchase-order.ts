import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {
  CreatePurchaseOrderDto,
  PurchaseOrder,
  PurchaseOrderPage,
  PurchaseOrderQuery,
  UpdatePurchaseOrderDto,
} from '../models/purchase-order.model';

@Injectable({
  providedIn: 'root',
})
export class PurchaseOrderService {
  private apiUrl = '/api/purchaseorders';

  constructor(private http: HttpClient) {}

  getPurchaseOrders(query: PurchaseOrderQuery = {}): Observable<PurchaseOrderPage> {
    let params = new HttpParams();

    if (query.supplier) {
      params = params.set('supplier', query.supplier);
    }

    if (query.status) {
      params = params.set('status', query.status);
    }

    if (query.sortBy) {
      params = params.set('sortBy', query.sortBy);
    }

    if (query.sortDir) {
      params = params.set('sortDir', query.sortDir);
    }

    if (query.page) {
      params = params.set('page', query.page);
    }

    if (query.pageSize) {
      params = params.set('pageSize', query.pageSize);
    }

    return this.http.get<PurchaseOrderPage>(this.apiUrl, { params });
  }

  getPurchaseOrderById(id: number): Observable<PurchaseOrder> {
    return this.http.get<PurchaseOrder>(`${this.apiUrl}/${id}`);
  }

  createPurchaseOrder(dto: CreatePurchaseOrderDto): Observable<PurchaseOrder> {
    return this.http.post<PurchaseOrder>(this.apiUrl, dto);
  }

  updatePurchaseOrder(id: number, dto: UpdatePurchaseOrderDto): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, dto);
  }

  deletePurchaseOrder(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
