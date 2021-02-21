import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { PayrollListComponent } from '@payroll/payroll-list/payroll-list.component';

describe('PayrollListComponent', () => {
  let component: PayrollListComponent;
  let fixture: ComponentFixture<PayrollListComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [PayrollListComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PayrollListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
