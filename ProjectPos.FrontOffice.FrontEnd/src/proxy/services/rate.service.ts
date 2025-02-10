import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Currency } from '../enums/currency';
import { RateDto } from '../interfaces/rate-dto';
import { environment } from 'src/environments/environment';

// const url = sessionStorage.getItem('urls');
const url = environment.apiUrl;
const headers: HttpHeaders = new HttpHeaders()
  .set('Content-Type', 'application/json, charset=utf-8');

@Injectable({
  providedIn: 'root'
})
export class RateService {

  constructor(private http: HttpClient) {}

  create(itemDto: RateDto) {
    const body = JSON.stringify(itemDto)
    return this.http.post<any>(`${url}/createRate`, body, { headers });
  }

  getTodayRates() {
    return this.http.get<any>(`${url}/getTodaysRates`);
  }

  getRate(currency: Currency) {
    return this.http.get<any>(`${url}/getRate/${currency}`);
  }

  getAllRates() {
    return this.http.get<any>(`${url}/getAllRates`);
  }

  update(itemDto: RateDto) {
    return this.http.put<any>(`${url}`, itemDto);
  }
}
