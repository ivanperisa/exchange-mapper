export interface InstitutionDto {
  id: string
  name: string
  country: string
  city: string | null
  erasmusCode: string | null
}

export interface StudyProgramDto {
  id: string
  name: string
  iscedCode: string
}

export interface StudyProfileDto {
  id: string
  name: string
}
