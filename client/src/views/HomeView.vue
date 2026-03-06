<script setup lang="ts">
import { computed } from 'vue'
import { useI18n } from 'vue-i18n'
import AppHeader from '@/components/AppHeader.vue'
import { useAuthStore } from '@/stores/auth.store'

const authStore = useAuthStore()
const { t } = useI18n()

const displayName = computed(() => authStore.name?.trim() || t('common.user'))
const isCoordinator = computed(() => authStore.role === 'Coordinator')
const roleLabel = computed(() =>
  authStore.role === 'Coordinator' ? t('onboarding.role.coordinator') : t('onboarding.role.student')
)
const roleBadgeClass = computed(() =>
  authStore.role === 'Coordinator'
    ? 'bg-[#218CD9]/20 text-[#8AC4ED] border-[#218CD9]'
    : 'bg-green-500/15 text-green-300 border-green-400'
)
</script>

<template>
  <main class="min-h-screen bg-[#071C2C]">
    <AppHeader />

    <section class="mx-auto flex min-h-[calc(100vh-4rem)] w-full max-w-6xl items-center justify-center px-6 py-10">
      <div class="w-full">
        <h1 class="text-center text-3xl font-bold text-[#CAE4F7] sm:text-4xl">{{ t('home.welcome', { name: displayName }) }}</h1>
        <p class="mt-3 text-center text-sm text-[#8AC4ED] sm:text-base">{{ t('home.noExchange') }}</p>

        <div class="mt-8 grid gap-4" :class="isCoordinator ? 'md:grid-cols-1' : 'md:grid-cols-3'">
          <article
            class="rounded-xl border border-[#218CD9] bg-[#0A2235] p-5"
            :class="isCoordinator ? 'mx-auto w-full max-w-md' : ''"
          >
            <div class="mb-3 h-7 w-7 text-[#8AC4ED]">
              <svg viewBox="0 0 24 24" fill="currentColor">
                <path d="M4 22h16v-2H4v2Zm2-3h3v-7H6v7Zm5 0h2V8h-2v11Zm4 0h3V4h-3v15ZM3 10l9-7 9 7-1.2 1.6L12 5.2 4.2 11.6 3 10Z" />
              </svg>
            </div>
            <p class="text-xs uppercase tracking-wide text-[#8AC4ED]">{{ t('home.institution') }}</p>
            <p class="mt-1 text-lg font-semibold text-[#CAE4F7]">{{ authStore.institution?.name ?? t('common.na') }}</p>
            <p class="mt-1 text-sm text-slate-300">{{ authStore.institution?.country ?? t('common.na') }}</p>
          </article>

          <article v-if="!isCoordinator" class="rounded-xl border border-[#218CD9] bg-[#0A2235] p-5">
            <div class="mb-3 h-7 w-7 text-[#8AC4ED]">
              <svg viewBox="0 0 24 24" fill="currentColor">
                <path d="M4 5a3 3 0 0 1 3-3h13v18H7a3 3 0 0 0-3 3V5Zm3-1a1 1 0 0 0-1 1v14.1A4.92 4.92 0 0 1 7 19h11V4H7Zm2 4h7v2H9V8Zm0 4h7v2H9v-2Z" />
              </svg>
            </div>
            <p class="text-xs uppercase tracking-wide text-[#8AC4ED]">{{ t('home.studyProgram') }}</p>
            <p class="mt-1 text-lg font-semibold text-[#CAE4F7]">{{ authStore.studyProgram?.name ?? t('common.na') }}</p>
          </article>

          <article v-if="!isCoordinator" class="rounded-xl border border-[#218CD9] bg-[#0A2235] p-5">
            <div class="mb-3 h-7 w-7 text-[#8AC4ED]">
              <svg viewBox="0 0 24 24" fill="currentColor">
                <path
                  d="M12 12a4 4 0 1 0-4-4 4 4 0 0 0 4 4Zm0 2c-3.3 0-6 1.8-6 4v1h7.6l5.1-5.1-1.4-1.4-4.3 4.3H8v-.8c0-1.6 2-3 4-3 1.1 0 2.1.2 2.9.6l1.5-1.5A9.5 9.5 0 0 0 12 14Zm7.7 1.3-3.7 3.7-1.7-1.7-1.4 1.4 3.1 3.1 5.1-5.1-1.4-1.4Z"
                />
              </svg>
            </div>
            <p class="text-xs uppercase tracking-wide text-[#8AC4ED]">{{ t('home.studyProfile') }}</p>
            <p class="mt-1 text-lg font-semibold text-[#CAE4F7]">{{ authStore.studyProfile?.name ?? t('common.na') }}</p>
          </article>
        </div>

        <div class="mt-6 flex items-center justify-center">
          <span class="rounded-full border px-4 py-1 text-sm font-semibold" :class="roleBadgeClass">
            {{ roleLabel }}
          </span>
        </div>

        <div class="mt-8 text-center">
          <RouterLink
            to="/exchange"
            class="inline-flex rounded-xl bg-[#218CD9] px-6 py-3 font-semibold text-white transition hover:bg-[#8AC4ED] hover:text-[#071C2C]"
          >
            {{ t('home.startExchange') }}
          </RouterLink>
        </div>
      </div>
    </section>
  </main>
</template>
