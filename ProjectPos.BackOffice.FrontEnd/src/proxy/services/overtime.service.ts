import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { OvertimeRecordDto } from '../interfaces/overtime-record-dto';
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
    var model: DateQuery = {
      startDate: startDate,
      endDate: endDate
    };
    var body = JSON.stringify(model);
    return this.http.post<ServiceResponse<OvertimeRecordDto[]>>(`${url}/overtime/getByDateRange`, body, {headers});
  }

  getByDateRangeAndEmployeeIdAsync(startDate: Date, endDate: Date, employeeId: number) {
    const model: EmployeeDateQuery = {
      employeeId: employeeId,
      startDate: startDate,
      endDate: endDate
    };

    var body = JSON.stringify(model);
    return this.http.post<ServiceResponse<OvertimeRecordDto[]>>(`${url}/overtime/getByDateRangeAndEmployeeId`, body, {headers});
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