import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { OvertimeRecordDto } from '../interfaces/overtime-record-dto';
import { ServiceResponse } from '../interfaces/service-response';

const url = environment.apiUrl;
const headers: HttpHeaders = new HttpHeaders()
  .set('Content-Type', 'application/json, charset=utf-8');

@Injectable({
  providedIn: 'root'
})
export class OvertimeService {

  constructor(private http: HttpClient) {}

  getAllAsync() {
    return this.http.get<ServiceResponse<OvertimeRecordDto[]>>(`${url}/overtime/getAll`);
  }

  getByIdAsync(id: number) {
    return this.http.get<ServiceResponse<OvertimeRecordDto>>(`${url}/overtime/getById/${id}`);
  }

  getByEmployeeIdAsync(employeeId: number) {
    return this.http.get<ServiceResponse<OvertimeRecordDto[]>>(`${url}/overtime/getByEmployeeId/${employeeId}`);
  }

  getByDateRangeAsync(startDate: Date, endDate: Date) {
    return this.http.get<ServiceResponse<OvertimeRecordDto[]>>(`${url}/overtime/getByDateRange?startDate=${startDate}&endDate=${endDate}`);
  }

  getByDateRangeAndEmployeeIdAsync(startDate: Date, endDate: Date, employeeId: number) {
    return this.http.get<ServiceResponse<OvertimeRecordDto[]>>(`${url}/overtime/getByDateRangeAndEmployeeId/${employeeId}?startDate=${startDate}&endDate=${endDate}`);
  }

  createAsync(overtime: OvertimeRecordDto) {
    var body = JSON.stringify(overtime);
    return this.http.post<ServiceResponse<OvertimeRecordDto>>(`${url}/overtime/create`, body, {headers});
  }

  updateAsync(overtime: OvertimeRecordDto) {
    var body = JSON.stringify(overtime);
    return this.http.put<ServiceResponse<OvertimeRecordDto>>(`${url}/overtime/update`, body, {headers});
  }

  deleteAsync(id: number) {
    return this.http.delete<boolean>(`${url}/overtime/delete/${id}`);
  }

  approveAsync(id: number, userId: number) {
    return this.http.put<ServiceResponse<OvertimeRecordDto>>(`${url}/overtime/approve/${id}/${userId}`, null);
  }
} 