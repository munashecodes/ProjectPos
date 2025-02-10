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
export class StockMovementServiceService {

  constructor(private http: HttpClient) { }

  createRange(itemDto: any) {
    console.log(itemDto)
    var body = JSON.stringify(itemDto);
    console.log(body)
    return this.http.post<any>(`${url}/addStockMovements`, body, { headers });
  }

  create(itemDto: any) {
    console.log(itemDto)
    var body = JSON.stringify(itemDto);
    console.log(body)
    return this.http.post<any>(`${url}/addStockMovement`, body, { headers });
  }

  delete(id: number) {
    return this.http.delete<any>(`${url}/deleteCompany/${id}`);
  }

  get(id: number) {
    return this.http.get<any>(`${url}/getCompanyById/${id}`);
  }

  getByMonth(id: number) {
    return this.http.get<any>(`${url}/getAllStockMovementsByMonth/${id}`);
  }

  approve(id: number, userId: number) {
    return this.http.put<any>(`${url}/approveStockMovement/${id}/${userId}`, { headers });
  }

  getAllList() {
    return this.http.get<any>(`${url}/getAllCompanies`);
  }

  getAllToday() {
    return this.http.get<any>(`${url}/getAllStockMovementsToday`);
  }

  getAllByDate(date: any) {
    return this.http.get<any>(`${url}/getAllStockMovementsByDate?date=${date}`);
  }

  getAllByDateRange(start: any, end: any) {
    return this.http.get<any>(`${url}/getAllStockMovementsByDateRange?startDate=${start}&endDate=${end}`);
  }

  update(itemDto: any) {
    console.log(itemDto)
    var body = JSON.stringify(itemDto);
    console.log(body)
    return this.http.put<any>(`${url}/updateCompany`, body, { headers });
  }
}
