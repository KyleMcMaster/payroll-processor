export interface UserSession {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  phone: string;
  status: Status;
  version: string;
}

export type Status = 'Active' | 'Inactive';

export function createInitialState(): UserSession {
  return {
    id: '00000000-0000-0000-0000-000000000000',
    email: '',
    firstName: '',
    lastName: '',
    phone: '',
    status: 'Inactive',
    version: '',
  };
}
