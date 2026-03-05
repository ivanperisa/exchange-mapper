import { UserManager, WebStorageStateStore, type User } from 'oidc-client-ts'

const userManager = new UserManager({
  authority: 'https://accounts.google.com',
  client_id: import.meta.env.VITE_GOOGLE_CLIENT_ID,
  client_secret: import.meta.env.VITE_GOOGLE_CLIENT_SECRET,
  redirect_uri: `${window.location.origin}/callback`,
  post_logout_redirect_uri: `${window.location.origin}/logged-out`,
  response_type: 'code',
  scope: 'openid email profile',
  userStore: new WebStorageStateStore({ store: localStorage })
})

export const authService = {
  login: (returnTo: string = '/') => userManager.signinRedirect({ state: { returnTo } }),
  logout: async () => {
    await userManager.removeUser()
    window.location.assign('/logged-out')
  },
  handleCallback: () => userManager.signinRedirectCallback(),
  getUser: (): Promise<User | null> => userManager.getUser(),
  isLoggedIn: async () => {
    const user = await userManager.getUser()
    return !!user && !user.expired
  }
}
