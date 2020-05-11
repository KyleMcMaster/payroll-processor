import { Department } from 'src/app/employee/employee-list/state/employee-list.model';

export interface PayrollList {
  id: string;
  checkDate: Date;
  employeeDepartment: Department;
  employeeFirstName: string;
  employeeLastName: string;
  grossPayroll: number;
  version: string;
}

export function createInitialState(): PayrollList {
  return {
    id: '',
    checkDate: new Date(),
    employeeDepartment: 'UNKNOWN',
    employeeFirstName: '',
    employeeLastName: '',
    grossPayroll: 0.0,
    version: '',
  };
}
