<script setup lang="ts">
import { useUserSearchRecent } from '@/composable/use-search-user';
import { onMounted } from 'vue';
import UserSearchItem from './UserSearchItem.vue';
import { PageUrlBuilder } from "@/helpers/pageUrlBuilder";

const { loadUsers, clearAll, deleteUser, users } = useUserSearchRecent();
onMounted(() => {
  loadUsers();
})
</script>

<template>
  <div class>
    <div class="flex justify-between mb-2 px-4 pt-4">
      <h3 class="font-semibold text-base">Recent</h3>
      <button class="text-blue-300 font-medium text-sm" @click="clearAll">Clear All</button>
    </div>
    <UserSearchItem v-for="user in users" :user="user" :can-delete="true" @delete="deleteUser" @select="$router.push(PageUrlBuilder['/:username/'](user.userName))"
      :key="user.userName" />
  </div>
</template>