<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import LanguageSwitcher from '@/components/LanguageSwitcher.vue'

const { t } = useI18n()

function redirectToLogin() {
  window.location.href = 'http://localhost:5000/auth/login?returnUrl=/home'
}
</script>

<template>
  <main class="relative min-h-screen overflow-hidden text-[#CAE4F7]">
    <div class="absolute right-4 top-4 z-20">
      <LanguageSwitcher />
    </div>

    <div class="absolute inset-0 flex">
      <div class="w-full bg-[#071C2C] lg:w-1/3"></div>
      <div class="hidden w-2/3 bg-gradient-to-br from-[#071C2C] via-[#0a2a42] to-[#071C2C] lg:block"></div>
    </div>

    <div class="relative z-10 flex min-h-screen flex-col lg:flex-row">
      <section class="relative flex w-full items-center px-6 py-12 lg:w-1/3 lg:px-12">
        <article
          class="landing-card w-full max-w-xl bg-[#071C2C] p-8 shadow-[24px_0_45px_rgba(7,28,44,0.65)] lg:translate-x-[80px] lg:rounded-[0_30px_30px_0]"
        >
          <h1 class="text-4xl font-black tracking-tight text-white sm:text-5xl">{{ t('common.appName') }}</h1>
          <p class="mt-4 text-lg font-semibold text-[#8AC4ED]">{{ t('landing.tagline') }}</p>
          <p class="mt-5 max-w-md text-sm leading-7 text-[#CAE4F7] sm:text-base">
            {{ t('landing.description') }}
          </p>

          <button
            class="mt-8 inline-flex items-center gap-3 rounded-xl bg-[#218CD9] px-6 py-3 text-sm font-bold text-white transition-colors hover:bg-[#8AC4ED] hover:text-[#071C2C]"
            @click="redirectToLogin"
          >
            <svg viewBox="0 0 24 24" class="h-5 w-5" fill="none" aria-hidden="true">
              <path
                d="M22 12.18c0-.79-.07-1.55-.21-2.28H12v4.31h5.6a4.8 4.8 0 0 1-2.08 3.15v2.61h3.37c1.97-1.82 3.11-4.49 3.11-7.79Z"
                fill="#4285F4"
              />
              <path
                d="M12 22c2.81 0 5.16-.93 6.88-2.53l-3.37-2.61c-.94.63-2.14 1-3.51 1-2.7 0-4.99-1.82-5.8-4.27H2.72v2.69A10.39 10.39 0 0 0 12 22Z"
                fill="#34A853"
              />
              <path
                d="M6.2 13.59a6.26 6.26 0 0 1 0-3.98V6.92H2.72a10.39 10.39 0 0 0 0 9.36l3.48-2.69Z"
                fill="#FBBC04"
              />
              <path
                d="M12 6.14c1.53 0 2.9.53 3.98 1.57l2.99-2.99C17.15 3.03 14.8 2 12 2A10.39 10.39 0 0 0 2.72 6.92l3.48 2.69C7.01 7.96 9.3 6.14 12 6.14Z"
                fill="#EA4335"
              />
            </svg>
            <span>{{ t('landing.signInWithGoogle') }}</span>
          </button>
        </article>
      </section>

      <section class="relative flex w-full flex-1 items-center justify-center overflow-hidden p-8">
        <svg viewBox="0 0 1000 700" class="network-svg absolute inset-0 h-full w-full opacity-70" aria-hidden="true">
          <g fill="none" stroke="#8AC4ED" stroke-width="2">
            <path d="M110 140 L320 200 L520 120 L760 220 L900 150" />
            <path d="M150 360 L350 300 L520 420 L730 340 L880 450" />
            <path d="M110 540 L310 470 L540 560 L760 500 L900 580" />
            <path d="M320 200 L350 300 L310 470" />
            <path d="M520 120 L520 420 L540 560" />
            <path d="M760 220 L730 340 L760 500" />
          </g>
          <g fill="#CAE4F7">
            <circle cx="110" cy="140" r="6" />
            <circle cx="320" cy="200" r="6" />
            <circle cx="520" cy="120" r="6" />
            <circle cx="760" cy="220" r="6" />
            <circle cx="900" cy="150" r="6" />
            <circle cx="150" cy="360" r="6" />
            <circle cx="350" cy="300" r="6" />
            <circle cx="520" cy="420" r="6" />
            <circle cx="730" cy="340" r="6" />
            <circle cx="880" cy="450" r="6" />
            <circle cx="110" cy="540" r="6" />
            <circle cx="310" cy="470" r="6" />
            <circle cx="540" cy="560" r="6" />
            <circle cx="760" cy="500" r="6" />
            <circle cx="900" cy="580" r="6" />
          </g>
        </svg>

        <div class="floating floating-a"></div>
        <div class="floating floating-b"></div>
        <div class="floating floating-c"></div>
        <div class="floating floating-d"></div>

        <h2 class="relative z-10 max-w-3xl text-center text-3xl font-black uppercase tracking-[0.2em] text-[#CAE4F7] sm:text-5xl">
          {{ t('landing.heroTitle') }}
        </h2>
      </section>
    </div>
  </main>
</template>

<style scoped>
.landing-card {
  border-radius: 1.5rem;
}

.floating {
  position: absolute;
  border-radius: 9999px;
  filter: blur(0.3px);
  animation: float 10s ease-in-out infinite;
}

.floating-a {
  width: 180px;
  height: 180px;
  top: 10%;
  left: 12%;
  background: rgba(33, 140, 217, 0.32);
}

.floating-b {
  width: 110px;
  height: 110px;
  top: 60%;
  left: 25%;
  background: rgba(138, 196, 237, 0.45);
  animation-delay: 1.3s;
}

.floating-c {
  width: 150px;
  height: 150px;
  top: 18%;
  right: 16%;
  background: rgba(202, 228, 247, 0.2);
  animation-delay: 2.1s;
}

.floating-d {
  width: 220px;
  height: 220px;
  top: 62%;
  right: 8%;
  background: rgba(33, 140, 217, 0.22);
  animation-delay: 0.8s;
}

.network-svg path {
  stroke-dasharray: 18 12;
  animation: dash 16s linear infinite;
}

@keyframes float {
  0% {
    transform: translateY(0) scale(1);
    opacity: 0.55;
  }

  50% {
    transform: translateY(-18px) scale(1.04);
    opacity: 0.9;
  }

  100% {
    transform: translateY(0) scale(1);
    opacity: 0.55;
  }
}

@keyframes dash {
  from {
    stroke-dashoffset: 420;
  }

  to {
    stroke-dashoffset: 0;
  }
}
</style>
