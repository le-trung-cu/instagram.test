<template>
  <AppBar  class="h-11">
    <template #leading><IconSetting/></template>
    <template #center><span class="text-base font-bold">{{$route.params.username}}</span></template>
    <template #actions><IconDiscoverPeople/></template>
  </AppBar>
  
</template>

<script setup lang="ts">
import AppBar from '../../components/AppBar.vue';
import IconSetting from '../../icons/IconSetting.vue';
import IconDiscoverPeople from '../../icons/IconDiscoverPeople.vue';
</script>