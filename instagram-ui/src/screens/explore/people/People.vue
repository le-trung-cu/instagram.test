<script setup lang="ts">
import LayoutBase from '@/layouts/LayoutBase.vue';
import UserTile from "@/components/UserTile.vue";
import AppBar from './AppBar.vue';
import { registerFetchSuggestionFriendsOnPeoplePage } from '@/composable/use-relationship-manager';
import FollowButton from '@/components/FollowButton.vue';
import IconSpinner from '@/icons/IconSpinner.vue';

const { suggestionFriends, status } = registerFetchSuggestionFriendsOnPeoplePage();

</script>

<template>
  <LayoutBase>
    <template #appBar>
      <AppBar />

    </template>
    <template #body>
      <div class="space-y-3 max-w-2xl mx-auto mt-4 sm:mt-28 px-3">
        <h2 class="font-semibold text-base text-gray-800 mb-4 ">Suggested</h2>
        <div class="px-3 py-4 bg-white">
          <UserTile v-for="{ data: user, followStatus } in suggestionFriends" :size="60" :avatar="user.avatar"
            :fullName="user.fullName" :subtile="user.subtile" :userName="user.userName">
            <template #trailing>
              <FollowButton :avatar="user.avatar!" :isFollowing="followStatus == 'following'" variant="bg-btn"
                :userId="user.id" :userName="user.userName" />
            </template>
          </UserTile>
          <div class="h-6 flex justify-between items-center">
            <IconSpinner v-if="status.Pending" class="animate-spin" />
          </div>
        </div>
      </div>
    </template>
  </LayoutBase>
</template>
