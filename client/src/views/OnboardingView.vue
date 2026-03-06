<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'
import { useI18n } from 'vue-i18n'
import { api } from '@/services/api'
import { useAuthStore } from '@/stores/auth.store'
import type { BaseResponse } from '@/types/api.types'
import type { InstitutionDto, StudyProgramDto, StudyProfileDto } from '@/types/institution.types'

type UserRole = 'Student' | 'Coordinator'

type InstitutionApiDto = InstitutionDto & {
  Id?: string
  Name?: string
  Country?: string
  City?: string | null
  ErasmusCode?: string | null
}

type StudyProgramApiDto = StudyProgramDto & {
  Id?: string
  Name?: string
  IscedCode?: string
}

type StudyProfileApiDto = StudyProfileDto & {
  Id?: string
  Name?: string
}

type NewInstitutionPayload = {
  name: string
  country: string
  city: string | null
  erasmusCode: string | null
  iscedCode: string
  programName: string
  profileName: string
}

type NewStudyProfilePayload = {
  studyProgramId: string
  profileName: string
}

type OnboardingPayload = {
  role: UserRole
  existingStudyProfileId: string | null
  existingInstitutionId: string | null
  newInstitution: NewInstitutionPayload | null
  newStudyProfile: NewStudyProfilePayload | null
}

const router = useRouter()
const authStore = useAuthStore()
const { locale, t } = useI18n()

const allSteps = [
  { id: 1, key: 'onboarding.steps.role' },
  { id: 2, key: 'onboarding.steps.institution' },
  { id: 3, key: 'onboarding.steps.program' },
  { id: 4, key: 'onboarding.steps.confirm' }
]

const currentStep = ref(1)
const selectedRole = ref<UserRole | null>(null)

const institutions = ref<InstitutionDto[]>([])
const searchQuery = ref('')
const selectedInstitution = ref<InstitutionDto | null>(null)
const showInstitutionDropdown = ref(false)
const createNewInstitution = ref(false)

const newInstitutionName = ref('')
const newInstitutionCountry = ref('')
const newInstitutionCity = ref('')
const newInstitutionErasmusCode = ref('')
const newInstitutionIscedCode = ref('')
const newInstitutionProgramName = ref('')
const newInstitutionProfileName = ref('')

const programs = ref<StudyProgramDto[]>([])
const profiles = ref<StudyProfileDto[]>([])
const selectedProgramId = ref<string | null>(null)
const selectedProfileId = ref<string | null>(null)
const createNewProfile = ref(false)
const newProfileName = ref('')

const errorMessage = ref<string | null>(null)
const isSubmitting = ref(false)

const isStudent = computed(() => selectedRole.value === 'Student')
const isCoordinator = computed(() => selectedRole.value === 'Coordinator')
const visibleSteps = computed(() => (isCoordinator.value ? allSteps.filter((step) => step.id !== 3) : allSteps))
const currentStepperPosition = computed(() => {
  const index = visibleSteps.value.findIndex((step) => step.id === currentStep.value)
  return index >= 0 ? index + 1 : 1
})

function toggleLocale() {
  locale.value = locale.value === 'hr' ? 'en' : 'hr'
  localStorage.setItem('locale', locale.value)
}

function logout() {
  window.location.href = `${import.meta.env.VITE_API_URL}/auth/logout`
}

const filteredInstitutions = computed(() => {
  const query = searchQuery.value.trim().toLowerCase()
  if (!query) {
    return institutions.value
  }

  return institutions.value.filter((institution) => institution.name.toLowerCase().includes(query))
})

const selectedProgram = computed(() => programs.value.find((program) => program.id === selectedProgramId.value) ?? null)
const selectedProfile = computed(() => profiles.value.find((profile) => profile.id === selectedProfileId.value) ?? null)
const selectedRoleLabel = computed(() => {
  if (selectedRole.value === 'Coordinator') {
    return t('onboarding.role.coordinator')
  }

  if (selectedRole.value === 'Student') {
    return t('onboarding.role.student')
  }

  return t('common.na')
})

