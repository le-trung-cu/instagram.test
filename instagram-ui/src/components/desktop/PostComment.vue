<script setup lang="ts">
import { onMounted, onUnmounted, PropType, ref } from 'vue';
import { ResizeObserver } from 'resize-observer';
import Carousel from '../base/Carousel.vue';
import PostMediaItem from '../PostMediaItem.vue';
import PostItem from "@/components/PostItem.vue";

const props = defineProps({
  post: { type: Object, required: true },
})

const refBox = ref<HTMLElement>();

const styleDetailBox = ref();
const showComment = ref(false);
let aspectRatioCarousel = {
  aspect: 1,
  w: 1,
  h: 1,
};



const resizeObserver = new ResizeObserver(() => {
  styleDetailBox.value = getStyle();
});

onMounted(() => {
  resizeObserver.observe(refBox.value!);
})
onUnmounted(() => {
  resizeObserver.disconnect();
})

function calculateAspectRatioCarousel(w: number, h: number) {
  if (aspectRatioCarousel.aspect > w / h) {
    aspectRatioCarousel = {
      aspect: w / h,
      w,
      h,
    }
    styleDetailBox.value = getStyle();
  }
}

function getStyle() {
  const widthRefBox = refBox.value?.clientWidth!;
  const heightRefBox = refBox.value?.clientHeight!;
  const { aspect } = aspectRatioCarousel;
  const w = aspect * heightRefBox;
  let style;
  if (widthRefBox < 400) {
    showComment.value = false;
    style = {
      width: '100%',
      height: widthRefBox / aspect < heightRefBox ? '100%' : widthRefBox / aspect + 'px'
    }
  }
  else if (widthRefBox < 700) {
    showComment.value = false;
    style = {
      padding: '50px',
      width: '100%',
      height: widthRefBox / aspect < heightRefBox ? '100%' : widthRefBox / aspect + 'px'
    }
  }
  else if (w > widthRefBox / 2) {
    showComment.value = true;
    style = {
      width: '50%',
      height: widthRefBox / aspect < heightRefBox ? '100%' : widthRefBox / aspect + 'px'
    }
  } else {
    showComment.value = true;
    style = {
      width: w + 'px',
      height: '100%'
    }
  }
  return style;
}
</script>

<template>
  <div ref="refBox" class="w-full h-full">
    <div v-if="fetchPostBySlugStatus.Success && post" class="flex justify-center">
      <div v-if="showComment" class="bg-black flex items-center max-h-full" :style="styleDetailBox">
        <div class="w-full max-h-full">
          <Carousel :items="post.mediaItems" :offset-navigation="20">
            <template #item="{ path, mediaType }">
              <PostMediaItem
                :path="path"
                :media-type="mediaType"
                @media-load="calculateAspectRatioCarousel"
              />
            </template>
          </Carousel>
        </div>
      </div>
      <div v-else class="flex items-center max-h-full" :style="styleDetailBox">
        <div class="w-full max-h-full">
          <PostItem :post="post"></PostItem>
        </div>
      </div>
      <div
        v-if="showComment"
        class="min-w-[300px] desktop:mix-w-[405px] max-w-[500px] flex-grow h-full bg-white rounded-r-md"
      >
        <div></div>
      </div>
    </div>
    <div v-if="fetchPostBySlugStatus.Pending" class="w-10/12 m-auto h-full">
      <div class=" flex h-full justify-center">
        <div class="w-1/2 bg-white h-full">
          <div class="animate-pulse bg-slate-200  h-full"></div>
        </div>
        <div class="w-1/2 bg-white rounded-r h-full">
          <div class="border shadow  rounded-tr p-4 w-full mx-auto">
            <div class="animate-pulse flex space-x-4">
              <div class="rounded-full bg-slate-200 h-10 w-10"></div>
              <div class="flex-1 space-y-6 py-1">
                <div class="h-2 bg-slate-200 rounded"></div>
                <div class="space-y-3">
                  <div class="grid grid-cols-3 gap-4">
                    <div class="h-2 bg-slate-200 col-span-2"></div>
                    <div class="h-2 bg-slate-200 col-span-1"></div>
                  </div>
                  <div class="h-2 bg-slate-200"></div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>