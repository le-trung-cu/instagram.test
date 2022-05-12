<template>
  <img :style="{height: height+'px'}" src="/icons/instagram.png"/>
</template>

<script setup lang="ts">
defineProps({
  height: {type: Number, default: 30}
})
</script>