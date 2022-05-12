<script setup lang="ts">
import { registerFetchSuggestionFriendsOnPeoplePage } from '@/composable/use-relationship-manager';
import { getUser } from '@/composable/use-user';
import LayoutBase from '@/layouts/LayoutBase.vue';
import { onMounted } from 'vue';
import AppBar from './AppBar.vue';
import UserTile from '@/components/UserTile.vue';
import FollowButton from '@/components/FollowButton.vue';
import BaseIconPending from '@/components/base/BaseIconPending.vue';

onMounted(() => {
  console.log('%c onMounted Explore', 'font-weight: 700; color:amber');
})

const { status, suggestionFriends } = registerFetchSuggestionFriendsOnPeoplePage();

</script>

<template>
  <LayoutBase>
    <template #appBar>
      <AppBar />
    </template>
    <template #body>
      <div class="space-y-4 px-4 py-6 max-w-2xl m-auto">
        <BaseIconPending :delay="1000" :pending="status.Pending" :once="true">

          <UserTile v-for="{ data: user, followStatus } in suggestionFriends" :size="50"
            :avatar="user!.avatar" :userName="user!.userName" :subtile="user!.subtile">
            <template #trailing>
              <FollowButton :isFollowing="followStatus === 'following'" :avatar="user!.avatar!" variant="text-btn"
                :userId="user!.id" :userName="user!.userName" />
            </template>
          </UserTile>

          <template #pending>
            <div class="p-4 max-w-sm w-full mx-auto space-y-2" v-for="i in 4">
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
          </template>
        </BaseIconPending>

      </div>
    </template>
  </LayoutBase>
</template>