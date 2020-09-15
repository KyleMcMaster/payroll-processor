import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { EnvService } from '@shared/env.service';

interface CreateDataRequest {
  employeesCount: number;
  payrollsMaxCount: number;
}

interface CreateDataResponse {
  totalEmployees: number;
  totalPayrolls: number;
  totalMilliseconds: number;
}

interface ResourceStatsResponse {
  totalEmployees: number;
  totalPayrolls: number;
}

@Injectable({ providedIn: 'root' })
export class ResourcesClient {
  constructor(
    private readonly http: HttpClient,
    private readonly env: EnvService,
  ) {}

  getStats(): Observable<ResourceStatsResponse> {
    return this.http.get<ResourceStatsResponse>(
      `${this.env.apiRootUrl}/resources/stats`,
    );
  }

  createResources(): Observable<void> {
    return this.http.post<void>(`${this.env.functionsRootUrl}/resources`, null);
  }

  createData(request: CreateDataRequest): Observable<CreateDataResponse> {
    const params = new HttpParams({
      fromObject: {
        employeesCount: request.employeesCount.toString(),
        payrollsMaxCount: request.payrollsMaxCount.toString(),
      },
    });

    return this.http.post<CreateDataResponse>(
      `${this.env.functionsRootUrl}/resources/data`,
      null,
      { params },
    );
  }

  resetResources(): Observable<void> {
    return this.http.delete<void>(`${this.env.functionsRootUrl}/resources`);
  }
}
