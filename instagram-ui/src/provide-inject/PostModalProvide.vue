<script setup lang="ts">
import { PageUrlBuilder } from '@/helpers/pageUrlBuilder';
import { provide, ref, watch } from 'vue';
import { useRoute } from 'vue-router';

import PostModal from './PostModal.vue';

const _slug = ref<string>();

provide('openPostModal', (slug: string, event?: Event) => {
  event?.preventDefault();
  console.log(event);
  
  history.pushState({}, '', PageUrlBuilder['/p/:slug'](slug));
  _slug.value = slug;
});

provide('closePostModal', close)

function close(redirect?: string) {
  if (_slug) {
    redirect? history.replaceState({}, '', redirect) : history.back();
    _slug.value = undefined;
  }
}
const route = useRoute();
watch(() => route.fullPath, () => {
  if(_slug){
    _slug.value = undefined;
  }
})
</script>


<template>
  <slot></slot>
  <PostModal v-if="_slug" :slug="_slug" @closePostModal="close" />
</template>