import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { EmployeeDetailsDto } from '../interfaces/employee-details-dto';
import { ServiceResponse } from '../interfaces/service-response';

const url = environment.apiUrl;
const headers: HttpHeaders = new HttpHeaders()
  .set('Content-Type', 'application/json, charset=utf-8');

@Injectable({
  providedIn: 'root'
})
export class EmployeeDetailsService {

  constructor(private http: HttpClient) {}

  createAsync(employeeDetails: EmployeeDetailsDto) {
    var body = JSON.stringify(employeeDetails);
    return this.http.post<ServiceResponse<EmployeeDetailsDto>>(`${url}/employeedetails/create`, body, {headers});
  }

  updateAsync(employeeDetails: EmployeeDetailsDto) {
    var body = JSON.stringify(employeeDetails);
    return this.http.put<ServiceResponse<EmployeeDetailsDto>>(`${url}/employeedetails/update`, body, {headers});
  }

  getByIdAsync(id: number) {
    return this.http.get<ServiceResponse<EmployeeDetailsDto>>(`${url}/employeedetails/getById/${id}`);
  }

  getByEmployeeIdAsync(employeeId: number) {
    return this.http.get<ServiceResponse<EmployeeDetailsDto>>(`${url}/employeedetails/getByEmployeeId/${employeeId}`);
  }

  getAllAsync() {
    return this.http.get<ServiceResponse<EmployeeDetailsDto[]>>(`${url}/employeedetails/getAll`);
  }

  deleteAsync(id: number) {
    return this.http.delete<ServiceResponse<boolean>>(`${url}/employeedetails/delete/${id}`);
  }
} 