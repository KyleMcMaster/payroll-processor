export interface Employee {
  id: string;
  department: Department;
  readonly email: string;
  readonly employmentStartedOn: string;
  readonly firstName: string;
  lastName: string;
  phone: string;
  status: Status;
  title: string;
}

export interface EmployeeCreateResponse extends Employee {
  rowKey: string;
}

export type Status = 'ACTIVE' | 'DISABLED';

export type Department = 'HR' | 'IT' | 'Sales' | 'Finance' | 'UNKNOWN';

export function createInitialState(): Employee {
  return {
    id: '',
    department: 'UNKNOWN',
    email: '',
    employmentStartedOn: '',
    firstName: '',
    lastName: '',
    phone: '',
    status: 'ACTIVE',
    title: '',
  };
}

export interface EmployeeCreate {
  department: Department;
  readonly email: string;
  readonly employmentStartedOn: string;
  readonly firstName: string;
  lastName: string;
  phone: string;
  status: Status;
  title: string;
}
