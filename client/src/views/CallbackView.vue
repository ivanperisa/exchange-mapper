<script setup lang="ts">
import { onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth.store'

const router = useRouter()
const authStore = useAuthStore()
const { t } = useI18n()

onMounted(async () => {
  try {
    await authStore.handleCallback()
    const returnTo =
      typeof authStore.user?.state === 'object' &&
      authStore.user.state !== null &&
      'returnTo' in authStore.user.state &&
      typeof authStore.user.state.returnTo === 'string'
        ? authStore.user.state.returnTo
        : '/'

    await router.replace(returnTo)
  } catch (error) {
    authStore.callbackError = error instanceof Error ? error.message : t('callback.failed')
    await router.replace('/auth-error')
  }
})
</script>

<template>
  <div>{{ t('callback.signingIn') }}</div>
</template>
