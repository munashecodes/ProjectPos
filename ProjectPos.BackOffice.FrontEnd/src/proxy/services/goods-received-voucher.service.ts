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
export class GoodsReceivedVoucherService {

  constructor(private http: HttpClient) { }

  create(itemDto: any) {
    console.log(itemDto)
    var body = JSON.stringify(itemDto);
    console.log(body)
    return this.http.post<any>(`${url}/createGoodsReceived`, body, { headers });
  }

  delete(id: number) {
    return this.http.delete<any>(`${url}/updateGoodsReceived/${id}`);
  }

  get(id: number) {
    return this.http.get<any>(`${url}/getGoodsReceivedVoucherById/${id}`);
  }

  getAllList() {
    return this.http.get<any>(`${url}/getAllGoodsReceivedVouchers`);
  }

  getAllToday() {
    return this.http.get<any>(`${url}/getAllTodayGoodsReceivedVouchers`);
  }

  // get all goods received vouchers by date
  getAllByDate(date: any) {
    return this.http.get<any>(`${url}/getAllByDateGoodsReceivedVouchers?date=${date}`);
  }

  // get all goods received vouchers by date range
  getAllByDateRange(startDate: any, endDate: any) {
    return this.http.get<any>(`${url}/getAllByDateRangeGoodsReceivedVouchers?start=${startDate}&end=${endDate}`);
  }

  // get all goods received vouchers by month
  getAllByMonth(month: number) {
    return this.http.get<any>(`${url}/getAllByMonthGoodsReceivedVouchers/${month}`);
  }

  // get all goods received vouchers by supplier id
  getAllBySupplier(supplierId: number) {
    return this.http.get<any>(`${url}/getAllBySupplierGoodsReceivedVouchers/${supplierId}`);
  }

  update(itemDto: any) {
    console.log(itemDto)
    var body = JSON.stringify(itemDto);
    console.log(body)
    return this.http.put<any>(`${url}/updateGoodsReceived`, body, { headers });
  }

  approve(itemDto: any) {
    console.log(itemDto)
    var body = JSON.stringify(itemDto);
    console.log(body)
    return this.http.put<any>(`${url}/approveGoodsReceived`, body, { headers });
  }
}
