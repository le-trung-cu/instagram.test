<script setup lang="ts">
import AppBar from '../../components/AppBar.vue';
import IconDirect from '../../icons/IconDirect.vue';
import IconInstagram from '../../icons/IconInstagram.vue';
import IconNewStory from '../../icons/IconNewStory.vue';
</script>
<template>
  <AppBar class="h-11">
    <template #leading><IconNewStory/></template>
    <template #center><IconInstagram/></template>
    <template #actions><IconDirect/></template>
  </AppBar>
</template>