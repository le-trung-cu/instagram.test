import { readonly, ref } from "vue";

const _hasNewNotification = ref(false)

export const hasNewNotification = readonly(_hasNewNotification)

export function setHasNewNotification(hasNew: boolean){
    _hasNewNotification.value = hasNew
}