<template>
<div class="sm:max-w-sm mx-auto rounded-xl overflow-hidden shadow-lg">
  <div class="px-6 py-4">
    <div class="font-bold text-xl mb-2">Resource Stats</div>
      <div>
    <label class="mr-6">Total Employess</label>
    <span>{{ state.employeeCount }}</span>
      </div>
      <div>
    <label class="mr-11">Total Payrolls</label>
    <span class="justify-self-center">{{ state.payrollCount }}</span>
      </div>
  </div>
</div>
</template>

<script setup lang="ts">
import { adminStore } from '@/stores/admin';

const state = adminStore();

await state.getData();

</script>
