import { Status } from 'src/app/employee/employee-list/state/employee-list.model';
import { Risk, risks } from 'src/app/risk-list/state/risk-model';

export interface Payroll {
  id: string;
  checkDate: string;
  employeeDepartment: string;
  employeeId: string;
  employeeName: string;
  employeeStatus: Status;
  grossPayroll: number;
  payrollPeriod: number;
  risk: Risk;
}

export function createInitialState(): Payroll {
  return {
    id: '',
    checkDate: '',
    employeeDepartment: '',
    employeeId: '',
    employeeName: '',
    employeeStatus: 'ACTIVE',
    grossPayroll: 0,
    payrollPeriod: 1,
    risk: risks[1],
  };
}
