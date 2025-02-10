import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ExpenseDto } from '../interfaces/expense-dto';

// const url = sessionStorage.getItem('urls');
const url = environment.apiUrl;
const headers: HttpHeaders = new HttpHeaders()
  .set('Content-Type', 'application/json, charset=utf-8');

@Injectable({
  providedIn: 'root'
})
export class ExpenseService {

  constructor(private http: HttpClient) {}

  create(itemDto: ExpenseDto) {
    console.log(itemDto);
    const body = JSON.stringify(itemDto);
    console.log(body);
    return this.http.post<any>(`${url}/addExpense`, body, { headers });
  }

  delete(id: number, userId: number) {
    return this.http.delete<any>(`${url}/deleteExpense/${id}/${userId}`);
  }

  get(id: number) {
    return this.http.get<any>(`${url}/getExpense/${id}`);
  }

  getAllList() {
    return this.http.get<any>(`${url}/getAllExpenses`);
  }

  update(itemDto: ExpenseDto) {
    console.log(itemDto);
    const body = JSON.stringify(itemDto);
    console.log(body);
    return this.http.put<any>(`${url}/updateExpense`, body, { headers });
  }

  getByAccountId(accountId: number) {
    return this.http.get<any>(`${url}/getExpensesByAccountId/${accountId}`);
  }

  getByCompanyId(companyId: number) {
    return this.http.get<any>(`${url}/getExpensesByCompanyId/${companyId}`);
  }

  approve(itemDto: ExpenseDto) {
    console.log(itemDto);
    const body = JSON.stringify(itemDto);
    console.log(body);
    return this.http.put<any>(`${url}/approveExpense`, body, { headers });
  }
}
