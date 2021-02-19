export interface UserCreate {
  accountId: string;
  email: string;
  firstName: string;
  lastName: string;
  phone: string;
  status: Status;
}

export interface User {
  id: string;
  accountId: string;
  email: string;
  firstName: string;
  lastName: string;
  phone: string;
  status: Status;
  version: string;
}

export type Status = 'Active' | 'Inactive';
