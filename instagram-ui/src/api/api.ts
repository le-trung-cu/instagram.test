import axios, { Axios, AxiosError, AxiosInstance, AxiosRequestConfig, AxiosResponse } from 'axios';
import { refreshToken } from "./auth-api";
import { API_URL_BASE } from './constants/api-constants';
import { getUser } from "@/composable/use-user";

const user = getUser();

// Default config for the axios instance
const axiosParams: AxiosRequestConfig = {
  baseURL: API_URL_BASE,
};

// Create axios instance with default params
const axiosInstance = axios.create(axiosParams);

axiosInstance.interceptors.request.use(async config => {
  console.log('%c api ' + config.method + ':%c ' + config.url, 'color: blue; font-weight: 800;', 'color: red; font-weight: 800;');

  if (user.isAuthenticated && (user.exp! * 1000 < Date.now())) {
    await refreshToken();
  }
  config.headers = {
    ...config.headers,
    Authorization: user.accessToken ? `Bearer ${user.accessToken}` : ''
  }
  return config;
})

axiosInstance.interceptors.response.use(response => {
  return response;
}, error => {
  console.log('error response api.ts: ', error.response);
  return Promise.reject(error);
})
// Main api function
// axiosInstance.interceptors.response.use(response => {
//   // Any status code that lie within the range of 2xx cause this function to trigger
//   return response;
// }, async (error) => {
//   // Any status codes that falls outside the range of 2xx cause this function to trigger
//   console.log('trigger interception! error status', error.response.status);
//   // if(error.response.status === 401 && currentUser.value.isAuthenticated){
//   console.log('access token was expire');
//   console.log('refresh access token');
//   await refreshToken();
//   if (currentUser.value.isAuthenticated) {
//     console.log('refresh access token successful')
//   } else {
//     console.log('refresh access token fail')
//   }
//   // }
//   return Promise.reject(error);
// });

export const didAbort = (error: any) => axios.isCancel(error);
const getCancelSource = () => axios.CancelToken.source();

const api = (axios: AxiosInstance) => {

  function withAbort(fn: Function) {
    return async (...args: any[]): Promise<AxiosResponse> => {
      const originalConfig = args[args.length - 1]
      let { abort, ...config } = originalConfig
      if (typeof abort === 'function') {
        const { cancel, token } = getCancelSource()
        config.cancelToken = token
        abort(cancel);
      }
      try {
        return await fn(...args.slice(0, args.length - 1), config)
      } catch (error) {
        didAbort(error) && ((error as any).aborted = true)
        throw error
      }
    }
  }

  // Wrapper functions around axios
  return {
    get: (url: string, config: AxiosRequestConfig = {}) => withAbort(axios.get)(url, config),
    post: (url: string, body?: any, config: AxiosRequestConfig = {}) => withAbort(axios.post)(url, body, config),
    put: (url: string, body?: any, config: AxiosRequestConfig = {}) => withAbort(axios.put)(url, body, config),
    patch: (url: string, body?: any, config: AxiosRequestConfig = {}) => withAbort(axios.patch)(url, body, config),
    delete: (url: string, config: AxiosRequestConfig = {}) => axios.delete(url, config),
  };
};

export const abortAble = (fn: Function) => {
  // Create cancel token and cancel method
  const { cancel, token } = getCancelSource()
  // Return the cancel method and the wrapped function with a cancel token
  return {
    abort: cancel,
    fn: (...args: any[]) => {
      // If the last argument is not an object then throw
      if (typeof args[args.length - 1] !== "object") {
        throw new Error("The last argument must be a config object!");
      }
      // Add the cancel token to the last argument passed
      // The last argument passed should always be a config object
      args[args.length - 1] = {
        ...args[args.length - 1],
        cancelToken: token,
      };
      return fn(...args);
    },
  };
};

// Initialize the api function and pass axiosInstance to it
export default api(axiosInstance);
