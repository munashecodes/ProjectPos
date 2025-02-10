import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

// const url = sessionStorage.getItem('urls');
const url = environment.apiUrl;
const headers: HttpHeaders = new HttpHeaders()
  .set('Content-Type', 'application/json, charset=utf-8');

@Injectable({
  providedIn: 'root'
})
export class PurchaceOrderService {

  constructor(private http: HttpClient) { }

  create(itemDto: any) {
    console.log(itemDto)
    var body = JSON.stringify(itemDto);
    console.log(body)
    return this.http.post<any>(`${url}/createPurchaceOrder`, body, { headers });
  }

  delete(id: number) {
    return this.http.delete<any>(`${url}/deletePurchaceOrder/${id}`);
  }

  get(id: number) {
    return this.http.get<any>(`${url}/getPurchaceOrderById/${id}`);
  }

  getAllList() {
    return this.http.get<any>(`${url}/getAllPurchaceOrders`);
  }

  update(itemDto: any) {
    console.log(itemDto)
    var body = JSON.stringify(itemDto);
    console.log(body)
    return this.http.put<any>(`${url}/updatePurchaceOrder`, body, { headers });
  }

  getAllToday() {
    return this.http.get<any>(`${url}/getAllTodayPurchaceOrders`);
  }

  // get all goods received vouchers by date
  getAllByDate(date: any) {
    return this.http.get<any>(`${url}/getAllByDatePurchaceOrders?date=${date}`);
  }

  // get all goods received vouchers by date range
  getAllByDateRange(startDate: any, endDate: any) {
    return this.http.get<any>(`${url}/getAllByDateRangePurchaceOrders?start=${startDate}&end=${endDate}`);
  }

  // get all goods received vouchers by month
  getAllByMonth(month: number) {
    return this.http.get<any>(`${url}/getAllByMonthPurchaceOrders/${month}`);
  }

  // get all goods received vouchers by supplier id
  getAllBySupplier(supplierId: number) {
    return this.http.get<any>(`${url}/getAllBySupplierPurchaceOrders/${supplierId}`);
  }
}
