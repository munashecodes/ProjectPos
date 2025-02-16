import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { EmployeeDeductionDto } from '../interfaces/employee-deduction-dto';
import { ServiceResponse } from '../interfaces/service-response';

interface DateQuery {
  startDate: Date;
  endDate: Date;
}

interface EmployeeDateQuery {
  employeeId: number;
  startDate: Date;
  endDate: Date;
}

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

  getByDateRangeAsync(startDate: Date, endDate: Date) {
      var model: DateQuery = {
        startDate: startDate,
        endDate: endDate,
      };
      var body = JSON.stringify(model);
      return this.http.post<ServiceResponse<EmployeeDeductionDto[]>>(
        `${url}/deduction/getByDateRange`,
        body,
        { headers }
      );
    }
  
    getByDateRangeAndEmployeeIdAsync(
      startDate: Date,
      endDate: Date,
      employeeId: number
    ) {
      const model: EmployeeDateQuery = {
        employeeId: employeeId,
        startDate: startDate,
        endDate: endDate,
      };
  
      var body = JSON.stringify(model);
      return this.http.post<ServiceResponse<EmployeeDeductionDto[]>>(
        `${url}/deduction/getByDateRangeAndEmployeeId`,
        body,
        { headers }
      );
    }

  approveAsync(id: number) {
    return this.http.put<ServiceResponse<EmployeeDeductionDto>>(`${url}/deduction/approve/${id}`, null);
  }

  deleteAsync(id: number) {
    return this.http.delete<boolean>(`${url}/deduction/delete/${id}`);
  }
} 