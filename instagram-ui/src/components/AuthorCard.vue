
<script setup lang="ts">
import IconMoreOption from '../icons/IconMoreOption.vue'
import CircleImage from './base/CircleImage.vue'
import BaseModal from './base/BaseModal.vue'
import { inject, PropType, ref } from 'vue'
import { IPost } from '@/models/IPost'
import { useBoolean } from '@/composable/use-toggle'
import UserTile from './UserTile.vue'
import { useRouter } from 'vue-router'
import { PageUrlBuilder } from '@/helpers/pageUrlBuilder'
import FollowButton from './FollowButton.vue'
import { getUser } from "@/composable/use-user";

const currentUser =  getUser();
type Author = Pick<IPost, 'ownerId' | 'avatar' | 'userName' | 'title'>
const props = defineProps({
  author: { type: Object as PropType<Author>, required: true },
  slug: { type: String, required: true },
  isFollowing: { type: Boolean }
})

const emit = defineEmits<{
  (event: 'follow', followed: boolean): void,
}>()
const closePostModal = inject('closePostModal') as Function

const { state: isOpenModalFollow } = useBoolean()
const { state: isOpenMoreOption } = useBoolean()

function followButtonClick() {
  if (props.isFollowing) {
    isOpenModalFollow.value = true
  } else {
    emit('follow', true)
  }
}
const router = useRouter()

function goToPostPage() {
  isOpenMoreOption.value = false
  closePostModal(PageUrlBuilder['/p/:slug'](props.slug))
  router.push(PageUrlBuilder['/p/:slug'](props.slug))
}

</script>

<template>
  <div class="pl-4 py-4 pr-1">
    <UserTile :size="40" :avatar="author.avatar" :userName="author.userName" :fullName="author.title">
      <template  v-if="currentUser.id !== author.ownerId" #additional>
        <span class="mx-1 text-gray-400">&#9830;</span>
        <FollowButton variant="text-btn" :isFollowing="isFollowing" :userId="author.ownerId" :userName="author.userName"
        :avatar="author.avatar"
          />
      </template>
      <template #trailing>
        <IconMoreOption @click="isOpenMoreOption = true" />
      </template>
    </UserTile>
    <BaseModal :open="isOpenModalFollow" @closeButtonClick="isOpenModalFollow = false">
      <template #content>
        <div class="flex flex-col rounded-md items-center w-full max-w-xs bg-white py-3 px-3 space-y-2">
          <CircleImage :src="author.avatar" :size="100" class="mb-5" />
          <span>Unfollow @piscocat?</span>
          <button @click="isOpenModalFollow = false; $emit('follow', false)"
            class="text-red-600 font-semibold">Unfollow</button>
          <button @click="isOpenModalFollow = false" class="font-semibold">Cancel</button>
        </div>
      </template>
    </BaseModal>
    <BaseModal :open="isOpenMoreOption" @closeButtonClick="isOpenMoreOption = false">
      <template #content>
        <ol class="bg-white w-full max-w-sm rounded-lg overflow-hidden">
          <li
            class="cursor-pointer shadow-sm py-2 w-full flex justify-center items-center font-medium hover:bg-gray-100 transition-colors">
            <button @click="goToPostPage" class="w-full">Go to Post Page</button>
          </li>
          <li
            class="cursor-pointer shadow-sm py-2 w-full flex justify-center items-center font-medium hover:bg-gray-100 transition-colors">
            <button @click="isOpenMoreOption = false; isOpenModalFollow = true" class="w-full">Unfollow</button>
          </li>
        </ol>
      </template>
    </BaseModal>
  </div>

</template>
