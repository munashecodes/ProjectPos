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
export class FileService {

  constructor(private http: HttpClient) { }

  create(itemDto: any) {

    let formdata = new FormData();
    formdata.append("file", itemDto);

    console.log(formdata)
    var body = JSON.stringify(itemDto);
    console.log(body)
    return this.http.post<any>(`${url}/saveFile`, formdata);
  }

  getFile(fileName: string){
    
  }
}
