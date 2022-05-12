<template>
  <span class="cursor-pointer">
    <svg aria-label="Save" class="_8-yf5 " color="#262626" fill="#262626" :height="size" role="img" viewBox="0 0 24 24" :width="size"><polygon :fill="saved?'#262626':'none'" points="20 21 12 13.44 4 21 4 3 20 3 20 21" stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"></polygon></svg>
  </span>
</template>

<script setup lang="ts">
defineProps({
  saved: Boolean,
  size: {type: Number, default: 24}
})
</script>