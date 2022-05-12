<script setup lang="ts">
import { useFollowUser } from "@/composable/use-relationship-manager";
import { PropType, ref, computed } from "vue";
import BaseModal from "./base/BaseModal.vue";
import CircleImage from "./base/CircleImage.vue";

const props = defineProps({
  userId: { type: String, required: true },
  userName: { type: String, required: true },
  avatar: { type: String, required: true },
  variant: { type: String as PropType<'text-btn' | 'bg-btn'>, default: 'text-btn' },
  isFollowing: Boolean,
})

const isOpenUnFollowModal = ref(false);

const { followUser, status } = useFollowUser();

function btnHandleFollow() {
  if (!props.isFollowing) {
    followUser(props.userName, props.userId, true)
  } else {
    isOpenUnFollowModal.value = true;
  }
}

function btnHandleUnFollow() {
  followUser(props.userName, props.userId, false);
  isOpenUnFollowModal.value = false;
}

const btnClass = computed(() => {
  if (props.variant == 'text-btn') {
    if (!props.isFollowing)
      return "text-blue-500 font-medium text-xs ml-auto";
    return "text-gray-500 font-medium text-xs ml-auto";
  }
  if (props.isFollowing)
    return "bg-gray-400 text-white font-semibold px-3 py-1 rounded-md"
  return "bg-blue-400 text-white font-semibold px-3 py-1 rounded-md"
});
</script>

<template>
  <button :class="btnClass" @click.prevent="btnHandleFollow" v-bind="$attrs">
    <slot>{{ isFollowing ? 'Following' : 'Follow' }}</slot>
  </button>
  <BaseModal v-if="isFollowing" :open="isOpenUnFollowModal" @closeButtonClick="isOpenUnFollowModal = false"
    @backdropClick="isOpenUnFollowModal = false">
    <template #content>
      <div class="flex flex-col rounded-md items-center w-full max-w-xs bg-white py-3 px-3 space-y-2">
        <CircleImage :src="avatar" :size="100" class="mb-5" />
        <span>Unfollow @{{userName}}?</span>
        <button class="text-red-600 font-semibold" @click="btnHandleUnFollow">Unfollow</button>
        <button class="font-semibold" @click="isOpenUnFollowModal = false">Cancel</button>
      </div>
    </template>
  </BaseModal>
</template>