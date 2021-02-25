import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { PayrollComponent } from '@payroll/payroll.component';

describe('PayrollComponent', () => {
  let component: PayrollComponent;
  let fixture: ComponentFixture<PayrollComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [PayrollComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PayrollComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
