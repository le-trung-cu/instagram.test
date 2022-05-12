import { ref } from "vue";

export type LoginScreenType = 'LoginScreen1' | 'LoginScreen2';

export function useLoginManager(){
  const loginScreenShow = ref<LoginScreenType>('LoginScreen1');
  return {
    loginScreenShow,
    setLoginScreen: (screen: LoginScreenType) => {
      loginScreenShow.value = screen
    }
  }
}