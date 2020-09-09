import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeePayrollCreateComponent } from './employee-payroll-create.component';

describe('EmployeePayrollCreateComponent', () => {
  let component: EmployeePayrollCreateComponent;
  let fixture: ComponentFixture<EmployeePayrollCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeePayrollCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeePayrollCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
