import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CogsReport, CostOfGoodsReport } from '../interfaces/cogs-report-dtos';
import { ServiceResponse } from '../interfaces/service-response-dto';
import { environment } from 'src/environments/environment';

const baseUrl = environment.apiUrl
@Injectable({ providedIn: 'root' })
export class ReportService {

  constructor(private http: HttpClient) { }

  getCogsReport(filter: ReportFilter): Observable<ServiceResponse<CogsReport>> {
    let params = new HttpParams();
    if (filter.startDate) params = params.set('startDate', filter.startDate);
    if (filter.endDate) params = params.set('endDate', filter.endDate);
    if (filter.timeRange) params = params.set('timeRange', filter.timeRange);
    
    return this.http.get<ServiceResponse<CogsReport>>(`${baseUrl}/CostOfGoods/cogs`, { params });
  }

  getCostOfGoodsReport(): Observable<ServiceResponse<CostOfGoodsReport>> {
    return this.http.get<ServiceResponse<CostOfGoodsReport>>(`${baseUrl}/CostOfGoods/cost-of-goods`);
  }
}

export interface ReportFilter {
  startDate?: any;
  endDate?: any;
  timeRange?: 'today' | 'month' | 'year' | 'custom' | 'date';
}

