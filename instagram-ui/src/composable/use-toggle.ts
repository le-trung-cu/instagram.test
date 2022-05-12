import { ref } from "vue"

export const useBoolean = (value = false) => {
  const state = ref(value);
  return {
    state,
    toggle: () => state.value = !state.value,
    setFalse: () => state.value = false,
    setTrue: () => state.value = true,
    setBoolean: (value: boolean) => state.value = value
  }
}