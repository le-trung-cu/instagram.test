<script setup lang="ts">
import { LoginScreenType } from '@/composable/types';
import { ref } from 'vue';
import LoginScreen1 from './LoginScreen1.vue';
import LoginScreen2 from './LoginScreen2.vue';

const loginScreen = ref<LoginScreenType>('LoginScreen1');
function setScreen(screen: LoginScreenType) {
  console.log('screen');
  
  loginScreen.value = screen;
}
</script>

<template>
  <LoginScreen1 v-if="loginScreen=='LoginScreen1'" @set-login-screen="setScreen"/>
  <LoginScreen2 v-if="loginScreen == 'LoginScreen2'"/>
</template>