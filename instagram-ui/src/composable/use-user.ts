import { readonly, ref } from "vue";
import { ACCESS_TOKEN, REFRESH_TOKEN } from '@/api/constants/api-constants';

export interface IUser {
  userName: string,
  fullName?: string,
  avatar?: string, 
  email?: string,
  accessToken?: string,
  refreshToken?: string,
  isAuthenticated?: boolean,
  id: string,
  exp?: number,
  roles?: string[],
  age?: number,
  subtile?: string
}


export interface Token {
  accessToken: string,
  refreshToken: string,
}


const unAuthenticatedUser: Readonly<IUser> = {
  userName: '',
  email: '',
  accessToken: '',
  refreshToken: '',
  isAuthenticated: false,
  id: ''
}

const _currentUser = ref<IUser>(loadUserFromStorage());
export const getUser = () => readonly(_currentUser.value);
export const setUser = (user: IUser) => _currentUser.value = user;
export const setUnAuthenticatedUser = () => {
  _currentUser.value = unAuthenticatedUser;
  deleteToken();
}
export function updateUser<T extends keyof IUser>(property: T, value: IUser[T]) {
  _currentUser.value[property] = value;
  if (property == 'accessToken') {
    localStorage.setItem(ACCESS_TOKEN, (value as string));
  }
  if (property == 'refreshToken') {
    localStorage.setItem(REFRESH_TOKEN, (value as string));
  }
}

function deleteToken() {
  localStorage.removeItem(ACCESS_TOKEN);
  localStorage.removeItem(REFRESH_TOKEN);
}

function getToken(): Token | null {
  const accessToken = localStorage.getItem(ACCESS_TOKEN);
  const refreshToken = localStorage.getItem(REFRESH_TOKEN);
  if (accessToken != null && refreshToken)
    return { accessToken, refreshToken };
  else
    return null;
}

function jwtDecode(accessToken: string) {
  const payload = JSON.parse(window.atob(accessToken.split('.')[1]));
  return {
    userName: payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] as string,
    id: payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] as string,
    exp: payload['exp'] as number,
    isAuthenticated: true,
  }
}

function loadUserFromStorage() {
  const token = getToken();
  try {
    if (token != null) {
      const user: IUser = {
        ...jwtDecode(token.accessToken),
        accessToken: token.accessToken,
        refreshToken: token.refreshToken,
      }
      return user
    }
  } catch (error) {
    
  }
  console.log('loadUserFromStorage', token);
  return unAuthenticatedUser;
}

export const logout = () => {
  setUnAuthenticatedUser()
}