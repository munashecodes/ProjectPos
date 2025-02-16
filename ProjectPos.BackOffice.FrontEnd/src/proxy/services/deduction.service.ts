import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { EmployeeDeductionDto } from '../interfaces/employee-deduction-dto';
import { ServiceResponse } from '../interfaces/service-response';

const url = environment.apiUrl;
const headers: HttpHeaders = new HttpHeaders()
  .set('Content-Type', 'application/json, charset=utf-8');

@Injectable({
  providedIn: 'root'
})
export class DeductionService {

  constructor(private http: HttpClient) {}

  createAsync(deduction: EmployeeDeductionDto) {
    var body = JSON.stringify(deduction);
    return this.http.post<ServiceResponse<EmployeeDeductionDto>>(`${url}/deduction/create`, body, {headers});
  }

  updateAsync(deduction: EmployeeDeductionDto) {
    var body = JSON.stringify(deduction);
    return this.http.put<ServiceResponse<EmployeeDeductionDto>>(`${url}/deduction/update`, body, {headers});
  }

  getByIdAsync(id: number) {
    return this.http.get<ServiceResponse<EmployeeDeductionDto>>(`${url}/deduction/getById/${id}`);
  }

  getByEmployeeIdAsync(employeeId: number) {
    return this.http.get<ServiceResponse<EmployeeDeductionDto[]>>(`${url}/deduction/getByEmployeeId/${employeeId}`);
  }

  getByDateRangeAsync(startDate: string, endDate: string) {
    return this.http.get<ServiceResponse<EmployeeDeductionDto[]>>(
        `${url}/deduction/getByDateRange?startDate=${startDate}&endDate=${endDate}`
    );
  }

  getByEmployeeAndDateRangeAsync(employeeId: number, startDate: Date, endDate: Date) {
    return this.http.get<ServiceResponse<EmployeeDeductionDto[]>>(
      `${url}/deduction/getByEmployeeAndDateRange/${employeeId}?startDate=${startDate}&endDate=${endDate}`
    );
  }

  approveAsync(id: number) {
    return this.http.put<ServiceResponse<EmployeeDeductionDto>>(`${url}/deduction/approve/${id}`, null);
  }

  deleteAsync(id: number) {
    return this.http.delete<boolean>(`${url}/deduction/delete/${id}`);
  }
} 