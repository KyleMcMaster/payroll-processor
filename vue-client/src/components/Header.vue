<template>
  <header class="w-full p-2 px-5 shadow-xl text-primary flex">
    <img
      src="https://raw.githubusercontent.com/KyleMcMaster/kmcmaster.dev/feat/v3/public/github-logo.png"
      alt="logo"
      class="mt-1"
    />
    <span class="font-brand text-2xl flex items-center mx-1">
      <a href="/">Payroll Processor</a>
    </span>
    <span class="inline-flex px-2">
      <a href="/blogs">Employess</a>
    </span>
    <span class="inline-flex px-2">
      <a href="/blogs">Departments</a>
    </span>
    <span class="inline-flex px-2">
      <a href="/blogs">Admin</a>
    </span>
    <div class="flex-grow"></div>
    <ThemeToggle />
  </header>
</template>

<script lang="ts" setup>
import ThemeToggle from "./ThemeToggle.vue";
</script>

<style scoped>
img {
  width: 1%;
  height: 1%;
  overflow: hidden;
  border-radius: 50%;
}
</style>
