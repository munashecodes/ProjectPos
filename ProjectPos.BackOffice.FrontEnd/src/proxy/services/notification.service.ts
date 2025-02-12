import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

const url = environment.apiUrl;
const headers: HttpHeaders = new HttpHeaders()
  .set('Content-Type', 'application/json, charset=utf-8');
  
@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  constructor(private http: HttpClient) { }

  getStats(){
    return this.http.get<any>(`${url}/Notification`, { headers });
  }
}
