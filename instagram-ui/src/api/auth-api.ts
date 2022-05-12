import { API_URL_BASE } from '@/api/constants/api-constants';
import axios, { AxiosRequestConfig } from 'axios';
import { useApi } from './composable/use-api';
import { Token, setUnAuthenticatedUser, updateUser, getUser } from '../composable/use-user';


const URLS = {
  refreshToken: 'token/refresh',
  login: 'authentication/login',
  signup: 'authentication',
  redirectAfterLogin: 'http://instagram.test',
}

// Default config for the axios instance
const axiosParams: AxiosRequestConfig = {
  baseURL: API_URL_BASE,
};

// Create axios instance with default params
const axiosInstance = axios.create(axiosParams);
axiosInstance.interceptors.request.use(config => {
  console.log('auh-api.ts request: ' + config.url);
  return config;
});

export async function initialAuth() {
}

export async function refreshToken() {
  const currentUser = getUser();
  if (currentUser.isAuthenticated)
    try {
      const token: Token = (await axiosInstance.post<Token>(URLS.refreshToken,
        {
          accessToken: currentUser.accessToken,
          refreshToken: currentUser.refreshToken,
        })).data;
      updateUser('accessToken', token.accessToken);
      updateUser('refreshToken', token.refreshToken);
      console.log('%c refreshToken was success', 'color: #0000FF; font-weight: 500;');
    } catch (error) {
      // setUnAuthenticatedUser();
      console.log('refreshToken error', error);
    }
}

export function useLoginByUsernameAndPassword() {
  const { exec, errors, statusError, statusIdle, statusPending, statusSuccess, status } = useApi(async (username: string, password: string) => {
    try {
      const token = (await axiosInstance.post<Token>(URLS.login, { username, password })).data;
      updateUser('accessToken', token.accessToken)
      updateUser('refreshToken', token.refreshToken)
      setTimeout(() => {
        document.location = URLS.redirectAfterLogin
      }, 1000);
      console.log('login success!');

    } catch (e) {
      console.log('login Fail');
      throw e;
    }
  });

  return {
    loginByUsernameAndPassword: exec,
    loginIdle: statusIdle,
    loginPending: statusPending,
    loginSuccess: statusSuccess,
    loginFail: statusError,
    errors,
    loginStatus: status
  }
}

export function useSignup() {
  const { exec, status, errors } = useApi(async (userName: string, email: string, password: string) => {
    try {
      const response = await axiosInstance.post(URLS.signup, { userName, email, password })
      if (response.status === 201) {
        console.log('signup success!...........');
      }
    } catch (e) {
      console.log('signup error!............');
      throw e;
    }
  })

  return {
    signup: exec,
    status,
    errors,
  }
}

export function logout() {
  setUnAuthenticatedUser();
}

export function isAccessTokenEpx(exp: number) {
  return (exp * 1000 < Date.now())
}
