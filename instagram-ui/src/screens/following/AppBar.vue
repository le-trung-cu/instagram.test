
<script setup lang="ts">
import AppBar from '@/components/AppBar.vue';
import IconBack from '@/icons/IconBack.vue';
</script>

<template>
  <AppBar class="h-11">
    <template #leading><IconBack @click="$router.go(-1)"/></template>
    <template #center>
      <h2 class="font-semibold text-gray-900">Following</h2>
    </template>
    <template #actions></template>
  </AppBar>

</template>
