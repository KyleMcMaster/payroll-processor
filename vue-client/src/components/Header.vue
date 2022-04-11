<template>
  <header class="w-full p-2 px-5 shadow-xl text-primary flex">
    <img
      src="https://raw.githubusercontent.com/KyleMcMaster/kmcmaster.dev/main/public/github-logo.png"
      alt="logo"
      class="mx-2 mt-2"
    />
    <span class="font-brand text-2xl flex items-center mx-1 mb-1">
      <RouterLink to="/">Payroll Processor</RouterLink>
    </span>
    <!-- <span class="inline-flex px-2">
      <RouterLink to="/Employees">Employees</RouterLink>
    </span>
    <span class="inline-flex px-2">
      <RouterLink to="/Departments">Departments</RouterLink>
    </span>-->
    <span class="inline-flex text-xl items-center mx-1">
      <RouterLink to="/Admin">Admin</RouterLink>
    </span>
    <div class="flex-grow"></div>
    <ThemeToggle />
  </header>
  <RouterView />
</template>

<script lang="ts" setup>

</script>

<script setup lang="ts">
import Header from "@/components/Header.vue";
import { RouterView } from "vue-router";
import ThemeToggle from "./ThemeToggle.vue";
</script>

<style scoped>
img {
  width: 3%;
  height: 3%;
  overflow: hidden;
  border-radius: 50%;
}
</style>
