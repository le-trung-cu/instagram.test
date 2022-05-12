<script setup lang="ts">
import { onMounted } from 'vue';
import IconSpinner from '@/icons/IconSpinner.vue';
import Thumbnail from '@/components/Thumbnail.vue';
import { registerFetchGridPost } from '@/composable/use-profile-manager';
import BaseIconPending from '@/components/base/BaseIconPending.vue';

onMounted(() => {
  console.log('%c onMounted GridPost', 'font-weight: 700; color:amber');
})

const { status, collectionGridPost } = registerFetchGridPost();

</script>

<template>
    <BaseIconPending :pending="status.Pending" :delay="1000" :once="true">
      <template #pending>
        <div class="flex justify-center">
          <IconSpinner :size="60" class="flex-shrink-0"/>
        </div>
      </template>
    <div class="px-1">
      <p class="text-gray-500 text-sm text-center py-3 border-b-2">When you share photos, they will appear on your profile.</p>
      <div class="grid grid-cols-3 gap-0.5 sm:gap-7 mt-7 pb-10">
        <Thumbnail v-for="item in collectionGridPost" :thumbnail="item.thumbnail" :likeCount="item.likeCount" 
        :commentCount="item.commentCount" :key="item.id" :slug="item.slug" />
      </div>
    </div>
    </BaseIconPending>
</template>