export interface EmployeeDetail {
  id: string;
  department: Department;
  email: string;
  employmentStartedOn: string;
  firstName: string;
  lastName: string;
  payrolls: EmployeePayroll[];
  phone: string;
  status: Status;
  title: string;
  version: string;
}

export type Status = 'Enabled' | 'Disabled';

export type Department = 'HR' | 'IT' | 'Sales' | 'Finance' | 'UNKNOWN';

export function createInitialState(): EmployeeDetail {
  return {
    id: '',
    department: 'UNKNOWN',
    email: '',
    employmentStartedOn: '',
    firstName: '',
    lastName: '',
    payrolls: [],
    phone: '',
    status: 'Enabled',
    title: '',
    version: '',
  };
}

export interface EmployeeUpdate {
  id: string;
  department: Department;
  email: string;
  employmentStartedOn: string;
  firstName: string;
  lastName: string;
  phone: string;
  status: Status;
  title: string;
  version: string;
}

export interface EmployeePayroll {
  id: string;
  checkDate: string;
  employeeId: string;
  grossPayroll: number;
  payrollPeriod: number;
  version: string;
}
