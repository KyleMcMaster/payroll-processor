<template>
  <Suspense>
    <!-- component with nested async dependencies -->
    <Admin />

    <!-- loading state via #fallback slot -->
    <template #fallback>Loading...</template>
  </Suspense>
</template>

<script setup lang="ts">
import Admin from '../components/Admin.vue'
</script>