function normalizeInstitution(dto: InstitutionApiDto): InstitutionDto {
  return {
    id: dto.id ?? dto.Id ?? '',
    name: dto.name ?? dto.Name ?? '',
    country: dto.country ?? dto.Country ?? '',
    city: dto.city ?? dto.City ?? null,
    erasmusCode: dto.erasmusCode ?? dto.ErasmusCode ?? null
  }
}

function normalizeProgram(dto: StudyProgramApiDto): StudyProgramDto {
  return {
    id: dto.id ?? dto.Id ?? '',
    name: dto.name ?? dto.Name ?? '',
    iscedCode: dto.iscedCode ?? dto.IscedCode ?? ''
  }
}

function normalizeProfile(dto: StudyProfileApiDto): StudyProfileDto {
  return {
    id: dto.id ?? dto.Id ?? '',
    name: dto.name ?? dto.Name ?? ''
  }
}

async function loadInstitutions() {
  try {
    const response = await api.get<BaseResponse<InstitutionApiDto[]>>('/institutions')
    institutions.value = (response.data.data ?? []).map(normalizeInstitution)
  } catch {
    errorMessage.value = t('errors.unexpected')
  }
}

async function onInstitutionSearchInput() {
  await loadInstitutions()
  showInstitutionDropdown.value = true
}

function selectInstitution(institution: InstitutionDto) {
  selectedInstitution.value = institution
  searchQuery.value = institution.name
  showInstitutionDropdown.value = false
  createNewInstitution.value = false
}

function enableNewInstitution() {
  selectedInstitution.value = null
  searchQuery.value = ''
  showInstitutionDropdown.value = false
  createNewInstitution.value = true
}

async function loadPrograms(institutionId: string) {
  try {
    const response = await api.get<BaseResponse<StudyProgramApiDto[]>>(`/institutions/${institutionId}/programs`)
    programs.value = (response.data.data ?? []).map(normalizeProgram)
  } catch {
    errorMessage.value = t('errors.unexpected')
  }
}

async function loadProfiles(programId: string) {
  if (!selectedInstitution.value) {
    return
  }

  try {
    const response = await api.get<BaseResponse<StudyProfileApiDto[]>>(
      `/institutions/${selectedInstitution.value.id}/programs/${programId}/profiles`
    )
    profiles.value = (response.data.data ?? []).map(normalizeProfile)
  } catch {
    errorMessage.value = t('errors.unexpected')
  }
}

async function onProgramChanged() {
  selectedProfileId.value = null
  createNewProfile.value = false
  newProfileName.value = ''
  profiles.value = []

  if (selectedProgramId.value) {
    await loadProfiles(selectedProgramId.value)
  }
}

function validateStep2(): boolean {
  if (createNewInstitution.value) {
    return Boolean(
      newInstitutionName.value.trim() &&
        newInstitutionCountry.value.trim() &&
        newInstitutionIscedCode.value.trim() &&
        newInstitutionProgramName.value.trim() &&
        newInstitutionProfileName.value.trim()
    )
  }

  return selectedInstitution.value !== null
}

function validateStep3(): boolean {
  if (!isStudent.value) {
    return true
  }

  if (!selectedProgramId.value) {
    return false
  }

  if (createNewProfile.value) {
    return newProfileName.value.trim().length > 0
  }

  return selectedProfileId.value !== null
}

