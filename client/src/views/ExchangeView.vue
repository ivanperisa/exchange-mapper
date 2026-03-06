<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import AppHeader from '@/components/AppHeader.vue'

const { t } = useI18n()
</script>

<template>
  <main class="min-h-screen bg-[#071C2C]">
    <AppHeader />
    <section class="flex min-h-[calc(100vh-4rem)] items-center justify-center px-6 text-center">
      <div>
        <h1 class="text-3xl font-bold text-[#CAE4F7]">{{ t('exchange.title') }}</h1>
        <p class="mt-3 text-[#8AC4ED]">{{ t('exchange.placeholder') }}</p>
      </div>
    </section>
  </main>
</template>
