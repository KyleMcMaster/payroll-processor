import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { EnvService } from '@shared/env.service';
import { ToastrService } from 'ngx-toastr';
import { User, UserCreate } from './user.model';

@Injectable({ providedIn: 'root' })
export class UserService {
  private readonly apiRootUrl: string;

  constructor(
    envService: EnvService,
    private http: HttpClient,
    private toastr: ToastrService,
  ) {
    this.apiRootUrl = envService.apiRootUrl;
  }

  createEmployee(employee: UserCreate): void {
    this.http.post<User>(`${this.apiRootUrl}/users`, employee).subscribe({
      error: () => this.toastr.error(`Could not create user`),
      next: (detail) => {
        this.toastr.show(`Hello ${detail.firstName} ${detail.lastName}!`);
      },
    });
  }
}
