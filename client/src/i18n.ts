import { watch } from 'vue'
import { createI18n } from 'vue-i18n'

export const supportedLocales = ['hr', 'en'] as const
export type AppLocale = (typeof supportedLocales)[number]

const localeStorageKey = 'app-locale'

const messages = {
  hr: {
    common: {
      appName: 'Exchange Mapper',
      na: 'N/A',
      language: 'Jezik'
    },
    languageSwitcher: {
      cycleButton: 'Promijeni jezik',
      dropdownLabel: 'Odaberi jezik',
      flagLabel: 'Brzi odabir',
      locales: {
        hr: 'Hrvatski',
        en: 'English'
      }
    },
    home: {
      title: 'Upravljanje preslikavanjem kolegija i ECTS bodova',
      lead: 'Jedno mjesto za studente i ECTS koordinatore: izrada shema, povijest promjena, prijevodi i mapiranje ocjena.',
      newScheme: 'Nova shema preslikavanja',
      importFromTable: 'Uvezi iz tablice',
      studentProfile: {
        title: 'Student profil',
        statusLabel: 'Status:',
        statusLoggedIn: 'Prijavljen',
        statusLoggedOut: 'Nije prijavljen',
        emailLabel: 'Email:',
        authErrorLabel: 'Auth greska:',
        logout: 'Odjava'
      },
      exchangeCourses: {
        title: 'Razmjena i kolegiji',
        item1: 'Odabir diplomskog studija FER-a',
        item2: 'Oznaka polozenih predmeta',
        item3: 'Unos predmeta za instituciju razmjene',
        item4: 'Drag & drop raspodjela ECTS bodova'
      },
      history: {
        title: 'Povijesni podaci',
        item1: 'Pregled prethodnih preslikavanja',
        item2: 'Automatski prijedlozi prijevoda',
        item3: 'Mapiranje ocjena izvan ECTS sustava',
        item4: 'Export / import Excel-kompatibilnog formata'
      }
    },
    callback: {
      signingIn: 'Prijava u tijeku...',
      failed: 'OAuth povratni poziv nije uspio.'
    },
    authError: {
      title: 'Prijava nije uspjela',
      unknown: 'Nepoznata OAuth greska.',
      retry: 'Pokusaj ponovno'
    },
    loggedOut: {
      title: 'Odjavljen si',
      message: 'Google sesija moze i dalje biti aktivna, ali lokalna aplikacijska sesija je obrisana.',
      loginAgain: 'Prijavi se ponovno'
    }
  },
  en: {
    common: {
      appName: 'Exchange Mapper',
      na: 'N/A',
      language: 'Language'
    },
    languageSwitcher: {
      cycleButton: 'Switch language',
      dropdownLabel: 'Select language',
      flagLabel: 'Quick select',
      locales: {
        hr: 'Croatian',
        en: 'English'
      }
    },
    home: {
      title: 'Course and ECTS mapping management',
      lead: 'One place for students and ECTS coordinators: schema creation, history tracking, translations, and grade mapping.',
      newScheme: 'New mapping schema',
      importFromTable: 'Import from table',
      studentProfile: {
        title: 'Student profile',
        statusLabel: 'Status:',
        statusLoggedIn: 'Logged in',
        statusLoggedOut: 'Logged out',
        emailLabel: 'Email:',
        authErrorLabel: 'Auth error:',
        logout: 'Log out'
      },
      exchangeCourses: {
        title: 'Exchange and courses',
        item1: 'Select FER graduate program',
        item2: 'Mark passed courses',
        item3: 'Add courses for exchange institution',
        item4: 'Drag & drop ECTS distribution'
      },
      history: {
        title: 'Historical data',
        item1: 'Review previous mappings',
        item2: 'Automatic translation suggestions',
        item3: 'Map grades outside the ECTS system',
        item4: 'Export / import Excel-compatible format'
      }
    },
    callback: {
      signingIn: 'Signing in...',
      failed: 'OAuth callback failed.'
    },
    authError: {
      title: 'Sign-in failed',
      unknown: 'Unknown OAuth error.',
      retry: 'Try again'
    },
    loggedOut: {
      title: 'You are logged out',
      message: 'Your Google session may still be active, but the local app session has been cleared.',
      loginAgain: 'Sign in again'
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
