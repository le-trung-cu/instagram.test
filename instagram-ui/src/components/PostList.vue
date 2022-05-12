<script setup lang="ts">
import { onMounted, PropType } from 'vue';
import { IPost } from '@/models/IPost';
import { WithFollowStatus } from '@/helpers';
import PostItemNotification from './PostItemNotification.vue';
import PostItem from './PostItem.vue';

const props = defineProps({
  items: Array as PropType<WithFollowStatus<IPost>[]>
});

</script>

<template>
  <div class="space-y-6 w-full max-w-full">
    <PostItem v-for="item in items"  :isFollowing="item.followStatus=='following'" :showComment="false" :key="item.data?.id"
      :post="item.data!" />
  </div>
</template>