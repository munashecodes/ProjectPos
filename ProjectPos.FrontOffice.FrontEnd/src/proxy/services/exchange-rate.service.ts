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
export class ExchangeRateService {

  constructor(private http: HttpClient) { }

  create(itemDto: any) {
    console.log(itemDto)
    var body = JSON.stringify(itemDto);
    console.log(body)
    return this.http.post<any>(`${url}/createExchangeRate`, body, { headers });
  }

  delete(id: number) {
    return this.http.delete<any>(`${url}/deleteExchangeRate/${id}`);
  }

  get(id: number) {
    return this.http.get<any>(`${url}/getExchangeRateById/${id}`);
  }

  getAllList() {
    return this.http.get<any>(`${url}/getAllExchangeRates`);
  }

  getByDate(itemDto: any) {
    console.log(itemDto)
    var body = JSON.stringify(itemDto);
    console.log(body)
    return this.http.get<any>(`${url}/getAllRatesByDate?date=${itemDto}`);
  }

  update(itemDto: any) {
    console.log(itemDto)
    var body = JSON.stringify(itemDto);
    console.log(body)
    return this.http.put<any>(`${url}/updateExchangeRate`, body, { headers });
  }
}
