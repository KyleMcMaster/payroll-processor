import { Risk, risks } from 'src/app/risk-list/state/risk-model';

export interface EmployeePayroll {
  id: string;
  employeeId: string;
  firstName: string;
  lastName: string;
  payrollPeriod: number;
  risk: Risk;
}

export function createInitialState(): EmployeePayroll {
  return {
    id: '',
    employeeId: '',
    firstName: '',
    lastName: '',
    payrollPeriod: 1,
    risk: risks[1],
  };
}

export const employeePayrolls: EmployeePayroll[] = [
  {
    id: '0',
    employeeId: '0',
    firstName: 'Kyle',
    lastName: 'McMaster',
    payrollPeriod: 1,
    risk: risks[1],
  },
  {
    id: '1',
    employeeId: '1',
    firstName: 'Nathan',
    lastName: 'Harper',
    payrollPeriod: 1,
    risk: risks[2],
  },
  {
    id: '2',
    employeeId: '2',
    firstName: 'Justin',
    lastName: 'Conklin',
    payrollPeriod: 1,
    risk: risks[1],
  },
];
