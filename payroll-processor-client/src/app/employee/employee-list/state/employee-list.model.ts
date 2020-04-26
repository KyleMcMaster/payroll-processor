export interface Employee {
  id: string;
  department: Department;
  employmentStartedOn: string;
  firstName: string;
  lastName: string;
  phone: string;
  status: Status;
  title: string;
}

export type Status = 'ACTIVE' | 'DISABLED';

export type Department = 'HR' | 'IT' | 'Sales' | 'Finance';

export function createInitialState(): Employee {
  return {
    id: '',
    department: 'IT',
    employmentStartedOn: '',
    firstName: '',
    lastName: '',
    phone: '',
    status: 'ACTIVE',
    title: '',
  };
}