async function goNext() {
  errorMessage.value = null

  if (currentStep.value === 1) {
    if (!selectedRole.value) {
      errorMessage.value = t('onboarding.errors.selectRole')
      return
    }
    currentStep.value = 2
    return
  }

  if (currentStep.value === 2) {
    if (!validateStep2()) {
      errorMessage.value = t('onboarding.errors.institutionRequired')
      return
    }

    if (createNewInstitution.value || isCoordinator.value) {
      currentStep.value = 4
      return
    }

    if (selectedInstitution.value) {
      await loadPrograms(selectedInstitution.value.id)
      currentStep.value = 3
    }
    return
  }

  if (currentStep.value === 3 && isStudent.value) {
    if (!validateStep3()) {
      errorMessage.value = t('onboarding.errors.programProfileRequired')
      return
    }
    currentStep.value = 4
  }
}

function goBack() {
  errorMessage.value = null

  if (currentStep.value === 4) {
    currentStep.value = createNewInstitution.value || isCoordinator.value ? 2 : 3
    return
  }

  if (currentStep.value > 1) {
    currentStep.value -= 1
  }
}

async function finishOnboarding() {
  if (!selectedRole.value) {
    errorMessage.value = t('onboarding.errors.selectRole')
    return
  }

  const payload: OnboardingPayload = {
    role: selectedRole.value,
    existingStudyProfileId:
      isStudent.value && !createNewInstitution.value && !createNewProfile.value ? selectedProfileId.value : null,
    existingInstitutionId: !createNewInstitution.value ? selectedInstitution.value?.id || null : null,
    newInstitution: createNewInstitution.value
      ? {
          name: newInstitutionName.value.trim(),
          country: newInstitutionCountry.value.trim(),
          city: newInstitutionCity.value.trim() || null,
          erasmusCode: newInstitutionErasmusCode.value.trim() || null,
          iscedCode: newInstitutionIscedCode.value.trim(),
          programName: newInstitutionProgramName.value.trim(),
          profileName: newInstitutionProfileName.value.trim()
        }
      : null,
    newStudyProfile:
      isStudent.value && !createNewInstitution.value && createNewProfile.value && selectedProgramId.value
        ? {
            studyProgramId: selectedProgramId.value,
            profileName: newProfileName.value.trim()
          }
        : null
  }

  try {
    isSubmitting.value = true
    errorMessage.value = null
    const response = await api.post<BaseResponse>('/auth/onboarding', payload)
    if (!response.data.success) {
      errorMessage.value = response.data.error?.message ?? t('errors.unexpected')
      return
    }

    await authStore.init(true)
    await router.push('/home')
  } catch {
    errorMessage.value = t('errors.unexpected')
  } finally {
    isSubmitting.value = false
  }
}

onMounted(async () => {
  await loadInstitutions()
})
</script>

