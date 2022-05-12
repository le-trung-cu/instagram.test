<script setup lang="ts">
import { IComment } from '@/models/IComment';
import { onMounted, onUnmounted, PropType, reactive, ref, watch } from 'vue';
import { ICommentParameter } from '@/api/request-features/ICommentParameter';
import { fetchCommentsApi } from '@/api/comment-api';
import { IPost } from '@/models/IPost';
import { useApi } from '@/api/composable/use-api';
import IconPlus from '../icons/IconPlus.vue';
import CommentItem from './CommentItem.vue';
import { useLayout } from '@/composable/useLayout';
import { whenScrollAtBottom } from '@/composable/use-scroll-at-bottom';
import BaseIconPending from './base/BaseIconPending.vue';

const props = defineProps({
  postSlug: { type: String as PropType<IPost['slug']>, required: true }
})
const commentCollection = reactive( new Array<IComment>());
const { data, exec: _fetchComments, status } = useApi(fetchCommentsApi);
let commentParameter: ICommentParameter = {
  pageNumber: 0,
  pageSize: 10,
}
let hasNextPage = ref(false);
const { layout, LAYOUTS } = useLayout();
const isMobile = layout.value == LAYOUTS.mobile;
const fetchCommentsAutoRegister = isMobile ? whenScrollAtBottom(async () => {
  if (!status.value.Pending && hasNextPage.value) {
    await fetchComments();
  }
}) : null;

onMounted( async () => { await fetchComments(); fetchCommentsAutoRegister?.listener(); })
onUnmounted(() => { fetchCommentsAutoRegister?.clearListener(); })

async function fetchComments() {
  await _fetchComments(props.postSlug, { ...commentParameter, pageNumber: commentParameter.pageNumber + 1 });
  if (status.value.Success) {
    if (data.value?.items) {
      commentCollection.push(...data.value.items);
      hasNextPage.value = data.value.pagination.hasNext;
      commentParameter.pageNumber++;
    }
  }
}

function likeCommentChange(commentId: number, liked: boolean){
  const comment = commentCollection.find(t => t.id === commentId);
  comment && (comment.liked = liked);
}

</script>

<template>
  <div class="h-full overflow-y-scroll min-h-[300px]">
    <CommentItem v-for="item in commentCollection" :key="item.id" :comment="item"
      @likeCommentChange="likeCommentChange"/>
    <div class="h-11 flex justify-center items-center">
      <BaseIconPending v-if="hasNextPage" :pending="status.Pending">
        <IconPlus @click="fetchComments()" />
      </BaseIconPending>
    </div>  </div>
</template>