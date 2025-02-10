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
export class UserService {

  constructor(private http: HttpClient) { }

  create(itemDto: any) {
    console.log(itemDto)
    var body = JSON.stringify(itemDto);
    console.log(body)
    return this.http.post<any>(`${url}/signin`, body, { headers });
  }

  verifySupervisor(code: any) {
    return this.http.post<any>(`${url}/verifySupervisor?code=${code}`, { headers });
  }

  delete(id: number) {
    return this.http.delete<any>(`${url}/deleteUser/${id}`);
  }

  get(id: number) {
    return this.http.get<any>(`${url}/getById/${id}`);
  }

  getAllList() {
    return this.http.get<any>(`${url}/getAll`);
  }

  update(itemDto: any) {
    console.log(itemDto)
    var body = JSON.stringify(itemDto);
    console.log(body)
    return this.http.put<any>(`${url}/update`, body, { headers });
  }
}
