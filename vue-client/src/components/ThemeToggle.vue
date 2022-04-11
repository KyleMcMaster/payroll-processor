<script setup lang="ts">
import { useDark, useToggle } from "@vueuse/core";
const isDark = useDark({
  selector: "html",
  attribute: "theme",
  valueLight: "light",
});
const toggleDark = useToggle(isDark);
</script>

<template>
  <div class="px-1 py-0.5 mt-1">
    <button @click="toggleDark()">
      <svg
        v-if="isDark"
        width="1.2em"
        height="1.2em"
        preserveAspectRatio="xMidYMid meet"
        viewBox="0 0 24 24"
      >
        <path
          d="M10 7a7 7 0 0 0 12 4.9v.1c0 5.523-4.477 10-10 10S2 17.523 2 12S6.477 2 12 2h.1A6.979 6.979 0 0 0 10 7zm-6 5a8 8 0 0 0 15.062 3.762A9 9 0 0 1 8.238 4.938A7.999 7.999 0 0 0 4 12z"
          fill="currentColor"
        />
      </svg>
      <svg
        v-if="!isDark"
        width="1.2em"
        height="1.2em"
        preserveAspectRatio="xMidYMid meet"
        viewBox="0 0 24 24"
      >
        <path
          d="M12 18a6 6 0 1 1 0-12a6 6 0 0 1 0 12zm0-2a4 4 0 1 0 0-8a4 4 0 0 0 0 8zM11 1h2v3h-2V1zm0 19h2v3h-2v-3zM3.515 4.929l1.414-1.414L7.05 5.636L5.636 7.05L3.515 4.93zM16.95 18.364l1.414-1.414l2.121 2.121l-1.414 1.414l-2.121-2.121zm2.121-14.85l1.414 1.415l-2.121 2.121l-1.414-1.414l2.121-2.121zM5.636 16.95l1.414 1.414l-2.121 2.121l-1.414-1.414l2.121-2.121zM23 11v2h-3v-2h3zM4 11v2H1v-2h3z"
          fill="currentColor"
        />
      </svg>
    </button>
  </div>
</template>
