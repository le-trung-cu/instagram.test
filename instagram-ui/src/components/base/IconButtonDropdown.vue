<script setup lang="ts">
import BaseBoxDropdown from './BaseBoxDropdown.vue';
import { defineProps, PropType, ref } from 'vue';

defineProps({
  width: { type: Number, default: 0 },
  position: String as PropType<'left' | 'right' | 'center'>,
  offset: { type: Number, default: 0 },
  backdrop: { type: Boolean, default: true }
})
const openBtn = ref<HTMLElement>();
</script>

<template>
  <BaseBoxDropdown :width="width" :offset="offset" :position="position">
    <template #default="{ closeDropdown, toggleDropdown }">
      <button ref="openBtn" @click="toggleDropdown" @blur="closeDropdown">
        <slot></slot>
      </button>
    </template>
    <template #dropdown="{ openDropdown }">
      <div @click="openDropdown" @mousedown="openDropdown" @mousemove="openDropdown" @mouseup="openBtn?.focus()">
        <slot name="dropdown"></slot>
      </div>
    </template>
  </BaseBoxDropdown>
</template>