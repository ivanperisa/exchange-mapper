export const authService = {
  login: () => {
    window.location.href = `${import.meta.env.VITE_API_URL}/auth/login?returnUrl=/home`
  },
  logout: () => {
    window.location.href = `${import.meta.env.VITE_API_URL}/auth/logout`
  }
}
