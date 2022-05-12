<script setup lang="ts">
import { onMounted,onUnmounted,watch } from 'vue'
import IconClose from '@/icons/IconClose.vue'
import { getScrollbarWidth } from "@/helpers";

const props = defineProps({
  open: { type: Boolean, default: false },
  scrollOffset: { type: Number, default: 0 },
  autoScroll: { type: Boolean, default: true }
})
const emit = defineEmits<{
  (event: 'closeButtonClick'): void,
  (event: 'backdropClick'): void
}>()

let scrollPosition = 0;

onMounted(() => {
  if (props.open)
    openModal();
})

onUnmounted(() => {
  closeModal();
})

watch(() => props.open, () => {
  if(!props.open) closeModal()
  else openModal()
})

function openModal() {

  scrollPosition = props.autoScroll ? window.scrollY : props.scrollOffset;
  const scrollWidth = getScrollbarWidth() + 'px';
  const appBar = document.getElementById('appBar');
  const baseModalReferenceId = document.getElementById('baseModalReferenceId');

  document.body.style.position = 'fixed';
  if (baseModalReferenceId) {
    baseModalReferenceId.style.transform = `translateY(-${scrollPosition}px)`;
  }
  document.body.style.paddingRight = scrollWidth;
  if (appBar) {
    appBar.style.right = scrollWidth;
  }
}


function closeModal() {

  document.body.style.position = 'static';
  document.body.style.paddingRight = '0';
  const appBar = document.querySelector<HTMLElement>('#appBar');
  const baseModalReferenceId = document.getElementById('baseModalReferenceId');
  if (appBar) {
    appBar.style.right = '0px';
  }
  if (baseModalReferenceId) {
    baseModalReferenceId.style.transform = ``;
  }
  document.documentElement.scrollTop = scrollPosition;
}

</script>

<template>
  <Teleport to="body">
    <!-- <Transition> -->
    <div v-if="open" class="BASEMODAL fixed z-50 inset-0 backdrop">
      <span class="absolute right-2 top-2">
        <IconClose class="text-white" @click="$emit('closeButtonClick')" />
      </span>
      <div class="flex items-center justify-center max-w-full h-screen overflow-auto py-6 px-2 desktop:px-16" @click="$emit('backdropClick')">
        <slot name="content" />
      </div>
    </div>
    <!-- </Transition> -->
  </Teleport>
</template>
<style scoped>
.backdrop {
  background-color: hsla(0, 0%, 0%, 0.214);
}

.v-enter-active,
.v-leave-active {
  transition: all 0.5s ease;
}

.v-enter-from,
.v-leave-to {
  opacity: 0;
  transform: translateY(100%);
  /* overflow: hidden; */
  /* font-weight: 700; */
}
</style>