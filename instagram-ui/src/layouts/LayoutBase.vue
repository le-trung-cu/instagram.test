<script setup lang="ts">
import { useLayout } from '@/composable/useLayout';
import { computed } from '@vue/reactivity';
import { useSlots } from 'vue';
import LayoutDesktopBase from './LayoutDesktopBase.vue';
import LayoutMobileBase from './LayoutMobileBase.vue';

const { layout, LAYOUTS } = useLayout();
const layoutComponents = {
  [LAYOUTS.mobile]: LayoutMobileBase,
  [LAYOUTS.desktop]: LayoutDesktopBase,
}

const currentLayoutComponent = computed(() => layoutComponents[layout.value])
const slots = useSlots();
</script>

<template>
  <component :is="currentLayoutComponent">
    <template
      v-for="slotName in Object.keys(slots)"
      :key="slotName"
      v-slot:[slotName]="slotProps">
      <slot :name="slotName" v-bind="slotProps"/>
    </template>
  </component>
</template>