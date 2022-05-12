<script setup lang="ts">
import { fetchCommentsApi } from '@/api/comment-api';
import { useApi } from '@/api/composable/use-api';
import { ICommentParameter } from '@/api/request-features/ICommentParameter';
import { whenScrollAtBottom } from '@/composable/use-scroll-at-bottom';
import LayoutBase from '@/layouts/LayoutBase.vue';
import { IComment } from '@/models/IComment';
import { onMounted, onUnmounted, ref } from 'vue';
import AppBar from './AppBar.vue';
import IconPlus from '@/icons/IconPlus.vue';
import { useLayout } from '@/composable/useLayout';
import IconSpinner from '@/icons/IconSpinner.vue';
import CommentItem from '@/components/CommentItem.vue';
import CommentList from '@/components/CommentList.vue';

const props = defineProps({
  slug: { type: String, required: true }
})
const commentCollection = new Array<IComment>();
const { data, exec: _fetchComments, status } = useApi(fetchCommentsApi);
let commentParameter: ICommentParameter = {
  pageNumber: 1,
  pageSize: 5,
}
let hasNextPage = ref(false);
const { layout, LAYOUTS } = useLayout();
const isMobile = layout.value == LAYOUTS.mobile;
const fetchCommentsAutoRegister = isMobile ? whenScrollAtBottom(async () => {
  if (!status.value.Pending && hasNextPage.value) {
    await fetchComments();
  }
}) : null;

onMounted(() => { fetchComments(); fetchCommentsAutoRegister?.listener(); })
onUnmounted(() => { fetchCommentsAutoRegister?.clearListener(); })

async function fetchComments() {
  await _fetchComments(props.slug, { ...commentParameter, pageNumber: commentParameter.pageNumber + 1 });
  if (status.value.Success) {
    if (data.value?.items) {
      commentCollection.push(...data.value.items);
      hasNextPage.value = data.value.pagination.hasNext;
      commentParameter.pageNumber++;
    }
  }
}
</script>

<template>
  <LayoutBase>
    <template #appBar>
      <AppBar />
    </template>
    <template #body>
      <CommentList :postSlug="slug"/>
      <!-- <CommentItem v-for="item in commentCollection" :comment="item" :key="item.id"/> -->
      <div class="h-10">
        <IconSpinner v-if="status.Pending"/>
        <IconPlus v-else-if="!isMobile && hasNextPage" @click="fetchComments" />
      </div>
    </template>
  </LayoutBase>
</template>