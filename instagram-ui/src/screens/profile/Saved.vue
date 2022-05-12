<script setup lang="ts">
import { useApi } from '@/api/composable/use-api';
import { fetchSavedPostsApi } from '@/api/post-api';
import { IPost } from '@/models/IPost';
import { onActivated, onDeactivated, onMounted, onUnmounted, reactive, watchEffect } from 'vue';
import { whenScrollAtBottom } from '@/composable/use-scroll-at-bottom';
import IconSpinner from '@/icons/IconSpinner.vue';
import { IPostParameter } from '@/api/request-features/IPostParameter';
import Thumbnail from '@/components/Thumbnail.vue';
import { useRouter } from 'vue-router';

onMounted(() => {
  console.log(useRouter());
  console.log('%c onMounted Saved', 'font-weight: 700; color:amber');
})

const { data, exec: fetchSavedPosts, status, statusSuccess } = useApi(fetchSavedPostsApi);
const collectionPost = reactive(new Array<IPost>());
const postParameter: IPostParameter = { pageNumber: 1, pageSize: 7 }
let hasNextPage: boolean = true;

const { listener, clearListener } = whenScrollAtBottom(async () => {
  if (!status.value.Pending && hasNextPage) {
    await fetchSavedPosts(postParameter);
    if (statusSuccess.value) {
      data.value && (hasNextPage = data.value.pagination.hasNext);
      data.value?.items && collectionPost.push(...data.value?.items);
      hasNextPage && postParameter.pageNumber++;
    }
  }
});

onMounted(async () => {

  await fetchSavedPosts(postParameter);
  collectionPost.length = 0;
  if (statusSuccess.value) {
    data.value && (hasNextPage = data.value.pagination.hasNext);
    data.value?.items && collectionPost.push(...data.value?.items);
    hasNextPage && postParameter.pageNumber++;
  }
  listener()
})
onUnmounted(() => {
  clearListener();
})
onActivated(() => {
  listener();
})
onDeactivated(() => {
  clearListener();
})
</script>

<template>

  <div class="px-1">
    <p class="text-gray-500 text-sm text-center py-3 border-b-2">Only you can see what you've saved</p>
    <div class="grid grid-cols-3 gap-0.5 sm:gap-7">
      <Thumbnail v-for="item in collectionPost" :thumbnail="item.thumbnail" :key="item.id" :slug="item.slug"
        :commentCount="item.commentCount" :likeCount="item.likeCount" />
    </div>
  </div>
  <div v-show="status.Pending" class="h-14 flex justify-center items-center">
    <IconSpinner />
  </div>
  <router-view></router-view>
</template>