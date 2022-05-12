<script setup lang="ts">
import AppBar from '@/components/AppBar.vue';
import { IconInstagram, IconHome, IconNewPost, IconDirect } from '@/icons';
import AccountDropdown from '@/components/desktop/AccountDropdown.vue';
import NotifyCationActivityOnPostDropdown from '@/components/desktop/NotifyCationActivityOnPostDropdown.vue';
import SearchUser from '../components/desktop/SearchUser.vue';

</script>

<template>
  <div id="baseLayout">
    <header id="appBar" class="fixed top-0 left-0 right-0 z-30 border-b-gray-300 border-b">
      <AppBar class="h-14 box-max-width">
        <template #leading>
          <router-link to="/">
            <IconInstagram />
          </router-link>
        </template>
        <template #center>
          <SearchUser />
        </template>
        <template #actions>
          <div class="flex items-center space-x-5">
            <router-link to="/">
              <IconHome />
            </router-link>
            <router-link to="/explore">
              <IconDirect />
            </router-link>
            <IconNewPost />
            <NotifyCationActivityOnPostDropdown />
            <AccountDropdown />
          </div>
        </template>
      </AppBar>
    </header>
    <section id="baseModalReferenceId" class="pt-14 box-max-width">
      <slot name="body"></slot>
    </section>
  </div>
</template>