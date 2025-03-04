import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ServiceResponse } from '../interfaces/service-response';
import { IncomeStatementDto } from '../interfaces/income-statement-dto';

const url = environment.apiUrl;
const headers: HttpHeaders = new HttpHeaders()
  .set('Content-Type', 'application/json, charset=utf-8');

@Injectable({
  providedIn: 'root'
})
export class IncomeStatementService {

  constructor(private http: HttpClient) {}

  generateIncomeStatementAsync(startDate: string, endDate: string) {
    return this.http.get<ServiceResponse<IncomeStatementDto>>(
      `${url}/incomestatement?startDate=${startDate}&endDate=${endDate}`
    );
  }
} 