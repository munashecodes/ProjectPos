import { HttpHeaders, HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { AttendanceDto } from "../interfaces/attendance-dto";
import { Observable } from "rxjs";
import { ServiceResponse } from "../interfaces/service-response";

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
const headers: HttpHeaders = new HttpHeaders().set(
  "Content-Type",
  "application/json, charset=utf-8"
);

@Injectable({
  providedIn: "root",
})
export class AttendanceService {
  constructor(private http: HttpClient) {}

  getAllAsync() {
    return this.http.get<ServiceResponse<AttendanceDto[]>>(
      `${url}/attendance/getAll`
    );
  }

  getByIdAsync(id: number) {
    return this.http.get<AttendanceDto>(`${url}/attendance/getById/${id}`);
  }

  getByDateRangeAsync(startDate: Date, endDate: Date) {
    var model: DateQuery = {
      startDate: startDate,
      endDate: endDate,
    };
    var body = JSON.stringify(model);
    return this.http.post<ServiceResponse<AttendanceDto[]>>(
      `${url}/attendance/getByDateRange`,
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
    return this.http.post<ServiceResponse<AttendanceDto[]>>(
      `${url}/attendance/getByDateRangeAndEmployeeId`,
      body,
      { headers }
    );
  }
  getByEmployeeIdAsync(employeeId: number) {
    return this.http.get<ServiceResponse<AttendanceDto[]>>(
      `${url}/attendance/getByEmployeeId/${employeeId}`
    );
  }

  createAsync(attendance: AttendanceDto) {
    var body = JSON.stringify(attendance);
    return this.http.post<ServiceResponse<AttendanceDto>>(
      `${url}/attendance/create`,
      body,
      { headers }
    );
  }

  updateAsync(attendance: AttendanceDto) {
    var body = JSON.stringify(attendance);
    return this.http.put<ServiceResponse<AttendanceDto>>(
      `${url}/attendance/update`,
      body,
      { headers }
    );
  }

  deleteAsync(id: number) {
    return this.http.delete<boolean>(`${url}/attendance/delete/${id}`);
  }
}
