export interface ProblemDetails {
  type?: string
  title: string
  status: number
  detail: string
  extensions?: {
    code?: string
    errors?: Array<{ code: string; description: string }>
  }
}
