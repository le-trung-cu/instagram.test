<script setup lang="ts">
import IconNoneProfile from '@/icons/IconNoneProfile.vue';
import UserTile from '@/components/UserTile.vue';
import { onMounted } from 'vue';
import { getUser } from '@/composable/use-user';
import { PageUrlBuilder } from "@/helpers/pageUrlBuilder";
import { useFetchSuggestionFriendsOnHomePage } from '@/composable/use-relationship-manager';
import FollowButton from '@/components/FollowButton.vue';

const currentUser = getUser();
const { suggestionFriends, fetchSuggestionFriends, status } = useFetchSuggestionFriendsOnHomePage();
onMounted(async () => {
  await fetchSuggestionFriends({ pageNumber: 1, pageSize: 7 });
});

</script>

<template>
  <aside class="space-y-2">
    <div class="flex items-center mb-8">
      <IconNoneProfile :size="60" class="mr-5" />
      <div class="flex flex-col">
        <router-link to class="font-semibold text-sm leading-tight">{{ currentUser.userName }}</router-link>
        <span class="leading-tight text-xs font-normal text-gray-600 mt-0.5">{{ currentUser.fullName }}</span>
      </div>
      <router-link :to="PageUrlBuilder['/login']()" class="text-blue-500 font-medium text-xs ml-auto">Switch
      </router-link>
    </div>
    <div class="flex justify-between">
      <h4 class="font-semibold tracking-wider text-gray-400 text-sm">Suggestions For You</h4>
      <router-link :to="PageUrlBuilder['/explore/people']()" class="font-medium text-xs">See All</router-link>
    </div>
    <div class="space-y-2">
      <UserTile v-if="status.Success" v-for="{ data: user, followStatus } in suggestionFriends" :size="50"
        :avatar="user!.avatar" :userName="user!.userName" :subtile="user!.subtile">
        <template #trailing>
          <FollowButton :isFollowing="followStatus === 'following'" :avatar="user!.avatar!" variant="text-btn"
            :userId="user!.id" :userName="user!.userName" />
        </template>
      </UserTile>
      <div v-else class="p-4 max-w-sm w-full mx-auto space-y-2"
        v-for="i in 4">
        <div class="animate-pulse flex space-x-4">
          <div class="rounded-full bg-slate-200 h-10 w-10"></div>
          <div class="flex-1 space-y-6 py-1">
            <div class="h-2 bg-slate-200 rounded"></div>
            <div class="space-y-3">
              <div class="grid grid-cols-3 gap-4">
                <div class="h-2 bg-slate-200 rounded col-span-2"></div>
                <div class="h-2 bg-slate-200 rounded col-span-1"></div>
              </div>
              <div class="h-2 bg-slate-200 rounded"></div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </aside>
</template>