<script setup lang="ts">
import { computed, DeepReadonly, onActivated, onDeactivated, onMounted, onUnmounted, PropType, ref } from 'vue';
import { useMousemove, Distance } from '@/composable/use-mousemove';
import { ResizeObserver } from "resize-observer";

let props = defineProps({
  itemCountShow: { type: Number, default: 1 },
  items: { type: Array as PropType<DeepReadonly<any[]>>, required: true },
  aspectRatio: String,
  offsetNavigation: { type: Number, default: 4 }
})

let outer = ref<HTMLElement>();
let inner = ref<HTMLElement>();
const { onmousedown, ontouchstart } = useMousemove({
  onmousedown: handleMousedown,
  onmousemove: handleMousemove,
  onmouseup: handleMouseup,
  ontouchstart: handleTouchstart,
  ontouchmove: handleTouchMove,
  ontouchend: handleTouchend,
})
const itemWidth = ref(0);
const translateX = ref(0);
const currentPage = ref(1);
const pageCount = (Math.ceil(props.items.length / props.itemCountShow));
let originalTranslateX = 0;

const itemStyle = computed(() => {
  return { width: itemWidth.value + 'px', }
})

const resizeObserver = new ResizeObserver(handleResize);

const innerStyle = computed(() => {
  return { transform: `translateX(${translateX.value}px)` };
})


onMounted(() => {
  handleResize();
  resizeObserver.observe(outer.value!);
})

onActivated(() => {
  // window.addEventListener('resize', handleResize);
})

onUnmounted(() => {
  resizeObserver.disconnect();
  // window.removeEventListener('resize', handleResize);
})

onDeactivated(() => {
  //window.removeEventListener('resize', handleResize);
})

function handleResize() {
  if (outer.value?.clientWidth) {
    itemWidth.value = outer.value.clientWidth / props.itemCountShow;
  }
}


function handleMousedown(payload: MouseEvent) {
  originalTranslateX = translateX.value;
}

function handleMousemove(payload: MouseEvent, distance: Distance) {
  translateX.value = originalTranslateX + distance.x;

  if (translateX.value > 0) {
    translateX.value = 0;
  }
  if (translateX.value < outer.value!.clientWidth - inner.value!.clientWidth) {
    translateX.value = outer.value!.clientWidth - inner.value!.clientWidth;
  }
}

function handleMouseup(payload: MouseEvent, distance: Distance) {
  const offset = translateX.value - originalTranslateX;
  const currentTranslate = translateX.value;
  const absOffset = Math.abs(offset);
  if (Math.abs(distance.x) < 50) {
    if (offset > 0) {
      animate({
        timing: easeInSine, draw: (value) => {
          translateX.value = currentTranslate - absOffset * value;
        }, duration: 50
      });
    }
    if (offset < 0) {
      animate({
        timing: easeInSine, draw: (value) => {
          translateX.value = currentTranslate + absOffset * value;
        }, duration: 50
      });
    }
  } else {
    if (offset > 0) {
      // translateX.value += itemWidth.value * props.itemCountShow - absOffset;
      animate({
        timing: easeInSine, draw: (value) => {
          translateX.value = currentTranslate + (itemWidth.value * props.itemCountShow - absOffset) * value;
          if (translateX.value > 0) {
            translateX.value = 0;
          }
          if (translateX.value < outer.value!.clientWidth - inner.value!.clientWidth) {
            translateX.value = outer.value!.clientWidth - inner.value!.clientWidth
          }
        }, duration: 250
      });
      if (currentPage.value > 1)
        currentPage.value--;
    } else if (offset < 0) {
      // translateX.value -= itemWidth.value * props.itemCountShow - absOffset;
      animate({
        timing: easeInSine, draw: (value) => {
          translateX.value = currentTranslate - (itemWidth.value * props.itemCountShow - absOffset) * value;
          if (translateX.value > 0) {
            translateX.value = 0;
          }
          if (translateX.value < outer.value!.clientWidth - inner.value!.clientWidth) {
            translateX.value = outer.value!.clientWidth - inner.value!.clientWidth
          }
        }, duration: 250
      });
      if (currentPage.value < pageCount)
        currentPage.value++;
    }

  }
}

