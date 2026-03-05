<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import { useAuthStore } from '@/stores/auth.store'
import '@/styles/home-view.css'

const authStore = useAuthStore()
const { t } = useI18n()
</script>

<template>
  <main class="home-page">
    <section class="hero">
      <p class="eyebrow">{{ t('common.appName') }}</p>
      <h1>{{ t('home.title') }}</h1>
      <p class="lead">{{ t('home.lead') }}</p>
      <div class="hero-actions">
        <button class="primary">{{ t('home.newScheme') }}</button>
        <button class="ghost">{{ t('home.importFromTable') }}</button>
      </div>
    </section>

    <section class="grid">
      <article class="card">
        <h2>{{ t('home.studentProfile.title') }}</h2>
        <p>
          <strong>{{ t('home.studentProfile.statusLabel') }}</strong>
          {{ authStore.isLoggedIn ? t('home.studentProfile.statusLoggedIn') : t('home.studentProfile.statusLoggedOut') }}
        </p>
        <p>
          <strong>{{ t('home.studentProfile.emailLabel') }}</strong>
          {{ authStore.user?.profile?.email ?? t('common.na') }}
        </p>
        <p v-if="authStore.callbackError" class="error">
          {{ t('home.studentProfile.authErrorLabel') }} {{ authStore.callbackError }}
        </p>
        <button class="danger" @click="authStore.logout()">{{ t('home.studentProfile.logout') }}</button>
      </article>

      <article class="card">
        <h2>{{ t('home.exchangeCourses.title') }}</h2>
        <ul>
          <li>{{ t('home.exchangeCourses.item1') }}</li>
          <li>{{ t('home.exchangeCourses.item2') }}</li>
          <li>{{ t('home.exchangeCourses.item3') }}</li>
          <li>{{ t('home.exchangeCourses.item4') }}</li>
        </ul>
      </article>

      <article class="card">
        <h2>{{ t('home.history.title') }}</h2>
        <ul>
          <li>{{ t('home.history.item1') }}</li>
          <li>{{ t('home.history.item2') }}</li>
          <li>{{ t('home.history.item3') }}</li>
          <li>{{ t('home.history.item4') }}</li>
        </ul>
      </article>
    </section>
  </main>
</template>
