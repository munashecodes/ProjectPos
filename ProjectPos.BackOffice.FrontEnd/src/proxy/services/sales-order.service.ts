import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SalesOrderDto } from '../interfaces/sales-order-dto';
import { ServiceResponse } from '../interfaces/service-response';
import { environment } from 'src/environments/environment';
import { start } from 'repl';

// const url = sessionStorage.getItem('urls');
const url = environment.apiUrl;
const headers: HttpHeaders = new HttpHeaders()
  .set('Content-Type', 'application/json, charset=utf-8');

@Injectable({
  providedIn: 'root'
})
export class SalesOrderService {

  constructor(private http: HttpClient) {
  }

  create(itemDto: SalesOrderDto) {
    return this.http.post<any>(`${url}/createSalesOrder`, itemDto);
  }

  delete(id: number) {
    return this.http.delete<ServiceResponse<SalesOrderDto>>(`${url}/deleteSalesOrder/${id}`);
  }

  get(id: number) {
    return this.http.get<any>(`${url}/getSalesOrderById/${id}`);
  }

  getByMonth(id: number) {
    return this.http.get<any>(`${url}/getSalesOrderById/${id}`);
  }

  getAll() {
    return this.http.get<any>(`${url}/getAllSalesOrders`);
  }

  getAllToday() {
    return this.http.get<any>(`${url}/getAllTodaySalesOrders`);
  }

  getTodaySales(id: number) {
    return this.http.get<any>(`${url}/getAllSalesOrderItems?id=${id}`);
  }

  getMonthAllItems(month: number, id: number) {
    return this.http.get<any>(`${url}/getAllMonthSalesOrderItems/${month}?id=${id}`);
  }

  getAllItemsByDate(date: string, id: number) {
    return this.http.get<any>(`${url}/getAllSalesOrderItem?id=${id}&date=${date}`);
  }

  getAllItemsByDateRange(start: string, end: string, id: number) {
    return this.http.get<any>(`${url}/getAllSalesOrderItemByRange?id=${id}&start=${start}&end=${end}`);
  }

  getAllByDate(date: any) {
    return this.http.get<any>(`${url}/getAllOrdersByDate?date=${date}`);
  }

  getAllByMonth(id: any) {
    return this.http.get<any>(`${url}/getAllOrdersByMonth/${id}`);
  }

  getAllByDateRange(startDate: any, endDate: any) {
    return this.http.get<any>(`${url}/getAllOrdersByRange?date=${startDate}&endDate=${endDate}`);
  }

  update(itemDto: SalesOrderDto) {
    return this.http.put<any>(`${url}/updateSalesOrder`, itemDto);
  }
  
}
