import { Risk, risks } from 'src/app/risk-list/state/risk-model';

export interface EmployeePayroll {
  id: string;
  employeeId: string;
  firstName: string;
  grossPayroll: number;
  lastName: string;
  payrollPeriod: number;
  risk: Risk;
}

export function createInitialState(): EmployeePayroll {
  return {
    id: '',
    employeeId: '',
    firstName: '',
    grossPayroll: 0,
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
    grossPayroll: 1234.0,
    lastName: 'McMaster',
    payrollPeriod: 1,
    risk: risks[1],
  },
  {
    id: '1',
    employeeId: '1',
    firstName: 'Nathan',
    grossPayroll: 1234.0,
    lastName: 'Harper',
    payrollPeriod: 1,
    risk: risks[2],
  },
  {
    id: '2',
    employeeId: '2',
    firstName: 'Justin',
    grossPayroll: 1234.0,
    lastName: 'Conklin',
    payrollPeriod: 1,
    risk: risks[1],
  },
];
