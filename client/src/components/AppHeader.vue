<script setup lang="ts">
import { computed, onBeforeUnmount, onMounted, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import { useAuthStore } from '@/stores/auth.store'

const authStore = useAuthStore()
const { locale, t } = useI18n()

const isLanguageOpen = ref(false)
const languageRoot = ref<HTMLElement | null>(null)

const localeItems = [
  { code: 'hr', flagClass: 'fi fi-hr' },
  { code: 'en', flagClass: 'fi fi-gb' }
] as const

const currentLocale = computed(() => {
  return localeItems.find((item) => item.code === locale.value) ?? localeItems[0]
})

const displayName = computed(() => authStore.name?.trim() || t('header.userFallback'))
const displayEmail = computed(() => authStore.email?.trim() || t('header.noEmail'))
const initials = computed(() => {
  const parts = displayName.value
    .split(' ')
    .filter(Boolean)
    .slice(0, 2)
    .map((value) => value[0]?.toUpperCase() ?? '')
    .join('')

  return parts || 'U'
})

function setLocale(nextLocale: (typeof localeItems)[number]['code']) {
  locale.value = nextLocale
  isLanguageOpen.value = false
}

function toggleLanguageMenu() {
  isLanguageOpen.value = !isLanguageOpen.value
}

function closeLanguageMenu() {
  isLanguageOpen.value = false
}

function handleDocumentClick(event: MouseEvent) {
  if (!languageRoot.value) {
    return
  }

  if (!languageRoot.value.contains(event.target as Node)) {
    closeLanguageMenu()
  }
}

function handleEscape(event: KeyboardEvent) {
  if (event.key === 'Escape') {
    closeLanguageMenu()
  }
}

onMounted(() => {
  document.addEventListener('click', handleDocumentClick)
  document.addEventListener('keydown', handleEscape)
})

onBeforeUnmount(() => {
  document.removeEventListener('click', handleDocumentClick)
  document.removeEventListener('keydown', handleEscape)
})
</script>

<template>
  <header class="sticky top-0 z-50 w-full border-b border-[#218CD9]/40 bg-[#071C2C]/95 backdrop-blur">
    <div class="mx-auto flex h-16 w-full max-w-7xl items-center justify-between px-4 sm:px-6">
      <div class="flex items-center gap-8">
        <RouterLink to="/home" class="flex items-center gap-2">
          <svg viewBox="0 0 24 24" class="h-7 w-7 text-[#218CD9]" fill="none" aria-hidden="true">
            <circle cx="12" cy="12" r="9" stroke="currentColor" stroke-width="1.8" />
            <path d="M7 14h3l2-4 2 6 3-4" stroke="currentColor" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round" />
          </svg>
          <span class="text-lg font-bold text-white">ExchangeMapper</span>
        </RouterLink>

        <nav class="hidden items-center gap-5 md:flex">
          <RouterLink to="/home" class="nav-link">{{ t('header.nav.home') }}</RouterLink>
          <RouterLink to="/exchange" class="nav-link">{{ t('header.nav.exchange') }}</RouterLink>
          <RouterLink to="/history" class="nav-link">{{ t('header.nav.history') }}</RouterLink>
        </nav>
      </div>

      <div class="flex items-center gap-3">
        <section ref="languageRoot" class="relative">
          <button
            type="button"
            class="inline-flex items-center gap-2 rounded-full bg-[#218CD9] px-3 py-1.5 text-sm font-semibold text-white transition hover:bg-[#8AC4ED] hover:text-[#071C2C]"
            :aria-expanded="isLanguageOpen"
            aria-haspopup="listbox"
            @click="toggleLanguageMenu"
          >
            <span class="fi" :class="currentLocale.flagClass" aria-hidden="true"></span>
            <span>{{ t(`languageSwitcher.locales.${currentLocale.code}`) }}</span>
          </button>

          <ul
            v-if="isLanguageOpen"
            class="absolute right-0 mt-2 w-32 overflow-hidden rounded-xl border border-[#218CD9]/40 bg-[#071C2C] py-1 shadow-lg shadow-black/40"
            role="listbox"
          >
            <li v-for="item in localeItems" :key="item.code">
              <button
                type="button"
                class="flex w-full items-center gap-2 px-3 py-2 text-left text-sm text-[#CAE4F7] transition hover:bg-[#218CD9]/20"
                @click="setLocale(item.code)"
              >
                <span class="fi" :class="item.flagClass" aria-hidden="true"></span>
                <span>{{ t(`languageSwitcher.locales.${item.code}`) }}</span>
              </button>
            </li>
          </ul>
        </section>

        <div class="group relative">
          <button
            type="button"
            class="flex h-10 w-10 items-center justify-center rounded-full bg-[#218CD9] text-sm font-bold text-white"
            :aria-label="t('header.userMenu')"
          >
            {{ initials }}
          </button>
          <div
            class="invisible absolute right-0 top-12 w-56 rounded-xl border border-[#218CD9]/40 bg-[#071C2C] p-3 text-sm text-[#CAE4F7] opacity-0 shadow-lg shadow-black/40 transition group-hover:visible group-hover:opacity-100 group-focus-within:visible group-focus-within:opacity-100"
          >
            <p class="font-semibold text-white">{{ displayName }}</p>
            <p class="truncate text-[#8AC4ED]">{{ displayEmail }}</p>
          </div>
        </div>

        <button
          type="button"
          class="text-sm font-semibold text-[#CAE4F7] transition hover:text-red-300"
          @click="authStore.logout()"
        >
          {{ t('header.signOut') }}
        </button>
      </div>
    </div>
  </header>
</template>

<style scoped>
.nav-link {
  border-bottom: 2px solid transparent;
  color: #cae4f7;
  font-weight: 600;
  padding-bottom: 2px;
  transition: color 0.2s ease, border-color 0.2s ease;
}

.nav-link:hover {
  color: #8ac4ed;
}

.router-link-active {
  border-color: #218cd9;
  color: #218cd9;
}
</style>
