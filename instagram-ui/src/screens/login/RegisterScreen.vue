
<script setup lang="ts">
import IconInstagram from '@/icons/IconInstagram.vue';
import BaseInput from '@/components/base/BaseInput.vue';
import { useField, useForm } from 'vee-validate';
import * as yup from "yup";
import { camelCaseToWords } from "@/helpers";
import { useSignup } from '@/api/auth-api';
import { useRouter } from 'vue-router';
import { PageUrlBuilder } from '@/helpers/pageUrlBuilder';

const router = useRouter()
const { values, errors, handleSubmit } = useForm({
  validationSchema: yup.object({
    username: yup.string().required(),
    email: yup.string().required(),
    password: yup.string().required(),
  })
})
const handleChangeUserName = useField('username').handleChange;
const handleChangeEmail = useField('email').handleChange;
const handleChangePassword = useField('password').handleChange;
const { signup, status, errors: errorsResponse } = useSignup();
const submit = handleSubmit(async (values) => {
  await signup(values.username!, values.email!, values.password!);
  console.log(status.value);
  console.log(errorsResponse.value);
  if(status.value.Success){
    router.push(PageUrlBuilder['/login']())
  }
  errorsResponse.value?.map(e => {

  })
})
</script>


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
      <form class="basis-full w-full space-y-2" @submit="submit">
        <BaseInput
          name="username"
          type="text"
          label="User name"
          placeholder="User name"
          :modelValue="values.username"
          :errorMessage="errors.email"
          @change="handleChangeUserName"
        />
        <BaseInput
          name="email"
          type="email"
          label="Email"
          placeholder="Email"
          :modelValue="values.email"
          :errorMessage="errors.email"
          @change="handleChangeEmail"
        />
        <BaseInput
          name="password"
          type="password"
          label="Password"
          placeholder="Password"
          :modelValue="values.password"
          :errorMessage="errors.password"
          @change="handleChangePassword"
        />
        <button class="button-bg-blue opacity-60">Sign Up</button>
      </form>
      <ol v-if="errorsResponse && errorsResponse.length>0" class="bg-yellow-100 p-4 w-full">
      <li  v-for="error in errorsResponse" >
        <span class="font-medium text-sm text-red-500 mr-4">{{camelCaseToWords(error.code)}}:</span>
        <span class="font-normal text-sm text-red-400">{{error.description}}</span>
      </li>
    </ol>
    </div>

  </div>
</template>