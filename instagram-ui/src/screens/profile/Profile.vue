<script setup lang="ts">
import LayoutBase from '@/layouts/LayoutBase.vue';
import { inject, onMounted,onUnmounted,watch} from 'vue';
import AppBar from './AppBar.vue';
import IconGrid from '@/icons/IconGrid.vue';
import IconFeed from '@/icons/IconFeed.vue';
import IconSave from '@/icons/IconSave.vue';
import IconTag from '../../icons/IconTag.vue';
import { PageUrlBuilder } from "@/helpers/pageUrlBuilder";
import { registerFetchUserProfile } from '@/composable/use-profile-manager';
import { getUser } from '@/composable/use-user';
import CircleImage from '@/components/base/CircleImage.vue';
import FollowButton from '@/components/FollowButton.vue';
import IconSpinner from '@/icons/IconSpinner.vue';
import BaseIconPending from '@/components/base/BaseIconPending.vue';
import { onBeforeRouteUpdate } from 'vue-router';


onMounted(() => {
  console.log('%c onMounted Profile', 'font-weight: 700; color:yellow');
})

const props = defineProps({
  userName: String
})


const { status, canViewSavedPost, userName: _userName, setUserName, userProfile } = registerFetchUserProfile();


watch(() => props.userName, () => {
  setUserName(props.userName);
})

onMounted(() => {
  setUserName(props.userName);
})

</script>

<template>
  <LayoutBase>
    <template #appBar>
      <AppBar />
    </template>
    <template #body>
      <BaseIconPending :pending="status.Pending" :delay="500">
        <template #pending>
          <div class="flex items-center h-full justify-center">
            <IconSpinner :size="100"/>
          </div>
        </template>
        <div v-if="userProfile.data">
          <div class="flex border-b border-b-gray-300 p-4">
            <div class="flex flex-col items-center">
              <CircleImage :size="80" :src="userProfile.data.avatarHome || '/icons/none-profile.jpg'" />
              <span class="text-black font-semibold mt-6">{{ userProfile.data.title }}</span>
            </div>
            <div class="grow shrink basis-0 min-w-0 ml-10 max-w-[250px]">
              <div class="text-3xl font-light overflow-hidden whitespace-nowrap text-ellipsis">{{ _userName }}</div>
              <FollowButton :userId="userProfile.data.id" :avatar="userProfile.data.avatar"
                v-if="userProfile && userProfile.data.id && userProfile.data.id !== getUser().id" variant="text-btn"
                :userName="userProfile.data.userName" :isFollowing="userProfile.followStatus == 'following'" />
              <button class="mt-2 w-full border-2 border-gray-200 py-1 rounded-sm font-semibold text-sm">
                Edit Profile</button>
            </div>
          </div>
          <div class="flex justify-around border-b border-b-gray-300 py-3">
            <div class="flex flex-col items-center">
              <span class="font-bold text-sm">{{ userProfile?.data?.posts }}</span>
              <span class="text-gray-500 text-sm">posts</span>
            </div>
            <router-link :to="PageUrlBuilder['/:username/followers'](_userName)" class="flex flex-col items-center">
              <span class="font-bold text-sm">{{ userProfile.data?.followers }}</span>
              <span class="text-gray-500 text-sm">followers</span>
            </router-link>
            <router-link :to="PageUrlBuilder['/:username/following'](_userName)" class="flex flex-col items-center">
              <span class="font-bold text-sm">{{ userProfile.data?.following }}</span>
              <span class="text-gray-500 text-sm">following</span>
            </router-link>
          </div>
          <div class="flex justify-around border-b border-b-gray-300 py-3">
            <router-link :to="PageUrlBuilder['/:username/'](_userName)">
              <IconGrid />
            </router-link>
            <router-link :to="PageUrlBuilder['/:username/feed'](_userName)">
              <IconFeed />
            </router-link>

            <router-link v-if="canViewSavedPost" :to="PageUrlBuilder['/:username/saved'](_userName)">
              <IconSave />
            </router-link>
            <router-link :to="PageUrlBuilder['/:username/tagged'](_userName)">
              <IconTag />
            </router-link>
          </div>
          <RouterView v-slot="{ Component }">
            <KeepAlive>
              <component :is="Component"></component>
            </KeepAlive>
          </RouterView>
        </div>
      </BaseIconPending>
    </template>
  </LayoutBase>
</template>
