export interface PayrollListItem {
  id: string;
  checkDate: string;
  employeeDepartment: Department;
  employeeFirstName: string;
  employeeLastName: string;
  grossPayroll: number;
  payrollPeriod: string;
  version: string;
}

export type Department =
  | 'Building Services'
  | 'Human Resources'
  | 'IT'
  | 'Marketing'
  | 'Sales'
  | 'Warehouse'
  | 'UNKNOWN';

export function createInitialState(): PayrollListItem {
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

export interface PayrollCreate {
  checkDate: string;
  employeeId: string;
  grossPayroll: number;
  payrollPeriod: string;
}
