<script setup lang="ts">
import feedImage from "./feed.png";
defineProps({
  size: { type: Number, default: 24 }
})
</script>

<template>
  <span class="inline-block">
    <img class="grayscale" :src="feedImage" :height="size" :width="size" />
  </span>
</template>