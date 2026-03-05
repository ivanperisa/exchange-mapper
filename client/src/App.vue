<script setup lang="ts">
import { RouterView } from 'vue-router'
import LanguageSwitcher from '@/components/LanguageSwitcher.vue'
import '@/styles/base.css'
</script>

<template>
  <div class="app-shell">
    <header class="global-controls">
      <LanguageSwitcher />
    </header>
    <RouterView />
  </div>
</template>
