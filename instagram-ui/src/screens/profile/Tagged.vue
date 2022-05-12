<script setup lang="ts">
import { onMounted} from 'vue';
import IconSpinner from '@/icons/IconSpinner.vue';
import { registerFetchMediasOfUser } from "@/composable/use-profile-manager";
import Thumbnail from '@/components/Thumbnail.vue';

onMounted(() => {
  console.log('%c onMounted Tagged', 'font-weight: 700; color:amber');
})
const {status, collectionMediasOfUser} = registerFetchMediasOfUser()

</script>

<template>

  <div class="px-1">
    <p class="text-gray-500 text-sm text-center py-3 border-b-2">When people tag you in photos, they'll appear here.</p>
    <div class="grid grid-cols-3 gap-0.5 sm:gap-7">
      <Thumbnail v-for="item in collectionMediasOfUser" :thumbnail="item.path" :key="item.id" :slug="item.slug" />
    </div>
  </div>
  <div v-show="status.Pending" class="h-14 flex justify-center items-center">
    <IconSpinner />
  </div>
  <router-view></router-view>
</template>