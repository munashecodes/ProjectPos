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

  get(id: number) {
    return this.http.get<any>(`${url}/getCashUpById/${id}`);
  }

  getAllList() {
    return this.http.get<any>(`${url}/getAllCasUps`);
  }

  update(itemDto: any) {
    console.log(itemDto)
    var body = JSON.stringify(itemDto);
    console.log(body)
    return this.http.put<any>(`${url}/updateCashUp`, body, { headers });
  }
}
