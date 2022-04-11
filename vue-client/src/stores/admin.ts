import { defineStore } from "pinia";

import { getSettings } from "../../settings";

const settings = getSettings();

export const adminStore = defineStore({
  id: "admin",
  state: () => ({
    totalEmployees: 0,
    totalPayrolls: 0,
  }),
  getters: {
    employeeCount: (state) => state.totalEmployees,
    payrollCount: (state) => state.totalPayrolls,
  },
  actions: {
    async getData() {
      const res = await fetch(`${settings.apiBaseUrl}/api/v1/resources/stats`);
      const data = await res.json();
      this.totalEmployees = data.totalEmployees;
      this.totalPayrolls = data.totalPayrolls;
    },
  },
});
