<script setup lang="ts">
import LayoutBase from '@/layouts/LayoutBase.vue';
import PostList from '@/components/PostList.vue';
import { defineAsyncComponent, onMounted, onUnmounted } from 'vue';
import AppBar from './AppBar.vue';
import { useLayout } from '@/composable/useLayout';
import IconSpinner from '@/icons/IconSpinner.vue';
import { homeScrollPosition, useHomeScrollPosition } from '@/composable/use-home-scroll-position';
import { registerFetchPostHomePage } from '@/composable/use-post-store';

onUnmounted(() => {
  console.log('%c onUnmounted Home', 'font-weight: 700; color: red;');
});

const SuggestionsFriendsAsync = defineAsyncComponent({ loader: () => import('./SuggestionsFriends.vue') });

const { layout, LAYOUTS } = useLayout();
const {status, collectionPostHomePage} = registerFetchPostHomePage();

// useHomeScrollPosition();
// onMounted(() => {
//   setTimeout(() => {
//     window.scroll(0, homeScrollPosition.value);
//   }, 0)
// })

</script>

<template>
  <div>
    <LayoutBase>
      <template #appBar>
        <AppBar />
      </template>
      <template #body>
        <div v-if="collectionPostHomePage" class="max-w-[935px] mx-auto flex flex-row">
          <div class="max-w-[600px] desktop:w-2/3 mx-auto w-full">
            <PostList :items="collectionPostHomePage" />
            <div v-show="status['Pending']" class="flex justify-center items-center h-14">
              <IconSpinner />
            </div>
          </div>
          <div class="flex-grow ml-6 hidden desktop:block">
            <SuggestionsFriendsAsync v-if="layout == LAYOUTS.desktop" class="sticky top-32" />
          </div>
        </div>
      </template>
    </LayoutBase>
  </div>

</template>