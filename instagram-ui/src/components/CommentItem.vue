<script setup lang="ts">
import IconHeart from '@/icons/IconHeart.vue';
import IconNoneProfile from '@/icons/IconNoneProfile.vue'
import { IComment } from '@/models/IComment';
import { inject, PropType } from 'vue';
import ago from "ts-ago";
import IconSpinner from '@/icons/IconSpinner.vue';
import { useApi } from '@/api/composable/use-api';
import { likeCommentApi } from '@/api/comment-api';
import CircleImage from './base/CircleImage.vue';
import BaseIconPending from './base/BaseIconPending.vue';

import { PageUrlBuilder } from "@/helpers/pageUrlBuilder";

defineProps({
  comment: { type: Object as PropType<IComment>, required: true }
});
const emit = defineEmits<{
  (event: 'likeCommentChange', commentId: number, liked: boolean): void,
}>();

const clickReplyComment = inject('clickReplyComment') as Function;

const { exec: likeComment, status } = useApi(likeCommentApi);

const toggleLikeComment = async (commentId: number, liked: boolean) => {
  if (!status.value.Pending) {
    await likeComment(commentId, liked);
    if (status.value.Success)
      emit('likeCommentChange', commentId, liked);
  }
}

</script>
<template>
  <div class="flex py-2 px-2">
    <CircleImage class="mr-2 flex-shrink-0" :src="comment.avatar || '/icons/none-profile.jpg'" :size="30"/>
    <div class="flex-grow">
      <div>
        <router-link :to="PageUrlBuilder['/:username/'](comment.userName)" class="font-semibold text-sm mr-1">{{ comment.userName }}</router-link>
        <span class="text-sm font-light text-gray-900">{{ comment.content }}</span>
      </div>
      <div>
        <span class="mr-5 text-xs text-gray-500">{{ ago(comment.createdAt) }}</span>
        <span v-if="comment.likeCount > 0" class="mr-5 text-xs text-gray-500">{{ comment.likeCount }} likes</span>
        <button class="text-gray-400 font-semibold text-xs" @click="clickReplyComment(comment.userName)">Reply</button>
      </div>
    </div>
    <BaseIconPending class="mx-2 flex-shrink-0" :pending="status.Pending" :size="24">
      <IconHeart class="mx-2 flex-shrink-0" @click="toggleLikeComment(comment.id, !comment.liked)" :liked="comment.liked" />
    </BaseIconPending>
  </div>
</template>

