export default {
  common: {
    appName: 'ExchangeMapper',
    signIn: 'Sign in with Google',
    signOut: 'Sign out',
    loading: 'Loading...',
    error: 'An error occurred.',
    na: 'N/A',
    language: 'Language',
    user: 'User'
  },
  landing: {
    tagline: 'Simplify your Erasmus course mapping',
    description:
      'Map your exchange courses, track ECTS credits, and get approval from your coordinator — all in one place.',
    visualTitle: 'Course Mapping Made Simple'
  },
  home: {
    welcome: 'Welcome back, {name}',
    noExchange: 'You have no active exchange. Get started below.',
    startExchange: 'Start Exchange Setup',
    institution: 'Home Institution',
    studyProgram: 'Study Program',
    studyProfile: 'Study Profile'
  },
  onboarding: {
    title: 'Account Setup',
    steps: {
      role: 'Role',
      institution: 'Home Institution',
      program: 'Program & Profile',
      confirm: 'Confirm'
    },
    role: {
      title: 'Choose your role',
      student: 'Student',
      coordinator: 'Coordinator',
      coordinatorNote: 'Your role will be confirmed.'
    },
    institution: {
      title: 'Select your home institution',
      searchPlaceholder: 'Search institutions...',
      addNew: '+ Add new institution',
      form: {
        name: 'Institution name',
        country: 'Country',
        city: 'City',
        erasmusCode: 'Erasmus code',
        iscedCode: 'ISCED code',
        programName: 'Study program name',
        profileName: 'Profile name'
      }
    },
    program: {
      title: 'Select program and profile',
      selectProgram: 'Select program',
      selectProfile: 'Select profile',
      addNewProfile: '+ Add new profile',
      profileNamePlaceholder: 'New profile name'
    },
    confirm: {
      title: 'Review your details',
      submit: 'Complete setup',
      role: 'Role',
      institution: 'Institution',
      program: 'Program',
      profile: 'Profile'
    },
    next: 'Next',
    back: 'Back',
    noResults: 'Not found.',
    errors: {
      selectRole: 'Choose your role before continuing.',
      institutionRequired: 'Select an institution or enter details for a new one.',
      programProfileRequired: 'Select a study program and profile or add a new profile.'
    }
  },
  errors: {
    required: 'This field is required.',
    notFound: 'Not found.',
    unexpected: 'An unexpected error occurred.',
    unauthorized: 'You are not authorized to view this page.'
  },
  nav: {
    home: 'Home',
    exchange: 'Exchange',
    history: 'History'
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
  },
  languageSwitcher: {
    dropdownLabel: 'Select language',
    locales: {
      hr: 'Croatian',
      en: 'English'
    }
  }
}
