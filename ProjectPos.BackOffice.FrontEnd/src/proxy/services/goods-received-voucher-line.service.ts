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
export class GoodsReceivedVoucherLineService {

  constructor(private http: HttpClient) {
  }

  getTodaySales() {
    return this.http.get<any>(`${url}/getTodayGrvLines`);
  }

  getMonthAllItems(month: number) {
    return this.http.get<any>(`${url}/getMonthGrvLines/${month}`);
  }

  getAllItemsByDate(date: string) {
    return this.http.get<any>(`${url}/getDateGrvLines?date=${date}`);
  }

  getAllItemsByDateRange(start: string, end: string) {
    return this.http.get<any>(`${url}/getRangeGrvLines?start=${start}&end=${end}`);
  }
}
