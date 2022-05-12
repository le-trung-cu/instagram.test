<script setup lang="ts">
import IconHeart from '@/icons/IconHeart.vue';
import IconButtonDropdown from '../base/IconButtonDropdown.vue';
import IconHeartInCircle from '@/icons/IconHeartInCircle.vue';
import { hasNewNotification, setHasNewNotification } from '@/composable/use-notification-manger';
import { useApi } from '@/api/composable/use-api';
import { fetchNotificationApi } from '@/api/notification-api';
import { computed, ref } from '@vue/reactivity';
import BaseIconPending from '../base/BaseIconPending.vue';
import BaseBoxDropdown from '../base/BaseBoxDropdown.vue';
import { PageUrlBuilder } from "@/helpers/pageUrlBuilder";
import { onMounted } from 'vue';

const { data, status, exec } = useApi(fetchNotificationApi)
const hasNewNotificationData = computed(() => data.value?.items.length && data.value?.items.length > 0)
const isOpen = ref(false);

onMounted(async () => {
  // await fetchNotification(()=>{});
})

async function fetchNotification(toggleDropdown: Function) {
  toggleDropdown()
  isOpen.value = !isOpen.value

  console.log('pooooooooooooooo', isOpen.value);
  if (isOpen.value) {
    exec({ pageNumber: 1, pageSize: 10 })
    setHasNewNotification(false)
  }
}

function close(closeDropdown: Function) {
  closeDropdown()
  isOpen.value = false
}
const openBtn = ref<HTMLElement>();

</script>

<template>
  <BaseBoxDropdown :width="450" :offset="60" position="left">
    <template #default="{ closeDropdown, toggleDropdown }">
      <button ref="openBtn" class="flex flex-col items-center relative" @click="fetchNotification(toggleDropdown)"
        @blur="closeDropdown">
        <IconHeart />
        <span v-if="hasNewNotification" class="block w-2 h-2 bg-red-400 rounded-full absolute -bottom-2"></span>
      </button>
    </template>
    <template #dropdown="{ openDropdown, closeDropdown }">
      <div class="h-60 flex flex-col justify-center items-center" @click="openDropdown" @mousedown="openDropdown"
        @mousemove="openDropdown" @mouseup="openBtn?.focus()">
        <BaseIconPending :pending="status.Pending">
          <div v-if="!hasNewNotificationData" class="h-60 flex flex-col justify-center items-center bg-width space-y-4">
            <IconHeartInCircle :size="60" />
            <span class="text-2xl font-normal">Activity On Your Posts</span>
            <span class="text-sm text-gray-800 font-light">When someone likes or comments on one of your posts, you'll
              see
              it here.</span>
          </div>
          <div v-else class="h-60 w-full px-4 bg-width space-y-4 overflow-y-auto">
            <div v-for="item in data?.items">
              <router-link :to="PageUrlBuilder['/:username/'](item.userName)" @click.stop="close(closeDropdown)">
                {{ item.message }}
              </router-link>
            </div>
          </div>
        </BaseIconPending>
      </div>
    </template>
  </BaseBoxDropdown>
</template>