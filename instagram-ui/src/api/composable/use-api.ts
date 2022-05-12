import { Awaitable } from 'vitest';
import { ref, computed, Ref, UnwrapRef } from 'vue'
import { apiStatus } from '../constants/api-constants'
const { Idle, Success, Pending, Error } = apiStatus

export interface IErrorResponse {
  succeeded?: boolean,
  errors?: Array<{
    code: string,
    description: string,
  }>
}
export const useApi = function <Fn extends (...args: any[]) => Awaitable<any>, Result extends Awaited<ReturnType<Fn>>>(fn: Fn, options?: { initialData: Result }) {
  // Reactive values to store data and API status
  const data: Ref<Result| undefined> = ref<any>(options?.initialData ?? undefined);
  const status = ref(Idle)
  const errors = ref<IErrorResponse['errors']|null>(null)
  // Initialize the api request
  const exec = async (...args: Parameters<Fn>) => {
    try {
      // Clear current value
      data.value = undefined
      errors.value = null
      // API request starts
      status.value = Pending
      let value = await fn(...args)
      status.value = Success
      if (value) {
        data.value = value;
      }
      // Done!
    } catch (e) {
      console.log(e);
      
      // Oops, there was an error
      // errors.value = (e as any).response.data.errors;
      console.log('errors ', JSON.stringify(errors.value));
      status.value = Error
    }
  }
  return {
    data: data,
    errors,
    exec,
    status: computed(() => {
      return {
        Idle: status.value === Idle,
        Pending: status.value === Pending,
        Success: status.value === Success,
        Error: status.value === Error,
      }
    }),
    statusIdle: computed(() => status.value === Idle),
    statusPending: computed(() => status.value === Pending),
    statusSuccess: computed(() => status.value === Success),
    statusError: computed(() => status.value === Error),
  }
}