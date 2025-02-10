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
export class CashUpService {

  constructor(private http: HttpClient) { }

  create(itemDto: any) {
    console.log(itemDto)
    var body = JSON.stringify(itemDto);
    console.log(body)
    return this.http.post<any>(`${url}/createCashUp`, body, { headers });
  }

  delete(id: number) {
    return this.http.delete<any>(`${url}/deleteCashUp/${id}`);
  }

  get(id: number, date: any) {
    return this.http.get<any>(`${url}/getCashUpById?date=${date}/${id}`);
  }

  getAllList(date: any) {
    return this.http.get<any>(`${url}/getAllCashUps?date=${date}`);
  }

  getRecon(id: number, date: any) {
    return this.http.get<any>(`${url}/getReconById/${id}?date=${date}`);
  }

  getAllRecons(date: any) {
    return this.http.get<any>(`${url}/getAllRecons?date=${date}`);
  }

  update(itemDto: any) {
    console.log(itemDto)
    var body = JSON.stringify(itemDto);
    console.log(body)
    return this.http.put<any>(`${url}/updateCashUp`, body, { headers });
  }

  getAllCashUpRecons(date: any) {
    return this.http.get<any>(`${url}/getAllCashUpRecons?date=${date}`);
  }

  getCashUpReconById(id: any, date: any) {
    return this.http.get<any>(`${url}/getCashUpReconById/${id}?date=${date}`);
  }
}
