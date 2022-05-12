<script setup lang="ts">
import { IconHome, IconExplore, IconHeart, IconNoneProfile, IconNewPost } from '@/icons';
import { PageUrlBuilder } from "@/helpers/pageUrlBuilder";
import { getUser } from '@/composable/use-user';
import { hasNewNotification, setHasNewNotification } from '@/composable/use-notification-manger';
const userName = getUser().userName;

</script>

<template>
  <div id="bottomNavigationBar" class="bg-white border-t border-gray-300 flex justify-around items-center">
    <router-link to="/" class-activity="text-red">
      <IconHome />
    </router-link>
    <router-link to="/explore">
      <IconExplore />
    </router-link>
    <IconNewPost />
    <router-link to="/accounts/activity"><span class="flex flex-col items-center relative">
        <IconHeart @click="setHasNewNotification(false)" />
        <span v-if="hasNewNotification" class="block w-2 h-2 bg-red-400 rounded-full absolute -bottom-2"></span>
      </span></router-link>
    <router-link :to="PageUrlBuilder['/:username/'](userName)">
      <IconNoneProfile :size="24" />
    </router-link>
  </div>
</template>

