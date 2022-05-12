<script setup lang="ts">
import UserSearchResult from "@/models/IUserSearchResult";
import { PropType } from "vue";
import IconClose from "@/icons/IconClose.vue";
import CircleImage from "./base/CircleImage.vue";
import { PageUrlBuilder } from "@/helpers/pageUrlBuilder";
defineProps({
  user: { type: Object as PropType<UserSearchResult>, required: true },
  canDelete: { type: Boolean, default: false },
  avatarSize: {type: Number, default: 44}
})
defineEmits<{
  (event: 'delete', user: UserSearchResult): void,
  (event: 'select', user: UserSearchResult): void,
}>()
</script>

<template>
  <div class="flex items-center py-1 px-4 hover:bg-gray-100 transition-colors cursor-pointer"
    @click="$emit('select', user)">
    <CircleImage :size="avatarSize" class="mr-2" :src="user.avatar" />
    <div class="flex-grow">
      <h6 class="font-semibold text-sm">{{ user.userName }}</h6>
      <p class="font-light text-sm text-gray-400">{{ user.title }}</p>
    </div>
    <IconClose v-if="canDelete" class="text-gray-400" @click.stop="$emit('delete', user)" />
  </div>
</template>