<template>
  <div class="mt-2 mx-4 text-xl">
    <label class="mr-2">Total Employess:</label>
    <span>{{ state.employeeCount }}</span>
  </div>
  <div class="mt-2 mx-4 text-xl">
    <label class="mr-2">Total Payrolls:</label>
    <span>{{ state.payrollCount }}</span>
  </div>
</template>

<script setup lang="ts">
import { adminStore } from '@/stores/admin';

const state = adminStore();

await state.getData();

</script>
