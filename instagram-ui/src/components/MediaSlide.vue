<script>
// If you are using PurgeCSS, make sure to whitelist the carousel CSS classes
import 'vue3-carousel/dist/carousel.css';
import { Carousel, Slide, Pagination, Navigation } from 'vue3-carousel';
import PostMediaItem from './PostMediaItem.vue';
import IconHeart from '@/icons/IconHeart.vue';
import { aspectRatioHelper } from '@/helpers';


export default {
  components: { Carousel, Slide, Pagination, Navigation, PostMediaItem, IconHeart },
  props: {
    medias: Array
  },
  emits: {
    minAspectRatioChange(width, height) {
      return width != 0 && height != 0;
    },
    maxAspectRatioChange(width, height) {
      return width != 0 && height != 0;
    },
  },
  data() {
    return {
      showTag: false,
      minAspect: {
        w: 0,
        h: 1,
      },
      maxAspect: {
        w: 0,
        h: 1
      },
    }
  },

  methods: {
    mediaItemLoadListener(width, height) {
      console.log('load item', width, height, this.minAspect, this.maxAspect);
      if (width == 0 || height == 0)
        return;
      if (this.minAspect.w == 0 || this.maxAspect.w == 0) {
        if (this.minAspect.w == 0) {
          this.minAspect = { w: width, h: height }
          this.$emit('minAspectRatioChange', this.minAspect.w, this.minAspect.h);
        }
        if (this.maxAspect.w == 0) {
          this.maxAspect = { w: width, h: height }
          this.$emit('maxAspectRatioChange', this.maxAspect.w, this.maxAspect.h);
        }
        return;
      }

      if (this.minAspect.w / this.minAspect.h > width / height) {
        const [w, h] = aspectRatioHelper(width, height);
        this.minAspect = {w, h};
        this.$emit('minAspectRatioChange', this.minAspect.w, this.minAspect.h);
      }

      if (this.maxAspect.w / this.maxAspect.h < width / height) {
        const [w, h] = aspectRatioHelper(width, height);
        this.maxAspect = {w, h};
        this.$emit('maxAspectRatioChange', this.maxAspect.w, this.maxAspect.h);
      }
    }
  }
}

</script>

<template>
  <carousel :items-to-show="1">
    <slide v-for="item in medias" :key="item.id">
      <post-media-item :mediaItem="item" :showTag="showTag" @toggleShowTag="showTag = !showTag"
         />
    </slide>
    <template #addons="{ slidesCount }">
      <navigation v-if="slidesCount > 1" />
      <pagination v-if="slidesCount > 1" />
    </template>
  </carousel>
</template>
