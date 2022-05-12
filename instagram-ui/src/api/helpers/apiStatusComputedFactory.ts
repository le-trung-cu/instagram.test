// import { upperFirst } from 'lodash-es'
import { apiStatus } from '../constants/api-constants'
export const apiStatusComputedFactory = (reactivePropertyKeys: string | string[]) => {
  /**
  * Object to store computed getters for
  * different API statuses
  */
  let computed: any = {};
  // If the argument passed is an array then assign it,
  // otherwise, wrap it in an array
  const properties = Array.isArray(reactivePropertyKeys)
    ? reactivePropertyKeys
    : [reactivePropertyKeys];

  for (const reactivePropertyKey of properties) {
    for (const [statusKey, statusValue] of Object.entries(apiStatus)) {
      //Add a computed property
      computed[`${reactivePropertyKey}${statusKey}`] = function () {
        return this[reactivePropertyKey] === statusValue;
      };
    }
  }
  return computed;
};