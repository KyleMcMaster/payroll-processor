export interface EmployeeListItem {
  id: string;
  department: Department;
  firstName: string;
  lastName: string;
  status: Status;
}

export type Status = 'Enabled' | 'Disabled';

export type Department = 'HR' | 'IT' | 'Sales' | 'Finance' | 'UNKNOWN';

export function createInitialState(): EmployeeListItem {
  return {
    id: '',
    department: 'UNKNOWN',
    firstName: '',
    lastName: '',
    status: 'Enabled',
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
