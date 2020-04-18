import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PayrollChartsComponent } from './payroll-charts.component';

describe('PayrollChartsComponent', () => {
  let component: PayrollChartsComponent;
  let fixture: ComponentFixture<PayrollChartsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PayrollChartsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PayrollChartsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
