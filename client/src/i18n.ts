import { watch } from 'vue'
import { createI18n } from 'vue-i18n'

export const supportedLocales = ['hr', 'en'] as const
export type AppLocale = (typeof supportedLocales)[number]

const localeStorageKey = 'app-locale'

const messages = {
  hr: {
    common: {
      appName: 'ExchangeMapper',
      na: 'N/A',
      language: 'Jezik'
    },
    languageSwitcher: {
      dropdownLabel: 'Odaberi jezik',
      locales: {
        hr: 'Hrvatski',
        en: 'English'
      }
    },
    landing: {
      tagline: 'Pojednostavi mapiranje Erasmus kolegija',
      description:
        'Mapiraj kolegije na razmjeni, prati ECTS bodove i pošalji prijedlog koordinatoru na jednom mjestu.',
      signInWithGoogle: 'Prijavi se s Google računom',
      heroTitle: 'Mapiranje kolegija bez kaosa'
    },
    header: {
      nav: {
        home: 'Početna',
        exchange: 'Razmjena',
        history: 'Povijest'
      },
      signOut: 'Odjava',
      userMenu: 'Korisnički izbornik',
      noEmail: 'Email nije dostupan',
      userFallback: 'Korisnik'
    },
    home: {
      welcomeBack: 'Dobrodošao natrag, {name}',
      noActiveExchange: 'Nemaš aktivnu razmjenu. Započni ispod.',
      startExchangeSetup: 'Započni postavljanje razmjene',
      userFallback: 'student'
    },
    exchange: {
      title: 'Postavljanje razmjene',
      placeholder: 'Ova stranica je privremena i bit će implementirana uskoro.'
    },
    historyPage: {
      title: 'Povijest',
      placeholder: 'Ova stranica je privremena i bit će implementirana uskoro.'
    },
    callback: {
      signingIn: 'Prijava u tijeku...',
      failed: 'OAuth povratni poziv nije uspio.'
    }
  },
  en: {
    common: {
      appName: 'ExchangeMapper',
      na: 'N/A',
      language: 'Language'
    },
    languageSwitcher: {
      dropdownLabel: 'Select language',
      locales: {
        hr: 'Croatian',
        en: 'English'
      }
    },
    landing: {
      tagline: 'Simplify your Erasmus course mapping',
      description:
        'Map your exchange courses, track ECTS credits, and get coordinator approval in one place.',
      signInWithGoogle: 'Sign in with Google',
      heroTitle: 'Course Mapping Made Simple'
    },
    header: {
      nav: {
        home: 'Home',
        exchange: 'Exchange',
        history: 'History'
      },
      signOut: 'Sign out',
      userMenu: 'User menu',
      noEmail: 'No email available',
      userFallback: 'User'
    },
    home: {
      welcomeBack: 'Welcome back, {name}',
      noActiveExchange: 'You have no active exchange. Get started below.',
      startExchangeSetup: 'Start Exchange Setup',
      userFallback: 'student'
    },
    exchange: {
      title: 'Exchange Setup',
      placeholder: 'This page is a placeholder and will be implemented next.'
    },
    historyPage: {
      title: 'History',
      placeholder: 'This page is a placeholder and will be implemented next.'
    },
    callback: {
      signingIn: 'Signing in...',
      failed: 'OAuth callback failed.'
    }
  }
}

function normalizeLocale(value: string | null | undefined): AppLocale | null {
  if (!value) {
    return null
  }

  const base = value.toLowerCase().split('-')[0]
  return supportedLocales.includes(base as AppLocale) ? (base as AppLocale) : null
}

function resolveLocale(): AppLocale {
  if (typeof window === 'undefined') {
    return 'hr'
  }

  const saved = normalizeLocale(window.localStorage.getItem(localeStorageKey))
  if (saved) {
    return saved
  }

  const browserLocales = window.navigator.languages.length
    ? window.navigator.languages
    : [window.navigator.language]

  for (const browserLocale of browserLocales) {
    const detected = normalizeLocale(browserLocale)
    if (detected) {
      return detected
    }
  }

  return 'hr'
}

export const i18n = createI18n({
  legacy: false,
  locale: resolveLocale(),
  fallbackLocale: 'en',
  messages
})

watch(i18n.global.locale, (nextLocale) => {
  if (typeof window !== 'undefined') {
    window.localStorage.setItem(localeStorageKey, nextLocale)
  }
})
