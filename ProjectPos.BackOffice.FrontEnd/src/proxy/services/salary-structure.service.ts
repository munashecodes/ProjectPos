import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { SalaryStructureDto } from '../interfaces/salary-structure-dto';
import { ServiceResponse } from '../interfaces/service-response';

const url = environment.apiUrl;
const headers: HttpHeaders = new HttpHeaders()
  .set('Content-Type', 'application/json, charset=utf-8');

@Injectable({
  providedIn: 'root'
})
export class SalaryStructureService {

  constructor(private http: HttpClient) {}

  createAsync(salaryStructure: SalaryStructureDto) {
    var body = JSON.stringify(salaryStructure);
    return this.http.post<ServiceResponse<SalaryStructureDto>>(`${url}/createSalaryStructure`, body, {headers});
  }

  updateAsync(salaryStructure: SalaryStructureDto) {
    var body = JSON.stringify(salaryStructure);
    return this.http.put<ServiceResponse<SalaryStructureDto>>(`${url}/updateSalaryStructure`, body, {headers});
  }

  getByIdAsync(id: number) {
    return this.http.get<ServiceResponse<SalaryStructureDto>>(`${url}/getSalaryStructure/${id}`);
  }

  getAllAsync() {
    return this.http.get<ServiceResponse<SalaryStructureDto[]>>(`${url}/getAllSalaryStructures`);
  }

  getByEmployeeIdAsync(employeeId: number) {
    return this.http.get<ServiceResponse<SalaryStructureDto[]>>(`${url}/getSalaryStructuresByEmployee/${employeeId}`);
  }

  deleteAsync(userId: number, id: number) {
    return this.http.delete<boolean>(`${url}/deleteSalaryStructure/${userId}/${id}`);
  }
} 