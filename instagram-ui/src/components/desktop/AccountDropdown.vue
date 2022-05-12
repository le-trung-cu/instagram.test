<script setup lang="ts">
import CircleImage from '../base/CircleImage.vue';
import IconButtonDropdown from '../base/IconButtonDropdown.vue';
import IconProfile from '@/icons/IconProfile.vue';
import IconSave from '@/icons/IconSave.vue';
import IconSetting from '@/icons/IconSetting.vue';
import IconSwitch from '@/icons/IconSwitch.vue';
import { PageUrlBuilder } from "@/helpers/pageUrlBuilder";
import { getUser, logout } from '@/composable/use-user';

const user = getUser();
</script>
<template>
  <IconButtonDropdown :width="230" :offset="30" position="left">
    <CircleImage :src="user.avatar || '/icons/none-profile.jpg'" />
    <template #dropdown>
      <ul>
        <li >
          <router-link :to="PageUrlBuilder['/:username/'](user.userName)" class="flex items-center px-4 h-9 cursor-pointer hover:bg-gray-100 transition-colors text-sm font-light">
            <IconProfile class="mr-2" />
            <span>Profile</span>
          </router-link>
        </li>
        <li >
          <router-link :to="PageUrlBuilder['saved']" class="flex items-center px-4 h-9 cursor-pointer hover:bg-gray-100 transition-colors text-sm font-light">
            <IconSave :size="16" class="mr-2" />
            <span>Saved</span>
          </router-link>
        </li>
        <li class="flex items-center px-4 h-9 cursor-pointer hover:bg-gray-100 transition-colors text-sm font-light">
          <IconSetting :size="16" class="mr-2" />
          <span>Settings</span>
        </li>
        <li class="flex items-center px-4 h-9 cursor-pointer hover:bg-gray-100 transition-colors text-sm font-light">
          <IconSwitch class="mr-2" />
          <span>Profile</span>
        </li>
        <li
          class="border-t-2 border-gray-300 flex items-center px-4 h-10 cursor-pointer hover:bg-gray-100 transition-colors text-sm font-light">
          <span @click="logout(); $router.push(PageUrlBuilder['/login']())">Log Out</span>
        </li>
      </ul>
    </template>
  </IconButtonDropdown>
</template>