export interface Risk {
  tag: string;
  isLow: boolean;
  isMedium: boolean;
  isHigh: boolean;
}

export function createInitialState(): Risk {
  return {
    tag: "",
    isLow: false,
    isHigh: false,
    isMedium: false
  };
}

export const risks = [
  {
    tag: "0",
    isLow: true,
    isHigh: false,
    isMedium: false
  }
];
