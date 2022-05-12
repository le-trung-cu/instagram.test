<script setup lang="ts">
import LayoutBase from '@/layouts/LayoutBase.vue';
import AppBar from './AppBar.vue';
import PostItem from '@/components/PostItem.vue';
import { useRoute } from 'vue-router';
import { onMounted, } from 'vue';
import PostItemSkeleton from '@/components/PostItemSkeleton.vue';
import { useApi } from '@/api/composable/use-api';
import { fetchPostBySlugApi } from '@/api/post-api';
import { currentPost, useFetchPostBySlug } from "@/composable/use-post-store";

const props = defineProps({
  slug: String,
});

const { exec, status} = useFetchPostBySlug();
onMounted(() => {
  console.log('%c onMounted PostDetail', 'color: darkorange; font-weight: 700;');
})
const slug = useRoute().params.slug as string;
onMounted(async () => {
  await exec(slug);
})

</script>

<template>
  <LayoutBase>
    <template #appBar>
      <AppBar />
    </template>
    <template #body>
      <div class="p-4">
        <PostItem v-if="status.Success && currentPost.data" class="mt-6 min-h-[70vh]" :post="currentPost.data"
          :showComment="true" :isFollowing="currentPost.followStatus=='following'"/>
        <PostItemSkeleton v-if="status.Pending || status.Idle" />
      </div>
    </template>
  </LayoutBase>
</template>