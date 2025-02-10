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
export class AccountCategoryService {

  constructor(private http: HttpClient) {}

  getAllasync() {
    return this.http.get<any>(`${url}/getAllAccountCategories`);
  }
}
