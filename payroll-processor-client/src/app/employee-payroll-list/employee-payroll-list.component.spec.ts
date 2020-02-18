import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeePayrollListComponent } from './employee-payroll-list.component';

describe('EmployeePayrollListComponent', () => {
  let component: EmployeePayrollListComponent;
  let fixture: ComponentFixture<EmployeePayrollListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeePayrollListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeePayrollListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
