import { Status } from 'src/app/employee/employee-list/state/employee-list.model';

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

export interface PayrollResponse {
  data: Payroll[];
}

export function createInitialState(): Payroll {
  return {
    id: '',
    checkDate: '',
    employeeDepartment: '',
    employeeId: '',
    employeeName: '',
    employeeStatus: 'Enabled',
    grossPayroll: 0,
    payrollPeriod: 1,
    risk: 'LOW',
  };
}

export type Risk = 'HIGH' | 'MEDIUM' | 'LOW';
