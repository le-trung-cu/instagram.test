<script setup lang="ts">
import { PageUrlBuilder } from "@/helpers/pageUrlBuilder";
import IconCarousel from '@/icons/IconCarousel.vue';
import IconHeart from '@/icons/IconHeart.vue';
import IconComment from '@/icons/IconComment.vue';
import { inject } from "vue";

const props = defineProps({
  slug: String,
  thumbnail: String,
  postType: String,
  commentCount: Number,
  likeCount: Number
})

const openPostModal = inject('openPostModal') as Function;
</script>

<template>
  <div class="bg-slate-500 aspect-square overflow-hidden relative">
    <router-link :to="PageUrlBuilder['/p/:slug'](slug!)">
      <img class="object-cover w-full h-full" :src="thumbnail" />
      <div
        class="absolute inset-0 opacity-0 hover:opacity-100 bg-gray-900 bg-opacity-40 flex justify-center items-center"
        @click="openPostModal(slug, $event)">
        <span class="text-gray-100 font-semibold flex items-center mr-5">
          <IconHeart class="fill-current mr-1" /> {{ likeCount}}
        </span>
        <span class="text-gray-100 font-semibold flex items-center">
          <IconComment class="fill-current mr-1" /> {{ commentCount }}
        </span>
      </div>
      <IconCarousel class="absolute top-2 right-2 inline-block"></IconCarousel>
    </router-link>
  </div>
</template>