<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import { useAuthStore } from '@/stores/auth.store'

const authStore = useAuthStore()
const { t } = useI18n()
</script>

<template>
  <div>
    <h2>{{ t('authError.title') }}</h2>
    <p>{{ authStore.callbackError ?? t('authError.unknown') }}</p>
    <button @click="authStore.login('/')">{{ t('authError.retry') }}</button>
  </div>
</template>
