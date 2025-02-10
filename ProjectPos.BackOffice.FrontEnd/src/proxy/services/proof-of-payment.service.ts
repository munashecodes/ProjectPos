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
export class ProofOfPaymentService {

  constructor(private http: HttpClient) { }

  create(itemDto: any) {
    console.log(itemDto)
    var body = JSON.stringify(itemDto);
    console.log(body)
    return this.http.post<any>(`${url}/createProofOfPayment`, body, { headers });
  }

  update(itemDto: any) {
    console.log(itemDto)
    var body = JSON.stringify(itemDto);
    console.log(body)
    return this.http.put<any>(`${url}/updateProofOfPayment`, body, { headers });
  }

  delete(id: number) {
    return this.http.delete<any>(`${url}/deleteProofOfPayment/${id}`);
  }

  get(id: number) {
    return this.http.get<any>(`${url}/getProofOfPayment/${id}`);
  }

  getByCustomer(id: number) {
    return this.http.get<any>(`${url}/getAllProofOfPayment/${id}`);
  }

  getAllList() {
    return this.http.get<any>(`${url}/getAllProofOfPayments`);
  }

  getByMonth(id: number) {
    return this.http.get<any>(`${url}/getProofOfPaymentById/${id}`);
  }

  getAll() {
    return this.http.get<any>(`${url}/getAllProofOfPayments`);
  }

  getAllByDate(date: any) {
    return this.http.get<any>(`${url}/getAllProofOfPaymentsByDate?date=${date}`);
  }

  getAllByMonth(id: any) {
    return this.http.get<any>(`${url}/getAllProofOfPaymentsByMonth/${id}`);
  }

  getAllByDateRange(startDate: any, endDate: any) {
    return this.http.get<any>(`${url}/getAllProofOfPaymentsByRange?date=${startDate}&endDate=${endDate}`);
  }

  getDateReportByCustomer(date: any) {
    return this.http.get<any>(`${url}/getDateReportByCustomer?date=${date}`);
  }

  getMonthReportByCustomer(id: any) {
    return this.http.get<any>(`${url}/getMonthReportByCustomer/${id}`);
  }

  getRangeReportByCustomer(startDate: any, endDate: any) {
    return this.http.get<any>(`${url}/getRangeReportByCustomer?date=${startDate}&endDate=${endDate}`);
  }
}
