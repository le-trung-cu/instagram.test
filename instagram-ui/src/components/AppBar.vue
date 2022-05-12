<script setup lang="ts">import { CSSProperties, PropType } from 'vue';

defineProps({
  centerStyle: Object as PropType<CSSProperties>
})
</script>

<template>
  <div class="flex flex-row justify-between items-center px-4 bg-white">
    <div>
      <slot name="leading"></slot>
    </div>
    <div class="absolute -translate-x-1/2 -translate-y-1/2 top-1/2 left-1/2" :style="centerStyle">
      <slot name="center"></slot>
    </div>
    <div>
      <slot name="actions"></slot>
    </div>
  </div>
</template>