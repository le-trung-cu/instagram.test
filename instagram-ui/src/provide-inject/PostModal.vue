<script setup lang="ts">
import BaseModal from '@/components/base/BaseModal.vue';
import { onActivated, onMounted, onUnmounted, ref } from 'vue';
import PostItem from '@/components/PostItem.vue';
import { useBoolean } from '@/composable/use-toggle';
import PostItemSkeleton from '@/components/PostItemSkeleton.vue';
import { currentPost, setCurrentPost, useFetchPostBySlug } from "@/composable/use-post-store";
import BaseIconPending from '@/components/base/BaseIconPending.vue';

onMounted(async () => {
  console.log('%c onMounted PostModal', 'color: darkorange; font-weight: 700;');
})

const props = defineProps({
  slug: { type: String, required: true },
})

const emit = defineEmits<{
  (event: 'closePostModal'): void;
}>()

const { exec, status } = useFetchPostBySlug();
const refBox = ref<HTMLElement>();

const refBoxWidth = ref('100%');
const commentHeight = ref('');
onMounted(async () => {
  await exec(props.slug);
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

</script>

<template>
  <BaseModal :open="true" @closeButtonClick="$emit('closePostModal')" :autoScroll="true"
    @backdropClick="$emit('closePostModal')">
    <template #content>
      <div ref="refBox" class="REF_BOX w-full h-full tablet:px-14 flex items-center justify-center max-w-[1080px]"
        :style="{ width: refBoxWidth }">
        <BaseIconPending :pending="status.Pending" :delay="500">
          <template #pending>
            <PostItemSkeleton />
          </template>
          <PostItem @click.stop="" v-if="status.Success && currentPost.data" class="max-h-full min-h-[70vh]"
            :post="currentPost.data" :showComment="true" :isFollowing="currentPost.followStatus == 'following'" />
        </BaseIconPending>
      </div>
    </template>
  </BaseModal>
</template>