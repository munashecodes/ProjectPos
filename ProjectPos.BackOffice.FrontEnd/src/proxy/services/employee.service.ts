import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

// const url = sessionStorage.getItem('urls');
const url = environment.apiUrl;
const headers: HttpHeaders = new HttpHeaders()
  .set('Content-Type', 'application/json, charset=utf-8');

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

  constructor(private http: HttpClient) { }

  create(itemDto: any) {
    console.log(itemDto)
    var body = JSON.stringify(itemDto);
    console.log(body)
    return this.http.post<any>(`${url}/createEmployee`, body, { headers });
  }

  delete(id: number) {
    return this.http.delete<any>(`${url}/deleteEmployeeById/${id}`);
  }

  get(id: number) {
    return this.http.get<any>(`${url}/getEmployeeById/${id}`);
  }

  getByName(name: string) {
    return this.http.get<any>(`${url}/getEmployeeById?name=${name}`);
  }

  getAllList() {
    return this.http.get<any>(`${url}/getAllEmployees`);
  }

  update(itemDto: any) {
    console.log(itemDto)
    var body = JSON.stringify(itemDto);
    console.log(body)
    return this.http.put<any>(`${url}/updateEmployee`, body, { headers });
  }
}
