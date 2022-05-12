<script setup lang="ts">
import BaseModal from '@/components/base/BaseModal.vue';
import { useRoute } from 'vue-router';
import { onActivated, onMounted, onUnmounted, ref } from 'vue';
import PostItem from '@/components/PostItem.vue';
import { useBoolean } from '@/composable/use-toggle';
import PostItemSkeleton from '@/components/PostItemSkeleton.vue';
import { currentPost, setCurrentPost, useFetchPostBySlug } from "@/composable/use-post-store";

onMounted(async () => {
  console.log('%c onMounted PostModal', 'color: darkorange; font-weight: 700;');
})
const slug = useRoute().params.slug as string;
const { exec, status } = useFetchPostBySlug();
const { state: isOpenModal, } = useBoolean(true);
const refBox = ref<HTMLElement>();

const refBoxWidth = ref('100%');
const commentHeight = ref('');
onMounted(async () => {
  await exec(slug);
  setWidthForRefBox();
  window.addEventListener('resize', setWidthForRefBox);
})
onUnmounted(() => {
  window.removeEventListener('resize', setWidthForRefBox);
  console.log('%c onUnMounted PostModal', 'color: darkorange; font-weight: 700;');
})

onActivated(() => {
  console.log('post Modal activited');
  
})

function setWidthForRefBox() {
  if (refBox.value) {
    refBoxWidth.value = refBox.value.clientHeight * 1.5 + 'px';
    commentHeight.value = `calc(${refBox.value.clientHeight}px - 180px - 65px)`
  }
}
// const _post = getPostMock();
</script>

<template>

  <BaseModal :open="isOpenModal" @closeButtonClick="isOpenModal = false; $router.go(-1)" :autoScroll="true"
    >
    <template #content>
      <div ref="refBox" class="REF_BOX w-full h-full tablet:px-14 flex items-center justify-center max-w-[1080px]"
        :style="{ width: refBoxWidth }">
        <PostItem v-if="status.Success && currentPost.data" class="max-h-full min-h-[70vh]" :post="currentPost.data"
          :showComment="true" :isFollowing="currentPost.followStatus=='following'"/>
        <PostItemSkeleton v-else />
      </div>
    </template>
  </BaseModal>
</template>