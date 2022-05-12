<script setup lang="ts">
import { defineProps } from "vue";
import IconHeart from './IconHeart.vue';
defineProps({
	size: {type:Number, default: 24}
})
</script>
<template>
	<span class="inline-flex justify-center items-center rounded-full border-2 border-gray-400" :style="{padding: `${size*0.2}px`}">
		<IconHeart :size="size - size * 0.2" class="text-gray-300"/>
	</span>
</template>