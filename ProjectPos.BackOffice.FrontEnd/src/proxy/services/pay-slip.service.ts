import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ServiceResponse } from '../interfaces/service-response';
import { PayRollCycleDto } from '../interfaces/payroll-cycle-dto';
import { PaySlipDto } from '../interfaces/payslip-dto';

const url = environment.apiUrl;
const headers: HttpHeaders = new HttpHeaders()
  .set('Content-Type', 'application/json, charset=utf-8');

@Injectable({
  providedIn: 'root'
})
export class PaySlipService {

  constructor(private http: HttpClient) {}

  generatePayRollAsync(userId: number) {
    return this.http.post<ServiceResponse<PayRollCycleDto>>(`${url}/generatePayRoll/${userId}`, null);
  }

  approvePayRollAsync(month: number, year: number, userId: number) {
    return this.http.put<ServiceResponse<PayRollCycleDto>>(
      `${url}/approvePayRoll/${month}/${year}/${userId}`, null
    );
  }

  editPaySlipAsync(paySlip: PaySlipDto) {
    var body = JSON.stringify(paySlip);
    return this.http.put<ServiceResponse<PaySlipDto>>(`${url}/editPaySlip`, body, {headers});
  }

  getPayRollAsync(month: number, year: number) {
    return this.http.get<ServiceResponse<PayRollCycleDto>>(`${url}/getPayRoll/${month}/${year}`);
  }

  getPaySlipAsync(month: number, year: number, employeeId: number) {
    return this.http.get<ServiceResponse<PaySlipDto>>(
      `${url}/getPaySlip/${month}/${year}/${employeeId}`
    );
  }

  getAllPayRollCyclesAsync() {
    return this.http.get<ServiceResponse<PayRollCycleDto[]>>(`${url}/getAllPayRollCycles`);
  }

  getAllPayRollCyclesByYearAsync(year: number) {
    return this.http.get<ServiceResponse<PayRollCycleDto[]>>(`${url}/getAllPayRollCyclesByYear/${year}`);
  }
} 