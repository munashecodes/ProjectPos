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
export class SubCategoryService {

  constructor(private http: HttpClient) { }

  create(itemDto: any) {
    console.log(itemDto)
    var body = JSON.stringify(itemDto);
    console.log(body)
    return this.http.post<any>(`${url}/createSubCategory`, body, { headers });
  }

  delete(id: number) {
    return this.http.delete<any>(`${url}/deleteSubCategory/${id}`);
  }

  get(id: number) {
    return this.http.get<any>(`${url}/getSubCategoryById/${id}`);
  }

  getAllList() {
    return this.http.get<any>(`${url}/getAllSubCategorys`);
  }

  update(itemDto: any) {
    console.log(itemDto)
    var body = JSON.stringify(itemDto);
    console.log(body)
    return this.http.put<any>(`${url}/updateSubCategory`, body, { headers });
  }
}
