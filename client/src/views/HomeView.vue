<script setup lang="ts">
import { computed } from 'vue'
import { useI18n } from 'vue-i18n'
import AppHeader from '@/components/AppHeader.vue'
import { useAuthStore } from '@/stores/auth.store'

const authStore = useAuthStore()
const { t } = useI18n()
const displayName = computed(() => authStore.name?.trim() || t('home.userFallback'))
</script>

<template>
  <main class="min-h-screen bg-[#071C2C]">
    <AppHeader />

    <section class="flex min-h-[calc(100vh-4rem)] items-center justify-center px-6">
      <div class="text-center">
        <h1 class="text-3xl font-bold text-[#CAE4F7] sm:text-4xl">{{ t('home.welcomeBack', { name: displayName }) }}</h1>
        <p class="mt-3 text-sm text-[#8AC4ED] sm:text-base">{{ t('home.noActiveExchange') }}</p>

        <RouterLink
          to="/exchange"
          class="mt-8 inline-flex rounded-xl bg-[#218CD9] px-6 py-3 font-semibold text-white transition hover:bg-[#8AC4ED] hover:text-[#071C2C]"
        >
          {{ t('home.startExchangeSetup') }}
        </RouterLink>
      </div>
    </section>
  </main>
</template>
