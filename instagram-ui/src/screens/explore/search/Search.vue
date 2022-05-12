<script setup lang="ts">
import LayoutBase from '@/layouts/LayoutBase.vue';
import { onMounted } from 'vue';
import { useSearchUser, useUserSearchRecent } from "@/composable/use-search-user";

import { PageUrlBuilder } from "@/helpers/pageUrlBuilder";
import AppBar from './AppBar.vue';
import UserSearchItem from '@/components/UserSearchItem.vue';
import RecentSearch from '@/components/RecentSearch.vue';

onMounted(() => {
  console.log('%c onMounted Search', 'font-weight: 700; color:amber');
})

const { stateSearch: state, } = useSearchUser()
const { saveUser } = useUserSearchRecent()

</script>

<template>
  <LayoutBase>
    <template #appBar>
      <AppBar :searchQuery="state.searchQuery" @update:modelValue="state.searchQuery = $event" />
    </template>
    <template #body>
      <div>

        <div v-if="!state.searchQuery">
          <RecentSearch />
        </div>
        <div v-else>
          <div v-if="state.isSearching">
            <IconSpinner />
          </div>
          <UserSearchItem v-for="user in state.userSearchResults" :user="user" :key="user.userName"
            @click="saveUser(user); $router.push({ path: PageUrlBuilder['/:username/'](user.userName) })" />
        </div>
      </div>
    </template>
  </LayoutBase>
</template>
