import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SalesOrderDto } from '../interfaces/sales-order-dto';
import { environment } from 'src/environments/environment';
import { PrintReceiptDto } from '../interfaces/print-receipt-dto';

// const url = sessionStorage.getItem('urls');
const url = environment.apiUrl;
const headers: HttpHeaders = new HttpHeaders()
  .set('Content-Type', 'application/json, charset=utf-8');

const formheaders: HttpHeaders = new HttpHeaders()
.set('Content-Type', 'multipart/form-data')


@Injectable({
  providedIn: 'root'
})
export class SalesOrderService {

  constructor(private http: HttpClient) {
  }

  create(itemDto: SalesOrderDto) {
    return this.http.post<any>(`${url}/createSalesOrder`, itemDto);
  }

  delete(id: number) {
    return this.http.delete<any>(`${url}/deleteSalesOrder/${id}`);
  }

  get(id: number) {
    return this.http.get<any>(`${url}/getSalesOrderById/${id}`);
  }

  getAllItemsByDate(date: string, id: number) {
    return this.http.get<any>(`${url}/getAllSalesOrderItem?id=${id}&date=${date}`, {headers});
  }


  getAll() {
    return this.http.get<any[]>(`${url}/getAllSalesOrders`);
  }

  update(itemDto: SalesOrderDto) {
    return this.http.put<any>(`${url}/updateSalesOrder`, itemDto);
  }

  printReceipt(receipt: PrintReceiptDto) {
    var formData = new FormData();
    formData.append('printerName', receipt.printerName);
    formData.append('receipt', receipt.receipt, `${this.generateGuid()}.pdf`);
    return this.http.post<any>(`https://localhost:8020/api/Receipt`, formData);
  }

  generateGuid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
        const r = Math.random() * 16 | 0; // Generate a random number between 0 and 15
        const v = c === 'x' ? r : (r & 0x3 | 0x8); // For 'y', ensure the first digit is 8, 9, A, or B
        return v.toString(16); // Convert to hexadecimal
    });
}
  
}
