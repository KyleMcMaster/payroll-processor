export interface Employee {
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

export type Status = 'Enabled' | 'Disabled';

export type Department =
  | 'Building Services'
  | 'Human Resources'
  | 'IT'
  | 'Marketing'
  | 'Sales'
  | 'Warehouse'
  | 'UNKNOWN';

export function createInitialState(): Employee {
  return {
    id: '',
    department: 'UNKNOWN',
    email: '',
    employmentStartedOn: '',
    firstName: '',
    lastName: '',
    phone: '',
    status: 'Enabled',
    title: '',
    version: '',
  };
}

export interface EmployeeCreate {
  department: Department;
  email: string;
  employmentStartedOn: string;
  firstName: string;
  lastName: string;
  phone: string;
  status: Status;
  title: string;
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
