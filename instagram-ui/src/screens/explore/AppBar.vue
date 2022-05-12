<template>
  <AppBar class="h-11" :centerStyle="{width: '100%'}">
    <template #leading></template>
    <template #center>
      <div class="flex px-8  ">
        <router-link to="/explore/search" class="flex py-1 px-8 border-gray-300 border rounded-sm items-center flex-grow justify-center" >
          <IconSearch :size="14" class="mr-2"/>
        Search
      </router-link>
        </div>
    </template>
    <template #actions></template>
  </AppBar>
  
</template>

<script setup lang="ts">
import AppBar from '../../components/AppBar.vue';
import IconSearch from '@/icons/IconSearch.vue';
</script>