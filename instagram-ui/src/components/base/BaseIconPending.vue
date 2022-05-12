<script lang="ts">
export default {
  inheritAttrs: false
}
</script>

<script setup lang="ts">
import { ref, watch } from 'vue';
import IconSpinner from '@/icons/IconSpinner.vue';

const props = defineProps({
  pending: { type: Boolean, default: false },
  delay: { type: Number, default: 500 },
  size: { type: Number, default: 24 },
  once: Boolean
});

const showLoader = ref(props.pending);
let timeout: number;
let isApply = false;

watch(() => props.pending, (pending) => {
  if (isApply && props.once) {
    return;
  }
  if (pending) {
    clearTimeout(timeout);
    !showLoader.value && (showLoader.value = true);
  } else {
    timeout = setTimeout(() => {
      showLoader.value = false;
      isApply = true;
    }, props.delay);
  }
  return { showLoader }
})

</script>

<template>
  <slot v-if="showLoader" name="pending">
    <IconSpinner v-bind="$attrs" :size="size" />
  </slot>
  <slot v-else></slot>
</template>
