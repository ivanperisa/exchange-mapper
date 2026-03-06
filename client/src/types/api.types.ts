export interface RequestInfo {
  method: string
  path: string
  timestamp: string
}

export interface ErrorDetails {
  code: string
  message: string
}

export interface BaseResponse<T = null> {
  success: boolean
  data: T | null
  error: ErrorDetails | null
  request: RequestInfo | null
}