function handleTouchstart(payload: TouchEvent) {
  originalTranslateX = translateX.value;
}

function handleTouchMove(payload: TouchEvent, distance: Distance) {
  translateX.value = originalTranslateX + distance.x;

  if (translateX.value > 0) {
    translateX.value = 0;
  }
  if (translateX.value < outer.value!.clientWidth - inner.value!.clientWidth) {
    translateX.value = outer.value!.clientWidth - inner.value!.clientWidth;
  }
}

function handleTouchend(payload: MouseEvent | TouchEvent, distance: { x: number, y: number }) {
  const offset = translateX.value - originalTranslateX;
  const currentTranslate = translateX.value;
  const absOffset = Math.abs(offset);
  if (Math.abs(distance.x) < 50) {
    if (offset > 0) {
      animate({
        timing: easeInSine, draw: (value) => {
          translateX.value = currentTranslate - absOffset * value;
        }, duration: 50
      });
    }
    if (offset < 0) {
      animate({
        timing: easeInSine, draw: (value) => {
          translateX.value = currentTranslate + absOffset * value;
        }, duration: 50
      });
    }
  } else {
    if (offset > 0) {
      // translateX.value += itemWidth.value * props.itemCountShow - absOffset;
      animate({
        timing: easeInSine, draw: (value) => {
          translateX.value = currentTranslate + (itemWidth.value * props.itemCountShow - absOffset) * value;
          if (translateX.value > 0) {
            translateX.value = 0;
          }
          if (translateX.value < outer.value!.clientWidth - inner.value!.clientWidth) {
            translateX.value = outer.value!.clientWidth - inner.value!.clientWidth
          }
        }, duration: 250
      });
      if (currentPage.value > 1)
        currentPage.value--;
    } else if (offset < 0) {
      // translateX.value -= itemWidth.value * props.itemCountShow - absOffset;
      animate({
        timing: easeInSine, draw: (value) => {
          translateX.value = currentTranslate - (itemWidth.value * props.itemCountShow - absOffset) * value;
          if (translateX.value > 0) {
            translateX.value = 0;
          }
          if (translateX.value < outer.value!.clientWidth - inner.value!.clientWidth) {
            translateX.value = outer.value!.clientWidth - inner.value!.clientWidth
          }
        }, duration: 250
      });
      if (currentPage.value < pageCount)
        currentPage.value++;
    }

  }
}

function animate(options: { timing: (timeFraction: number) => number, draw: (progress: number) => void, duration: number }) {

  let start = performance.now();

  requestAnimationFrame(function animate(time) {
    // timeFraction goes from 0 to 1
    let timeFraction = (time - start) / options.duration;
    if (timeFraction > 1) timeFraction = 1;

    // calculate the current animation state
    let progress = options.timing(timeFraction)

    options.draw(progress); // draw it

    if (timeFraction < 1) {
      requestAnimationFrame(animate);
    }
  });
}
function linear(timeFraction: number): number {
  return timeFraction;
}
function easeInSine(x: number): number {
  return 1 - Math.cos((x * Math.PI) / 2);
}

</script>

<template>
  <div class="relative max-w-full" ref="outer">
    <div
      class="outer bg-slate-500 w-full overflow-hidden"
      :style="{ aspectRatio: aspectRatio }"
      @mousedown="onmousedown"
      @touchstart="ontouchstart"
    >
      <div ref="inner" class="w-fit items-center flex h-full select-none" :style="innerStyle">
        <div :style="itemStyle" v-for="item in items" :key="item.id">
          <slot name="item" v-bind="item"></slot>
        </div>
      </div>
    </div>
    <div class="space-x-1 absolute left-1/2 -translate-x-1/2" :style="{bottom: offsetNavigation+'px'}">
      <span
        class="inline-block w-2 h-2 rounded-full transition-all text-white text-xs"
        :class="[item == currentPage ? 'bg-blue-500' : 'bg-gray-300']"
        v-for="item in pageCount"
        :key="item"
      ></span>
    </div>
  </div>
</template>