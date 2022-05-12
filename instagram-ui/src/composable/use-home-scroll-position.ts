import { onMounted, onBeforeUnmount, ref, readonly } from "vue";
import { useRoute } from "vue-router";

let _homeScrollPosition = ref(0);

function saveHomeScrollPosition() {

  const current = window.scrollY;
  _homeScrollPosition.value = current;

}

export const homeScrollPosition = readonly(_homeScrollPosition);



export function useHomeScrollPosition() {
  const route = useRoute();
  onMounted(() => {
    window.scroll(0, _homeScrollPosition.value)
    if (route.name === 'Home') {
      setTimeout(() => {
        document.addEventListener('scroll', saveHomeScrollPosition, true)
      },310)
    }
  })
  
  onBeforeUnmount(() => {
    document.removeEventListener('scroll', saveHomeScrollPosition, true);
  })
}