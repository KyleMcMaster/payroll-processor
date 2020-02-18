export interface Risk {
  id: string;
  codeName: string;
  displayName: string;
}

export function createInitialState(): Risk {
  return {
    id: '',
    codeName: '',
    displayName: '',
  };
}

export const risks: Risk[] = [
  {
    id: '0',
    codeName: 'LOW',
    displayName: 'Low',
  },
  {
    id: '1',
    codeName: 'MEDIUM',
    displayName: 'Medium',
  },
  {
    id: '2',
    codeName: 'HIGH',
    displayName: 'High',
  },
];
