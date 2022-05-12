<script setup lang="ts">
import { onMounted, PropType, ref} from 'vue';

const props = defineProps({
  width: { type: Number, default: 0 },
  position: String as PropType<'left' | 'right' | 'center'>,
  offset: { type: Number, default: 0 },
  backdrop: { type: Boolean, default: true },
  open: Boolean
})

const title = ref<HTMLElement>();
const titleWidth = ref(0);
const isOpen = ref(props.open);

onMounted(() => {
  titleWidth.value = title.value?.clientWidth ?? 0;
})

const styleBoxOuter = () => {

  let style: any = {
    width: props.width + 'px',
  }
  // return style;
  switch (props.position) {

    case 'right':
      style = {
        ...style,
        transform: `translateX(-${props.offset}px)`
      }
      break;
    case 'left':
      style = {
        ...style,
        transform: `translateX(-${props.width - titleWidth.value - props.offset}px)`
      }
      break;
    case 'center':
      style = {
        ...style,
        left: '50%',
        transform: `translateX(-50%)`
      }
      break;
  }
  return style;
}

const styleArrow = () => {
  let style: any = {
    left: '50%',
  };

  switch (props.position) {
    case 'right':
      style = {
        transform: `translateX(${props.offset + 3}px) translateY(-50%) rotate(45deg)`
      }
      break;
    case 'left':
      style = {
        right: 0,
        transform: `translateX(-${props.offset - 3}px) translateX(-50%) translateY(-50%) rotate(45deg)`
      }
      break;
    case 'center':
      style = {
        ...style,
        transform: `translateX(-50%) translateY(-50%) rotate(45deg)`
      }
      break
  }
  return style;
}

let idTiming: number | undefined = undefined;
function openDropdown() {
  if(idTiming != undefined){
    console.log('clear time out in open', idTiming);
    clearTimeout(idTiming);
    idTiming = undefined;
  }
  if(!isOpen.value){
    console.log('open');
    
    isOpen.value = true;
  }
}
function closeDropdown(){
  if(idTiming != undefined){
    clearTimeout(idTiming);
    console.log('clear time out in close', idTiming);
  }
  
  idTiming = setTimeout(() => {
    console.log('close');
    
    isOpen.value = false;
    idTiming = undefined;
    }, 300);
}

function toggleDropdown(){
  if(isOpen.value)
    closeDropdown();
  else
    openDropdown();
}
</script>

<template>
  <div :style="{ zIndex: isOpen ? 50 : 0 }">
    <!-- <div v-if="open && backdrop" class="fixed inset-0" @click.self="$emit('close')"></div> -->
    <div ref="title" class="cursor-pointer select-none relative flex justify-center">
      <slot v-bind="{  openDropdown, closeDropdown, toggleDropdown, isOpen }"></slot>
    </div>
    <div :style="{ ...styleBoxOuter() }" class="absolute mt-4">
      <Transition>
        <div v-if="isOpen" class="rounded-sm bg-white box-shadow">
        <div
          style="transform-origin: center center;"
          :style="{ ...styleArrow() }"
          class="inline-block absolute h-4 w-4 bg-white box-shadow"></div>
          <div class="shadow rounded-sm overflow-hidden relative">
            <slot name="dropdown" v-bind="{ openDropdown, closeDropdown, toggleDropdown, isOpen }"></slot>
          </div>
        </div>
      </Transition>
    </div>
  </div>
</template>

<style scoped>
.box-shadow {
  box-shadow: -3px -3px 4px #00000043;
}
.v-enter-active,
.v-leave-active {
  transition: all .2s ease;
}

.v-enter-from,
.v-leave-to {
  opacity: .2;
  transform: translateY(20px);
}
</style>