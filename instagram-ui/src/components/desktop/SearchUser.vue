<script setup lang="ts">
import IconSearch from '@/icons/IconSearch.vue';
import IconSearchClear from '@/icons/IconSearchClear.vue';
import { ref } from 'vue';
import IconSpinner from '@/icons/IconSpinner.vue';
import BaseBoxDropdown from '../base/BaseBoxDropdown.vue';
import RecentSearch from '../RecentSearch.vue';
import { useUserSearchRecent , useSearchUser} from '@/composable/use-search-user';
import UserSearchItem from '../UserSearchItem.vue';
import { PageUrlBuilder } from "@/helpers/pageUrlBuilder";

const input = ref<HTMLInputElement>();
const isOpen = ref(false);
const {stateSearch: state } = useSearchUser();
const { saveUser } = useUserSearchRecent();

</script>
<template>
  <BaseBoxDropdown position="center" :width="500" :backdrop="false">
    <template #default="{ openDropdown, closeDropdown }">
      <form class="hidden w-64 sm:block bg-zinc-200 px-4 rounded-md">
        <div class="flex items-center">
          <label for="searchUser">
            <IconSearch v-show="!isOpen" class="mr-3" />
          </label>
          <input
            ref="input"
            id="searchUser"
            class="h-9 flex-grow bg-zinc-200 placeholder-shown:font-thin text-base text-gray-900 focus:outline-none"
            autocomplete="off"
            placeholder="Search"
            v-model="state.searchQuery"
            @focus="() => { openDropdown(); isOpen = true }"
            @blur="() => { closeDropdown(); isOpen = false }"
          />
          <IconSpinner v-if="state.isSearching" />
          <IconSearchClear
            v-else
            v-show="state.searchQuery"
            @click="state.searchQuery = ''; input?.focus()"
          />
        </div>
      </form>
    </template>
    <template #dropdown="{ openDropdown }">
      <div class="h-[500px] bg-white" @mousedown="openDropdown" @mouseup="input?.focus()">
        <div v-if="!state.searchQuery">
          <RecentSearch />
        </div>
        <div v-else>
          <div v-if="state.isSearching">
            <IconSpinner />
          </div>
          <router-link v-for="user in state.userSearchResults" :to="PageUrlBuilder['/:username/'](user.userName)">
          <UserSearchItem
            :user="user"
            :key="user.userName"
            @click="saveUser(user); $router.push({path: ``})"
          />
          </router-link>
        </div>
      </div>
    </template>
  </BaseBoxDropdown>
</template>