
<script setup lang="ts">
import AppBar from '@/components/AppBar.vue';
import IconBack from '@/icons/IconBack.vue';
</script>

<template>
  <AppBar  class="h-11">
    <template #leading><IconBack @click="$router.go(-1)"/></template>
    <template #center><span class="text-base font-bold">Post</span></template>
  </AppBar>
</template>