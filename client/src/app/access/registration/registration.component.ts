import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { MsalService } from '@azure/msal-angular';
import { UserCreate } from './state/user.model';
import { UserService } from './state/user.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css'],
})
export class RegistrationComponent implements OnInit {
  filterForm = new FormGroup({
    email: new FormControl(''),
    firstName: new FormControl(''),
    lastName: new FormControl(''),
    phone: new FormControl(''),
  });

  constructor(
    private authService: MsalService,
    private userService: UserService,
    private router: Router,
  ) {}

  create() {
    const user: UserCreate = {
      accountId: this.authService.getAccount().accountIdentifier,
      email: this.filterForm.get('email').value,
      firstName: this.filterForm.get('firstName').value,
      lastName: this.filterForm.get('lastName').value,
      phone: this.filterForm.get('phone').value,
      status: 'Active',
    };

    this.userService.createEmployee(user);

    this.router.navigate(['/']);
  }

  ngOnInit(): void {}
}
