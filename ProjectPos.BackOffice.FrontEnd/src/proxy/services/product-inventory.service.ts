import { Category } from 'src/proxy/enums/category';
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
    itemDto.product = undefined;
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

  getAllList() {
    return this.http.get<any>(`${url}/getAllProductInventories`);
  }

  update(itemDto: any) {
    console.log(itemDto)
    var body = JSON.stringify(itemDto);
    console.log(body)
    return this.http.put<any>(`${url}/updateProductInventory`, body, { headers });
  }

  updateRange(itemDto: any) {
    console.log(itemDto)
    var body = JSON.stringify(itemDto);
    console.log(body)
    return this.http.put<any>(`${url}/updateRangeProductInventory`, body, { headers });
  }

  generateInventory(){
    let itemDto = {
      id: 0
    }
    var body = JSON.stringify(itemDto);
    console.log(body)
    return this.http.post<any>(`${url}/generateInventory`, body, { headers });
  }

  generateStockTakeReport(department: Category) {
    return this.http.post<any>(`${url}/generateStockTakeReport?department=${department}`,  { headers });
  }

  generateStockValueReport(department: Category) {
    return this.http.post<any>(`${url}/generateStockValueReport?department=${department}`,  { headers });
  }

  createStockTakeLog(itemDto: any) {
    itemDto.product = undefined;
    console.log(itemDto)
    var body = JSON.stringify(itemDto);
    console.log(body)
    return this.http.post<any>(`${url}/generateStockTakeLog`, body, { headers });
  }

  closeStockTakeLog(itemDto: any) {
    itemDto.product = undefined;
    console.log(itemDto)
    var body = JSON.stringify(itemDto);
    console.log(body)
    return this.http.post<any>(`${url}/closeStockTakeLog`, body, { headers });
  }

  checkStockTakeLog(department: any) {
    return this.http.post<any>(`${url}/checkStockTakeLog?department=${department}`, { headers });
  }
}
