<template>
  <div class="fixed inset-0 bg-white flex justify-center items-center">
    <div class="text-center space-y-3 flex flex-col items-center w-4/5">
      <IconInstagram :height="50" />
      <p class="text-sm text-gray-500">Sign up to see photo and videos from your friends.</p>
      <a
        class="btn-bg-blue"
        href="http://"
        target="_blank"
        rel="noopener noreferrer"
      >Get the Instagram app</a>
      <div class="pt-2">
        <a @click="$emit('setLoginScreen', 'LoginScreen2')" class="text-btn">Log In</a>
        <span class="mx-2">or</span>
        <router-link class="text-btn" to="/account/signup">Sign Up</router-link>
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
import IconInstagram from '@/icons/IconInstagram.vue';
import { LoginScreenType } from "@/composable/types";
import { defineEmits } from 'vue';
defineEmits<{
  (event: 'setLoginScreen', screen: LoginScreenType): void
}>();

</script>