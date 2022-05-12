<script setup lang="ts">

import AuthorCard from './AuthorCard.vue';
import { CSSProperties, DeepReadonly, inject, onMounted, PropType, provide, ref } from 'vue';
import { IPost } from '@/models/IPost';
import PostAction from './PostAction.vue';
import MediaSlide from './MediaSlide.vue';
import CommentList from './CommentList.vue';
import CommentForm from './CommentForm.vue';
import { useLayout } from '@/composable/useLayout';

const props = defineProps({
  post: { type: Object as PropType<DeepReadonly<IPost>>, required: true, },
  showComment: { type: Boolean, default: true },
  mediaSlideStyle: { type: Object as PropType<CSSProperties> },
  isFollowing: Boolean,
});

const { layout, LAYOUTS } = useLayout();

const focusComment = ref(false);
const replyForUserName = ref('');
const openPostModal = inject('openPostModal') as Function;

provide('clickIconComment', clickIconComment);
provide('clickReplyComment', clickReplyComment);

function clickIconComment(e: MouseEvent) {

  if (layout.value === LAYOUTS.desktop) e.preventDefault();
  if (props.showComment) {
    focusComment.value = true;
  } else if (layout.value === LAYOUTS.desktop) {
    openPostModal(props.post.slug);
  } else {
    
  }
}

function clickReplyComment(userName: string) {
  focusComment.value = true;
  replyForUserName.value = '@' + userName + ' ';
}

</script>

<template>
  <div class="POST_ITEM border bg-white border-gray-300 overflow-hidden "
    :class="[showComment ? 'post-comment-layout' : 'only-post-layout']">
    <AuthorCard class="header"
      :author="{ ownerId: post.ownerId, avatar: post.avatar, title: post.title, userName: post.userName }"
      :slug="post.slug" :isFollowing="isFollowing" />
    <div class="comment pl-4 overflow-hidden relative" v-if="showComment">
      <div class="absolute h-full w-full overflow-y-scroll hide-scroll">
        <CommentList :postSlug="post.slug" />
      </div>
    </div>
    <div class="media flex items-center" :style="mediaSlideStyle">
      <MediaSlide class="w-full" :medias="post.mediaItems"/>
    </div>
    <div class="footer mt-2">
      <PostAction :post="post" />
      <CommentForm v-if="showComment" :slug="post.slug" :focusInput="focusComment" @blur="focusComment = false"
        :replyForUserName="replyForUserName" />
    </div>
  </div>
</template>

<style scoped>
.only-post-layout {
  display: grid;
  grid-template-areas:
    "header"
    "media"
    "footer";
}

.post-comment-layout {
  display: grid;
  grid-template-rows: auto 1fr auto;
  grid-template-columns: 1fr minmax(405px, 1fr);
  grid-template-areas:
    "media header"
    "media comment"
    "media footer";
  border-top-right-radius: 6px;
  border-bottom-right-radius: 6px;
}

.header {
  grid-area: header;
}

.footer {
  grid-area: footer;
  align-self: flex-end;
}

.media {
  grid-area: media;
  background: whitesmoke;
}

.comment {
  grid-area: comment;

}

@media only screen and (max-width: 600px) {
  .post-comment-layout {
    display: grid;
    grid-template-rows: 60px auto auto;
    grid-template-columns: auto;
    grid-template-areas:
      "header"
      "media"
      "footer";
    border-radius: 0;
  }

  .media {
    width: 100%;
  }

  .comment {
    display: none;
  }
}
</style>
<style>
.post-comment-layout .carousel__pagination {
  position: absolute;
  bottom: 0px;
  width: 100%;
}
</style>