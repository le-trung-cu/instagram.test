<template>
  <div v-show="showLoader">
    <slot></slot>
  </div>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue';

const props = defineProps({
  show: { type: Boolean, default: false },
  delay: { type: Number, default: 500 }
});

const showLoader = ref(props.show);
let timeout: number;

watch(() => props.show, (show) => {
  if (show) {
    timeout = setTimeout(() => {
      showLoader.value = true;
    }, props.delay);
  } else {
    clearTimeout(timeout);
    showLoader.value && (showLoader.value = false);
  }
  return { showLoader }
})

</script>