import { Component, OnInit } from '@angular/core';
import { DataService } from 'src/app/data/data-service';
import { NameValue } from 'src/app/data/state/data-model';

@Component({
  selector: 'app-payroll-charts',
  templateUrl: './payroll-charts.component.html',
  styleUrls: ['./payroll-charts.component.scss'],
})
export class PayrollChartsComponent implements OnInit {
  departmentDistributionData: any[] = [];
  employeeDistributionData: any[] = [];
  view: any[] = [700, 400];

  // options
  gradient = false;
  showLegend = true;
  showLabels = false;
  isDoughnut = false;
  legendPosition = 'below';

  colorScheme = {
    domain: ['#5AA454', '#A10A28', '#C7B42C', '#4287f5', '#AAAAAA'],
  };

  constructor(private dataService: DataService) {
    this.prepData();
  }

  ngOnInit(): void {}

  prepData() {
    this.prepDepartmentDistributionData();
    this.prepEmployeeDistributionData();
  }

  prepEmployeeDistributionData() {
    const payroll = this.dataService.getPayroll();
    const employees = this.dataService.getEmployees();
    const map: Map<string, number> = new Map();

    payroll.forEach(p => {
      employees.forEach(e => {
        if (e.status === 'ACTIVE') {
          const employeeName = e.firstName + ' ' + e.lastName;
          if (p.employeeId === e.id) {
            if (!map.has(employeeName)) {
              map.set(employeeName, p.grossPayroll);
            } else {
              const sum = map.get(employeeName) + p.grossPayroll;
              map.set(employeeName, sum);
            }
          }
        }
      });
    });

    this.employeeDistributionData = Array.from(
      map,
      ([key, value]) => new NameValue(key, value),
    );
  }

  prepDepartmentDistributionData() {
    const payroll = this.dataService.getPayroll();
    const employees = this.dataService.getEmployees();
    const map: Map<string, number> = new Map();

    payroll.forEach(pr => {
      if (pr.employeeStatus !== 'ACTIVE') {
        return;
      }
      return map.has(pr.employeeDepartment)
        ? map.set(
            pr.employeeDepartment,
            map.get(pr.employeeDepartment) + pr.grossPayroll,
          )
        : map.set(pr.employeeDepartment, pr.grossPayroll);
    });

    this.departmentDistributionData = Array.from(
      map,
      ([key, value]) => new NameValue(key, value),
    );
  }
}
