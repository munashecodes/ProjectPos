import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ProductInventorySnapshotDto } from '../interfaces/product-inventory-snapshot-dto';
import { InventorySnapShotSummaryDto } from '../interfaces/inventory-snap-shot-summary-dto';

// const url = sessionStorage.getItem('urls');
const url = environment.apiUrl;
const headers: HttpHeaders = new HttpHeaders()
  .set('Content-Type', 'application/json, charset=utf-8');

@Injectable({
  providedIn: 'root'
})
export class ProductInventorySnapShotService {

  constructor(private http: HttpClient) { }

  create(snapShot: InventorySnapShotSummaryDto) {
    console.log(snapShot)
    var body = JSON.stringify(snapShot);
    console.log(body)
    return this.http.post<any>(`${url}/createInventorySnapShot`, body, { headers });
  }


  getAll(id: number) {
    return this.http.get<any>(`${url}/getInventorySnapShot`);
  }

  getByDate(date: string) {
    return this.http.get<any>(`${url}/getInventorySnapShot?date=${date}`);
  }
}
