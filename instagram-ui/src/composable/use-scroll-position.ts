import { onUnmounted, ref } from "vue"

export const useScrollPosition = () => {
  const scrollY = ref(window.scrollY);
  const onScroll = () => {
    scrollY.value = window.scrollY;
  }

  window.addEventListener('scroll', onScroll, false);
  onUnmounted(() => {
    window.removeEventListener('scroll', onScroll, false);
  });
  return scrollY;
}