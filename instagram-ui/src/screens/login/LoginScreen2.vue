<template>
  <div class="fixed inset-0 bg-white flex justify-center items-center">
    <div class="flex flex-col items-center w-4/5">
      <IconInstagram :height="50" />
      <a
        class="button-bg-blue mt-9"
        href="http://"
        target="_blank"
        rel="noopener noreferrer"
      >Continue with Facebook</a>
      <div class="flex w-full justify-between items-center my-5">
        <span class="w-2/5 border-b border-gray-300"></span>
        <span class="text-gray-600 text-sm">OR</span>
        <span class="w-2/5 border-b border-gray-300"></span>
      </div>
      <form @submit.prevent="submit" class="basis-full w-full space-y-2">
        <BaseInput name="email" type="text" label="Email" placeholder="Email" 
        :modelValue="values.email"
        :errorMessage="errors.email"
        @change="handleChangeEmail"/>
        <BaseInput name="password" type="password" label="Password" placeholder="Password" 
        :modelValue="values.password"
        :errorMessage="errors.password"
        @change="handleChangePassword"/>
        <div class="text-right w-full mt-3">
          <router-link to="/">
            <span class="text-blue-400 text-sm">Forgot password?</span>
          </router-link>
        </div>
        <button class="button-bg-blue" :class="[errors.email || errors.password? 'opacity-60':'']"> <IconSpinner v-if="loginStatus.Pending"/>Log in</button>
        <p v-if="loginStatus.Error">Login Fail Error: {{loginErrorMessage}}</p>
        <p v-if="loginStatus.Success">Login Success! waiting few seconds...</p>
        <div class="pt-2 text-center">
          <span class="text-sm text-gray-500">Don't have an account?</span>
          <router-link class="text-button" to="/account/signup">Sign Up</router-link>
        </div>
      </form>
    </div>
  </div>
</template>
<script setup lang="ts">
import IconInstagram from '@/icons/IconInstagram.vue';
import BaseInput from '@/components/base/BaseInput.vue';
import { useForm, useField } from 'vee-validate';
import * as yup from 'yup';
import { useLoginByUsernameAndPassword } from '@/api/auth-api';
import IconSpinner from '../../icons/IconSpinner.vue';
const { values, errors, handleSubmit,  } = useForm({
  validationSchema: yup.object({
    email: yup.string().required(),//.email().max(120),
    password: yup.string().required(),//.min(6).max(120)
  })
})

const handleChangeEmail = useField('email').handleChange;
const handleChangePassword = useField('password').handleChange;

const {loginStatus, loginErrorMessage, loginByUsernameAndPassword} = useLoginByUsernameAndPassword();

const submit = handleSubmit(values => {
  loginByUsernameAndPassword(values.email!, values.password!);
})
</script>
