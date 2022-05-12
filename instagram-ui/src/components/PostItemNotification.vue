<script setup lang="ts">
import PostItem from "@/components/PostItem.vue"
import { IPost } from "@/models/IPost";
import { PropType, DeepReadonly, CSSProperties, ref, watchEffect, watch } from "vue";
import { useElementVisibility } from "@vueuse/core";
const props = defineProps({
  post: { type: Object as PropType<DeepReadonly<IPost>>, required: true, },
  showComment: { type: Boolean, default: true },
  mediaSlideStyle: { type: Object as PropType<CSSProperties> },
  isFollowing: Boolean,
})
const target = ref<HTMLElement|null>(null);
const targetIsVisible = useElementVisibility(target);
watch(() => targetIsVisible.value, () =>{
  console.log(targetIsVisible.value);
})
</script>

<template>
  <div ref="target" class="v-observe-visibility" >
    <PostItem :isFollowing="isFollowing" :showComment="false" :key="post.id"
      :post="post"/>
  </div>
</template>