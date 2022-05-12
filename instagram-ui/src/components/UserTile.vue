<script setup lang="ts">
import CircleImage from './base/CircleImage.vue';
import { PageUrlBuilder } from "@/helpers/pageUrlBuilder";
defineProps({
  avatar: String,
  userName: String,
  fullName: String,
  subtile: String,
  size: Number
})
</script>

<template>
  <div class="flex items-center">
    <router-link class="flex items-center" :to="PageUrlBuilder['/:username/'](userName)">
      <CircleImage v-if="avatar" :size="size" class="mr-2" :src="avatar" />
      <div class="flex flex-col">
        <span class="font-semibold text-sm leading-tight">{{ userName }} <slot name="additional"></slot> </span>
        <span v-if="fullName" class="leading-tight text-xs font-normal text-gray-600 mt-0.5">{{ fullName }}</span>
        <span class="leading-tight text-xs font-normal text-gray-600 mt-0.5">{{ subtile }}</span>
      </div>
    </router-link>
    <div class="ml-auto">
      <slot name="trailing">
      </slot>
    </div>
  </div>
</template>