<template>
  <main class="min-h-screen bg-[#071C2C] text-[#CAE4F7]">
    <header class="sticky top-0 z-40 w-full border-b border-[#218CD9] bg-[#071C2C]">
      <div class="mx-auto flex h-16 w-full max-w-5xl items-center justify-between px-4 sm:px-6">
        <span class="text-lg font-bold text-white">{{ t('common.appName') }}</span>
        <div class="flex items-center gap-3">
          <button
            type="button"
            class="inline-flex items-center rounded-full bg-[#218CD9] px-3 py-1.5 text-sm font-semibold text-white transition hover:bg-[#8AC4ED] hover:text-[#071C2C]"
            @click="toggleLocale"
          >
            {{ locale.toUpperCase() }}
          </button>
          <button
            type="button"
            class="text-sm font-semibold text-[#CAE4F7] transition hover:text-red-300"
            @click="logout"
          >
            {{ t('common.signOut') }}
          </button>
        </div>
      </div>
    </header>

    <section class="mx-auto flex w-full max-w-5xl flex-col px-4 py-10 sm:px-6">
      <div class="mb-10 flex flex-wrap items-center gap-3">
        <div v-for="(step, index) in visibleSteps" :key="step.id" class="flex items-center gap-3">
          <div
            class="flex h-9 w-9 items-center justify-center rounded-full border text-sm font-semibold"
            :class="
              index + 1 < currentStepperPosition
                ? 'border-[#8AC4ED] bg-[#8AC4ED] text-[#071C2C]'
                : index + 1 === currentStepperPosition
                  ? 'border-[#218CD9] bg-[#218CD9] text-white'
                  : 'border-slate-500 bg-transparent text-slate-300'
            "
          >
            {{ index + 1 }}
          </div>
          <span
            class="text-sm"
            :class="index + 1 <= currentStepperPosition ? 'text-[#CAE4F7]' : 'text-slate-400'"
          >
            {{ t(step.key) }}
          </span>
          <div v-if="index < visibleSteps.length - 1" class="h-px w-8 bg-slate-600 sm:w-12"></div>
        </div>
      </div>

      <div class="rounded-2xl border border-slate-700 bg-[#0B263B] p-6 shadow-xl sm:p-8">
        <h1 class="text-2xl font-bold">{{ t('onboarding.title') }}</h1>

        <p v-if="errorMessage" class="mt-4 rounded-lg border border-red-400/50 bg-red-500/10 px-4 py-2 text-sm text-red-200">
          {{ errorMessage }}
        </p>

        <div v-if="currentStep === 1" class="mt-6 grid gap-4 md:grid-cols-2">
          <p class="md:col-span-2 text-sm text-[#8AC4ED]">{{ t('onboarding.role.title') }}</p>
          <button
            type="button"
            class="rounded-xl border p-5 text-left transition"
            :class="
              selectedRole === 'Student'
                ? 'border-[#218CD9] bg-[#123451]'
                : 'border-slate-600 bg-[#0A2235] hover:border-[#8AC4ED]'
            "
            @click="selectedRole = 'Student'"
          >
            <div class="mb-3 h-8 w-8 text-[#8AC4ED]">
              <svg viewBox="0 0 24 24" fill="currentColor">
                <path d="M12 12a5 5 0 1 0-5-5 5 5 0 0 0 5 5Zm0 2c-4.418 0-8 2.239-8 5v1h16v-1c0-2.761-3.582-5-8-5Z" />
              </svg>
            </div>
            <h2 class="text-lg font-semibold">{{ t('onboarding.role.student') }}</h2>
          </button>

          <button
            type="button"
            class="rounded-xl border p-5 text-left transition"
            :class="
              selectedRole === 'Coordinator'
                ? 'border-[#218CD9] bg-[#123451]'
                : 'border-slate-600 bg-[#0A2235] hover:border-[#8AC4ED]'
            "
            @click="selectedRole = 'Coordinator'"
          >
            <div class="mb-3 h-8 w-8 text-[#8AC4ED]">
              <svg viewBox="0 0 24 24" fill="currentColor">
                <path
                  d="M8 11a4 4 0 1 0-4-4 4 4 0 0 0 4 4Zm8 0a4 4 0 1 0-4-4 4 4 0 0 0 4 4ZM8 13c-3.314 0-6 1.791-6 4v1h8.31A5.892 5.892 0 0 1 10 16.2 5.96 5.96 0 0 1 11.07 13Zm8 0c-3.314 0-6 1.791-6 4v1h12v-1c0-2.209-2.686-4-6-4Z"
                />
              </svg>
            </div>
            <h2 class="text-lg font-semibold">{{ t('onboarding.role.coordinator') }}</h2>
            <p class="mt-2 text-sm text-[#8AC4ED]">{{ t('onboarding.role.coordinatorNote') }}</p>
          </button>
        </div>

        <div v-if="currentStep === 2" class="mt-6 space-y-4">
          <div class="relative">
            <label class="mb-2 block text-sm font-medium">{{ t('onboarding.institution.title') }}</label>
            <input
              v-model="searchQuery"
              type="text"
              class="w-full rounded-lg border border-slate-600 bg-[#071C2C] px-4 py-2 outline-none transition focus:border-[#218CD9]"
              :placeholder="t('onboarding.institution.searchPlaceholder')"
              @focus="showInstitutionDropdown = true"
              @input="onInstitutionSearchInput"
            />

            <div
              v-if="showInstitutionDropdown && !createNewInstitution"
              class="absolute z-10 mt-2 w-full rounded-lg border border-slate-600 bg-[#071C2C] p-2 shadow-lg"
            >
              <button
                v-for="institution in filteredInstitutions"
                :key="institution.id"
                type="button"
                class="block w-full rounded px-3 py-2 text-left text-sm hover:bg-[#123451]"
                @click="selectInstitution(institution)"
              >
                {{ institution.name }} - {{ institution.country }}
              </button>
              <p v-if="filteredInstitutions.length === 0" class="px-3 py-2 text-sm text-slate-300">
                {{ t('onboarding.noResults') }}
              </p>
              <button
                type="button"
                class="mt-2 block w-full rounded border border-dashed border-[#8AC4ED] px-3 py-2 text-left text-sm text-[#8AC4ED] hover:bg-[#123451]"
                @click="enableNewInstitution"
              >
                {{ t('onboarding.institution.addNew') }}
              </button>
            </div>
          </div>

          <div v-if="createNewInstitution" class="grid gap-3 sm:grid-cols-2">
            <input
              v-model="newInstitutionName"
              class="rounded-lg border border-slate-600 bg-[#071C2C] px-4 py-2"
              :placeholder="`${t('onboarding.institution.form.name')} *`"
            />
            <input
              v-model="newInstitutionCountry"
              class="rounded-lg border border-slate-600 bg-[#071C2C] px-4 py-2"
              :placeholder="`${t('onboarding.institution.form.country')} *`"
            />
            <input
              v-model="newInstitutionCity"
              class="rounded-lg border border-slate-600 bg-[#071C2C] px-4 py-2"
              :placeholder="t('onboarding.institution.form.city')"
            />
            <input
              v-model="newInstitutionErasmusCode"
              class="rounded-lg border border-slate-600 bg-[#071C2C] px-4 py-2"
              :placeholder="t('onboarding.institution.form.erasmusCode')"
            />
            <input
              v-model="newInstitutionIscedCode"
              class="rounded-lg border border-slate-600 bg-[#071C2C] px-4 py-2"
              :placeholder="`${t('onboarding.institution.form.iscedCode')} *`"
            />
            <input
              v-model="newInstitutionProgramName"
              class="rounded-lg border border-slate-600 bg-[#071C2C] px-4 py-2"
              :placeholder="`${t('onboarding.institution.form.programName')} *`"
            />
            <input
              v-model="newInstitutionProfileName"
              class="rounded-lg border border-slate-600 bg-[#071C2C] px-4 py-2 sm:col-span-2"
              :placeholder="`${t('onboarding.institution.form.profileName')} *`"
            />
          </div>
        </div>

        <div v-if="currentStep === 3 && isStudent" class="mt-6 space-y-4">
          <p class="text-sm text-[#8AC4ED]">{{ t('onboarding.program.title') }}</p>
          <div>
            <label class="mb-2 block text-sm font-medium">{{ t('onboarding.confirm.program') }}</label>
            <select
              v-model="selectedProgramId"
              class="w-full rounded-lg border border-slate-600 bg-[#071C2C] px-4 py-2"
              @change="onProgramChanged"
            >
              <option :value="null">{{ t('onboarding.program.selectProgram') }}</option>
              <option v-for="program in programs" :key="program.id" :value="program.id">{{ program.name }}</option>
            </select>
          </div>

          <div>
            <label class="mb-2 block text-sm font-medium">{{ t('onboarding.confirm.profile') }}</label>
            <select
              v-model="selectedProfileId"
              class="w-full rounded-lg border border-slate-600 bg-[#071C2C] px-4 py-2"
              :disabled="createNewProfile"
            >
              <option :value="null">{{ t('onboarding.program.selectProfile') }}</option>
              <option v-for="profile in profiles" :key="profile.id" :value="profile.id">{{ profile.name }}</option>
            </select>
          </div>

          <button
            type="button"
            class="rounded-lg border border-dashed border-[#8AC4ED] px-4 py-2 text-sm text-[#8AC4ED] hover:bg-[#123451]"
            @click="createNewProfile = !createNewProfile"
          >
            {{ t('onboarding.program.addNewProfile') }}
          </button>

          <div v-if="createNewProfile">
            <input
              v-model="newProfileName"
              class="w-full rounded-lg border border-slate-600 bg-[#071C2C] px-4 py-2"
              :placeholder="`${t('onboarding.program.profileNamePlaceholder')} *`"
            />
          </div>
        </div>

        <div v-if="currentStep === 4" class="mt-6 rounded-xl border border-slate-600 bg-[#071C2C] p-5">
          <h2 class="text-lg font-semibold">{{ t('onboarding.confirm.title') }}</h2>
          <div class="mt-4 space-y-2 text-sm text-[#CAE4F7]">
            <p><span class="text-[#8AC4ED]">{{ t('onboarding.confirm.role') }}:</span> {{ selectedRoleLabel }}</p>
            <p v-if="createNewInstitution">
              <span class="text-[#8AC4ED]">{{ t('onboarding.confirm.institution') }}:</span>
              {{ newInstitutionName }} ({{ newInstitutionCountry }})
            </p>
            <p v-else>
              <span class="text-[#8AC4ED]">{{ t('onboarding.confirm.institution') }}:</span>
              {{ selectedInstitution?.name ?? t('common.na') }} ({{ selectedInstitution?.country ?? t('common.na') }})
            </p>
            <p v-if="createNewInstitution && isStudent">
              <span class="text-[#8AC4ED]">{{ t('onboarding.confirm.program') }} / {{ t('onboarding.confirm.profile') }}:</span>
              {{ newInstitutionProgramName }} / {{ newInstitutionProfileName }}
            </p>
            <p v-else-if="isStudent && createNewProfile">
              <span class="text-[#8AC4ED]">{{ t('onboarding.confirm.program') }} / {{ t('onboarding.confirm.profile') }}:</span>
              {{ selectedProgram?.name ?? t('common.na') }} / {{ newProfileName || t('common.na') }}
            </p>
            <p v-else-if="isStudent">
              <span class="text-[#8AC4ED]">{{ t('onboarding.confirm.program') }} / {{ t('onboarding.confirm.profile') }}:</span>
              {{ selectedProgram?.name ?? t('common.na') }} / {{ selectedProfile?.name ?? t('common.na') }}
            </p>
          </div>
        </div>

        <div class="mt-8 flex items-center justify-between">
          <button
            type="button"
            class="rounded-lg border border-slate-500 px-4 py-2 text-sm text-slate-200 transition hover:bg-slate-700/40 disabled:opacity-40"
            :disabled="currentStep === 1 || isSubmitting"
            @click="goBack"
          >
            {{ t('onboarding.back') }}
          </button>

          <button
            v-if="currentStep < 4"
            type="button"
            class="rounded-lg bg-[#218CD9] px-5 py-2 text-sm font-semibold text-white transition hover:bg-[#8AC4ED] hover:text-[#071C2C]"
            @click="goNext"
          >
            {{ t('onboarding.next') }}
          </button>

          <button
            v-else
            type="button"
            class="rounded-lg bg-[#218CD9] px-5 py-2 text-sm font-semibold text-white transition hover:bg-[#8AC4ED] hover:text-[#071C2C] disabled:opacity-60"
            :disabled="isSubmitting"
            @click="finishOnboarding"
          >
            {{ isSubmitting ? t('common.loading') : t('onboarding.confirm.submit') }}
          </button>
        </div>
      </div>
    </section>
  </main>
</template>
