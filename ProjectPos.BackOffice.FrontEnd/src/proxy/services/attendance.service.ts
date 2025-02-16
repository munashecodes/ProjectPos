import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { AttendanceDto } from '../interfaces/attendance-dto';
import { Observable } from 'rxjs';
import { ServiceResponse } from '../interfaces/service-response';

const url = environment.apiUrl;
const headers: HttpHeaders = new HttpHeaders()
  .set('Content-Type', 'application/json, charset=utf-8');

@Injectable({
  providedIn: 'root'
})
export class AttendanceService {

  constructor(private http: HttpClient) {}

  getAllAsync(){
    return this.http.get<ServiceResponse<AttendanceDto[]>>(`${url}/attendance/getAll`);
  }

  getByIdAsync(id: number) {
    return this.http.get<AttendanceDto>(`${url}/attendance/getById/${id}`);
  }

  //get all attendance by by date range
  getByDateRangeAsync(startDate: Date, endDate: Date) {
    return this.http.get<ServiceResponse<AttendanceDto[]>>(`${url}/attendance/getByDateRange?startDate=${startDate}&endDate=${endDate}`);
  }

  //get all attendance by date range and employee id
  getByDateRangeAndEmployeeIdAsync(startDate: Date, endDate: Date, employeeId: number) {
    return this.http.get<ServiceResponse<AttendanceDto[]>>(`${url}/attendance/getByDateRangeAndEmployeeId/${employeeId}?startDate=${startDate}&endDate=${endDate}`);
  }

  getByEmployeeIdAsync(employeeId: number) {
    return this.http.get<ServiceResponse<AttendanceDto[]>>(`${url}/attendance/getByEmployeeId/${employeeId}`);
  }

  createAsync(attendance: AttendanceDto) {
    var body = JSON.stringify(attendance);
    return this.http.post<ServiceResponse<AttendanceDto>>(`${url}/attendance/create`, body, {headers});
  }

  updateAsync(attendance: AttendanceDto) {
    var body = JSON.stringify(attendance);
    return this.http.put<ServiceResponse<AttendanceDto>>(`${url}/attendance/update`, body, {headers});
  }

  deleteAsync(id: number) {
    return this.http.delete<boolean>(`${url}/attendance/delete/${id}`);
  }
} 