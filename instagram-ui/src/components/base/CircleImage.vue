<template>
  <div
    v-if="hasBorder"
    class="rounded-full flex-shrink-0 shadow shadow-black  overflow-hidden mr-2 bg-gradient-to-r from-purple-500 to-orange-600 flex justify-center items-center"
    :style="{ width: size + 'px', height: size + 'px' }"
  >
    <div
      :style="{ width: size - 4 + 'px', height: size - 4 + 'px' }"
      class="rounded-full border-2 border-white overflow-hidden"
    >
      <img class="rounded-full" draggable="false" :src="src || '/icons/none-profile.jpg'" />
    </div>
  </div>
  <div
    v-else
    :style="{ width: size + 'px', height: size + 'px' }"
    class="rounded-full flex-shrink-0 border-2 border-white overflow-hidden img-shadow"
  >
    <img class="rounded-full" draggable="false" :src="src || '/icons/none-profile.jpg'" />
  </div>
</template>

<script setup lang="ts">
const props = defineProps({
  size: { type: Number, default: 30 },
  hasBorder: { type: Boolean, default: false },
  src: { type: String , required: true}
});
</script>

<style scoped>
.img-shadow{
  box-shadow: 0 0 2px;
}
</style>