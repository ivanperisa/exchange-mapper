<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import { useAuthStore } from '@/stores/auth.store'
import '@/styles/logged-out-view.css'

const authStore = useAuthStore()
const { t } = useI18n()
</script>

<template>
  <main class="logged-out">
    <div class="panel">
      <h1>{{ t('loggedOut.title') }}</h1>
      <p>{{ t('loggedOut.message') }}</p>
      <button @click="authStore.login('/')">{{ t('loggedOut.loginAgain') }}</button>
    </div>
  </main>
</template>
