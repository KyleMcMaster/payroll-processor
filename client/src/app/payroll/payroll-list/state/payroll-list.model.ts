import { Department } from 'src/app/employee/employee-list/state/employee-list.model';

export interface PayrollList {
  id: string;
  checkDate: string;
  employeeDepartment: Department;
  employeeFirstName: string;
  employeeLastName: string;
  grossPayroll: number;
  payrollPeriod: string;
  version: string;
}

export function createInitialState(): PayrollList {
  return {
    id: '',
    checkDate: '',
    employeeDepartment: 'UNKNOWN',
    employeeFirstName: '',
    employeeLastName: '',
    grossPayroll: 0.0,
    payrollPeriod: '',
    version: '',
  };
}
