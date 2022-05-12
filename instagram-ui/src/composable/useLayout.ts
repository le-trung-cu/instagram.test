import { mobileCheck } from "@/helpers";
import { ref } from "vue"

export const LAYOUTS = {
  mobile: Symbol('mobile'),
  desktop: Symbol('desktop'),
}

const layout = ref(mobileCheck()? LAYOUTS.mobile : LAYOUTS.desktop);

export function useLayout(){
  return {
    layout,
    setLayout,
    LAYOUTS,
  }
}

function setLayout(layoutType: symbol){
  layout.value = layoutType
}