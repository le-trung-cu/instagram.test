<script setup lang="ts">

import ago from 'ts-ago';
import { IPost } from '@/models/IPost';
import IconLike from '../icons/IconHeart.vue';
import IconComment from '../icons/IconComment.vue';
import IconSave from '../icons/IconSave.vue';
import { inject, PropType } from 'vue';
import IconDirect from '@/icons/IconDirect.vue';
import { useLikePost, useSavePost } from "@/composable/use-post-store";
import { PageUrlBuilder } from "@/helpers/pageUrlBuilder";
import BaseIconPending from './base/BaseIconPending.vue';

type PostActionPropType = Required<Pick<IPost, 'slug' | 'liked' | 'saved' | 'commentCount' | 'createdAt' | 'likeCount'>>;
const props = defineProps({
  post: { type: Object as PropType<PostActionPropType>, required: true },
})
const { likePost, likePostStatus } = useLikePost();
const { savePost, savePostStatus } = useSavePost();
const clickIconComment = inject('clickIconComment') as Function;
defineEmits<{
  (event: 'iconCommentClick', e: MouseEvent): void
}>();

</script>

<template>
  <div>
    <div class="flex justify-between items-center pl-4 pr-2">
      <div class="flex space-x-3 items-center">
        <BaseIconPending :pending="likePostStatus['Pending']">
          <IconLike @click="likePost(post.slug, !post.liked)" :liked="post.liked" />
        </BaseIconPending>
        <router-link :to="PageUrlBuilder['/p/:slug/comments'](post.slug)">
          <IconComment @click="clickIconComment" />
        </router-link>
        <IconDirect />
      </div>
      <BaseIconPending :pending="savePostStatus['Pending']">
        <IconSave @click="savePost(post.slug, !post.saved)" :saved="post.saved" />
      </BaseIconPending>
    </div>
    <div class="text-left pl-4 pb-2">
      <div class="text-black text-sm font-bold">{{ post.likeCount }} likes</div>
      <p class="text-sm font-thin text-gray-800">
        <span class="text-sm font-bold">erin_kay</span> Lorem ipsum, dolor sit amet consectetur adipisicing elit
      </p>
      <router-link :to="PageUrlBuilder['/p/:slug/comments'](post.slug)" class="text-gray-500 font-thin text-base cursor-pointer">
        <button @click="(e) =>clickIconComment">View all {{ post.commentCount }} comments</button>
      </router-link>
      <div class="text-gray-500 text-[10px] font-thin uppercase">{{ ago(post.createdAt) }}</div>
    </div>
  </div>
</template>