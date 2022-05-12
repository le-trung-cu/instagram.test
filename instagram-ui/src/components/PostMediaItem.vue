<script setup lang="ts">
import { IMediaItem } from "@/models/IPost";
import { PropType, ref } from "vue";
import IconUserTag from '../icons/IconUserTag.vue';

defineProps({
  mediaItem: { type: Object as PropType<IMediaItem>, required: true },
  showTag: { type: Boolean, default: false }
})

defineEmits<{
  (event: 'mediaLoad', width: number, height: number): void,
  (event: 'toggleShowTag'): void
}>();
const media = ref<HTMLImageElement | HTMLVideoElement>();
</script>
<template>
  <div>
    <img ref="media" class="w-full h-full" :src="mediaItem.path" @load="$emit('mediaLoad', media?.width!, media?.height!)"/>
    <div class="absolute inset-0">
      <div class="absolute inset-0" v-if="mediaItem.mediaType === 'PhotoItem'">
        <router-link :to="`/${tag.userName}`" v-for="tag in mediaItem.userTags" class="tooltip"
          :style="{ top: tag.top! * 100 + '%', left: tag.left! * 100 + '%', visibility: showTag ? 'visible' : 'hidden' }">
          {{ tag.userName }}
        </router-link>
      </div>
      <icon-user-tag v-if="mediaItem.userTags.length > 0" class="bottom-5 left-5 absolute"
        @click="$emit('toggleShowTag')" />
    </div>
  </div>
  <!-- <img
    v-if="mediaType == 'PhotoItem'"
    ref="media"
    class="select-none w-full"
    draggable="false"
    :src="path"
    @load="$emit('mediaLoad', media?.width!, media?.height!)"
  />
  <video
    v-if="mediaType == 'VideoItem'"
    ref="media"
    :src="path"
    @load="$emit('mediaLoad', media?.width!, media?.height!)"
  ></video> -->
</template>


<style scoped>.tooltip {
  visibility: hidden;
  /* width: 120px; */
  background-color: rgba(0, 0, 0, 0.721);
  color: rgb(208, 205, 205);
  text-align: center;
  padding: 5px 12px;
  border-radius: 6px;
  position: absolute;
  transform: translateY(calc(-100% - 5px)) translateX(-50%);
  z-index: 1;
}

.tooltip::after {
  content: " ";
  position: absolute;
  top: 100%;
  /* At the bottom of the tooltip */
  left: 50%;
  margin-left: -5px;
  border-width: 5px;
  border-style: solid;
  border-color: black transparent transparent transparent;
}</style>
