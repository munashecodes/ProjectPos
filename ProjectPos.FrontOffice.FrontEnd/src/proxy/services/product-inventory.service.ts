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
export class ProductInventoryService {

  constructor(private http: HttpClient) { }

  create(itemDto: any) {
    console.log(itemDto)
    var body = JSON.stringify(itemDto);
    console.log(body)
    return this.http.post<any>(`${url}/createProductInventory`, body, { headers });
  }

  delete(id: number) {
    return this.http.delete<any>(`${url}/deleteProductInventory/${id}`);
  }

  get(id: number) {
    return this.http.get<any>(`${url}/getProductInventoryById/${id}`);
  }

  getByPlu(plu: string) {
    return this.http.get<any>(`${url}/getProductInventoryPlu?plu=${plu}`);
  }

  getByCode(barCode: string) {
    return this.http.get<any>(`${url}/getProductInventoryByCode?barCode=${barCode}`);
  }

  getAllList() {
    return this.http.get<any>(`${url}/getAllPricedProductInventories`);
  }

  update(itemDto: any) {
    console.log(itemDto)
    var body = JSON.stringify(itemDto);
    console.log(body)
    return this.http.put<any>(`${url}/updateProductInventory`, body, { headers });
  }
}
