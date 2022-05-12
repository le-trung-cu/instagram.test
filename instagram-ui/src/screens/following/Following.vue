<script setup lang="ts">
import LayoutBase from '@/layouts/LayoutBase.vue';
import UserTile from "@/components/UserTile.vue";
import AppBar from './AppBar.vue';
import { registerFetchFollowing, } from '@/composable/use-relationship-manager';
import FollowButton from '@/components/FollowButton.vue';
import IconSpinner from '@/icons/IconSpinner.vue';

const props = defineProps({
  userName: String,
})

const { following, status } = registerFetchFollowing(props.userName!);

</script>

<template>
  <LayoutBase>
    <template #appBar>
      <AppBar />
    </template>
    <template #body>
      <div class="space-y-3 max-w-2xl mx-auto mt-4 sm:mt-28 px-3">
        <h2 class="font-semibold text-base text-gray-800 mb-4 ">Suggested</h2>
        <div class="px-3 space-y-4 py-4 bg-white">
          <UserTile v-for="{data: user, followStatus} in following" :size="60" :avatar="user!.avatar" :fullName="user!.fullName"
           :userName="user!.userName">
            <template #trailing>
                <FollowButton :isFollowing="followStatus == 'following'" variant="bg-btn" :avatar="user!.avatar" :userId="user!.id" :userName="user!.userName" />
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
