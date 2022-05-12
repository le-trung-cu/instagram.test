
<script setup lang="ts">
import AppBar from '@/components/AppBar.vue';
import IconSearch from '@/icons/IconSearch.vue';
import { onMounted, ref } from 'vue';

const props = defineProps({
  searchQuery: { type: String, default: '' }
})

defineEmits<{
  (event: 'update:modelValue', value: string | number): void
}>()

const input = ref<HTMLElement>();

onMounted(() => {
  input.value?.focus();
})

</script>

<template>
  <AppBar class="h-11" :centerStyle="{ width: '100%' }">
    <template #leading></template>
    <template #center>
      <div class="px-6">
        <div class="flex">
          <form class="flex items-center flex-grow relative">
            <input ref="input" id="search" autocomplete="off"
              class="search-input h-full py-1 px-8 w-full border-gray-300 border rounded-md "
              @input="$emit('update:modelValue', ($event.target as HTMLInputElement).value)" v-bind="$attrs" />
            <label for="search"
              class="absolute flex justify-center items-center text-gray-400 left-1/2 -translate-x-1/2 transition-all delay-150"
              :class="searchQuery.length > 0 && 'search-label'">
              <IconSearch :size="14" class="mr-2" /><span v-if="searchQuery.length == 0">Search</span>
            </label>
          </form>
          <button class="font-semibold text-base ml-2" @click="$router.go(-1)">Cancel</button>
        </div>
      </div>
    </template>
    <template #actions></template>
  </AppBar>
</template>

<style scoped>
.search-label {
  left: 15px;
  transform: translateX(0);
}
#search:focus+label{
  left: 15px;
  transform: translateX(0);
}
</style>