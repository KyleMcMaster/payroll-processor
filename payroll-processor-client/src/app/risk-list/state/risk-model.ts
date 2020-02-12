export interface Risk {
  id: string;
  codeName: string;
  dispayName: string;
}

export function createInitialState(): Risk {
  return {
    id: '',
    codeName: '',
    dispayName: '',
  };
}

export const risks = [
  {
    id: '0',
    codeName: 'LOW',
    dispayName: 'Low',
  },
  {
    id: '1',
    codeName: 'MEDIUM',
    dispayName: 'Medium',
  },
  {
    id: '2',
    codeName: 'HIGH',
    dispayName: 'High',
  },
];
