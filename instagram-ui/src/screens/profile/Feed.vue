<script setup lang="ts">
import { onMounted } from 'vue';
import IconSpinner from '@/icons/IconSpinner.vue';
import PostList from '@/components/PostList.vue';
import { registerFetchFeedPost } from '@/composable/use-profile-manager';

onMounted(() => {
  console.log('%c onMounted FeedPost', 'font-weight: 700; color:amber');
})
const { status, collectionPostFeedPage } = registerFetchFeedPost();

</script>

<template>
  <div class="max-w-[600px] desktop:w-2/3 mx-auto w-full mt-7">
    <div class="grid grid-cols-1 gap-0.5">
      <PostList :items="collectionPostFeedPage" />
    </div>
    <div v-show="status.Pending" class="h-14 flex justify-center items-center">
      <IconSpinner />
    </div>
  </div>
  <router-view>
  </router-view>
</template>