<script setup lang="ts">
defineProps({
  size: {type: Number, default: 40}
})
</script>

<template>
  <img class="rounded-full" :width="size" :height="size" src="/icons/none-profile.jpg"/>
</template>