import { api } from '@/services/api'

export interface MakeCoordinatorRequest {
  userId: string
}

export const adminService = {
  makeCoordinator(request: MakeCoordinatorRequest) {
    return api.post('/admin/make-coordinator', request)
  }
}
