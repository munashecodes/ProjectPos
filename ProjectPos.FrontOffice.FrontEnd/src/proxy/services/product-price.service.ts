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
export class ProductPriceService {

  constructor(private http: HttpClient) { }

  create(itemDto: any) {
    console.log(itemDto)
    var body = JSON.stringify(itemDto);
    console.log(body)
    return this.http.post<any>(`${url}/createProductPrice`, body, { headers });
  }

  delete(id: number) {
    return this.http.delete<any>(`${url}/deleteProductPrice/${id}`);
  }

  get(id: number) {
    return this.http.get<any>(`${url}/getProductPriceById/${id}`);
  }

  getAllList() {
    return this.http.get<any>(`${url}/getAllProductPrices`);
  }

  update(itemDto: any) {
    console.log(itemDto)
    var body = JSON.stringify(itemDto);
    console.log(body)
    return this.http.put<any>(`${url}/updateProductPrice`, body, { headers });
  }
}
