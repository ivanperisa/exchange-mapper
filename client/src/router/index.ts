import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '@/stores/auth.store'
import LandingView from '@/views/LandingView.vue'
import HomeView from '@/views/HomeView.vue'
import CallbackView from '@/views/CallbackView.vue'
import ExchangeView from '@/views/ExchangeView.vue'
import HistoryView from '@/views/HistoryView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      component: LandingView
    },
    {
      path: '/home',
      component: HomeView,
      meta: { requiresAuth: true }
    },
    {
      path: '/exchange',
      component: ExchangeView,
      meta: { requiresAuth: true }
    },
    {
      path: '/history',
      component: HistoryView,
      meta: { requiresAuth: true }
    },
    { path: '/callback', component: CallbackView }
  ]
})

router.beforeEach(async (to) => {
  if (to.meta.requiresAuth) {
    const authStore = useAuthStore()
    await authStore.init()
    if (!authStore.isLoggedIn) {
      return '/'
    }
  }
})

export default router
