import { Injectable } from '@angular/core';
import { DateRangeDto } from '../interfaces/date-range-dto';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';

const url = environment.apiUrl;
const headers: HttpHeaders = new HttpHeaders()
  .set('Content-Type', 'application/json, charset=utf-8');
  
@Injectable({
  providedIn: 'root'
})
export class TrialBalanceService {

  constructor(private http: HttpClient) { }
  
  getAll(item: DateRangeDto) {
    var body = JSON.stringify(item);
    return this.http.post<any>(`${url}/TrialBalance/get`, body, { headers });
  }
}
