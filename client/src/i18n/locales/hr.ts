export default {
  common: {
    appName: 'ExchangeMapper',
    signIn: 'Prijava putem Googlea',
    signOut: 'Odjava',
    loading: 'Učitavanje...',
    error: 'Došlo je do greške.',
    na: 'N/A',
    language: 'Jezik',
    user: 'Korisnik'
  },
  landing: {
    tagline: 'Pojednostavite preslikavanje kolegija za Erasmus',
    description:
      'Preslikajte kolegije s razmjene, pratite ECTS bodove i dobijte odobrenje koordinatora — sve na jednom mjestu.',
    visualTitle: 'Preslikavanje kolegija jednostavno'
  },
  home: {
    welcome: 'Dobrodošli, {name}',
    noExchange: 'Nemate aktivnu razmjenu. Krenite ispod.',
    startExchange: 'Postavi razmjenu',
    institution: 'Matični fakultet',
    studyProgram: 'Studijski program',
    studyProfile: 'Studijski profil'
  },
  onboarding: {
    title: 'Postavljanje računa',
    steps: {
      role: 'Uloga',
      institution: 'Matični fakultet',
      program: 'Program i profil',
      confirm: 'Potvrda'
    },
    role: {
      title: 'Odaberite svoju ulogu',
      student: 'Student',
      coordinator: 'Koordinator',
      coordinatorNote: 'Vaša uloga bit će potvrđena.'
    },
    institution: {
      title: 'Odaberite matični fakultet',
      searchPlaceholder: 'Pretraži fakultete...',
      addNew: '+ Dodaj novi fakultet',
      form: {
        name: 'Naziv fakulteta',
        country: 'Država',
        city: 'Grad',
        erasmusCode: 'Erasmus kod',
        iscedCode: 'ISCED kod',
        programName: 'Naziv studijskog programa',
        profileName: 'Naziv profila'
      }
    },
    program: {
      title: 'Odaberite program i profil',
      selectProgram: 'Odaberi program',
      selectProfile: 'Odaberi profil',
      addNewProfile: '+ Dodaj novi profil',
      profileNamePlaceholder: 'Naziv novog profila'
    },
    confirm: {
      title: 'Pregled podataka',
      submit: 'Završi postavljanje',
      role: 'Uloga',
      institution: 'Fakultet',
      program: 'Program',
      profile: 'Profil'
    },
    next: 'Sljedeći',
    back: 'Natrag',
    noResults: 'Nije pronađeno.',
    errors: {
      selectRole: 'Odaberite ulogu prije nastavka.',
      institutionRequired: 'Odaberite fakultet ili unesite podatke za novi fakultet.',
      programProfileRequired: 'Odaberite studijski program i profil ili unesite novi profil.'
    }
  },
  errors: {
    required: 'Ovo polje je obavezno.',
    notFound: 'Nije pronađeno.',
    unexpected: 'Došlo je do neočekivane greške.',
    unauthorized: 'Nemate pristup ovoj stranici.'
  },
  nav: {
    home: 'Početna',
    exchange: 'Razmjena',
    history: 'Povijest'
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
  },
  languageSwitcher: {
    dropdownLabel: 'Odaberi jezik',
    locales: {
      hr: 'Hrvatski',
      en: 'English'
    }
  }
}
