<script setup lang="ts">
import { ref, watch, watchPostEffect } from 'vue';
import { useSearchUser } from '@/composable/use-search-user';
import UserSearchItem from './UserSearchItem.vue';
import IUserSearchResult from '@/models/IUserSearchResult';
import CircleImage from './base/CircleImage.vue';
import { getUser } from '@/composable/use-user';
import { useApi } from '@/api/composable/use-api';
import { createCommentApi } from '@/api/comment-api';
import { IComment } from '@/models/IComment';

const props = defineProps({
  focusInput: { type: Boolean },
  replyForUserName: { type: String, default: '' },
  slug: { type: String, required: true }
})
const emit = defineEmits<{
  (event: 'blur'): void,
  (event: 'commentCreated', comment: IComment): void
}>()


const content = ref('');
const showUserSearch = ref(false);
const refTextComment = ref<HTMLTextAreaElement>();

const { stateSearch } = useSearchUser();
const currentUser = getUser()

watchPostEffect(() => {
  if (props.focusInput) {
    console.log('focus input');
    refTextComment.value?.focus();
  }
  if (props.replyForUserName.length > 0) {
    content.value = props.replyForUserName;
  }
})

watch(content, async () => {
  const tag = regexTagUser(content.value);
  if (tag != '' && stateSearch.searchQuery != tag) {
    showUserSearch.value = true;
    stateSearch.searchQuery = tag;
  } else {
    stateSearch.searchQuery = '';
    showUserSearch.value = false;
  }
})

function setTagUser(tag: string) {
  content.value = content.value.replace(/@\w+/, '@' + tag);
  showUserSearch.value = false;
}

function regexTagUser(text: string): string {
  const regex = /@\w+$/gm;
  const str = text;
  let m;

  while ((m = regex.exec(str)) !== null) {
    // This is necessary to avoid infinite loops with zero-width matches
    if (m.index === regex.lastIndex) {
      regex.lastIndex++;
    }
    return m[0]?.substring(1) ?? '';
  }
  return '';
}

function selectUserTagged(user: IUserSearchResult) {
  content.value = content.value.replace(/@\w+$/gm, '@' + user.userName + ' ');
}

const { status, data, exec } = useApi(createCommentApi);
async function createComment() {
  console.log(content.value);
  
  const comment = {
    content: content.value,
  }
  await exec(props.slug, comment);
  if(status.value.Success){
    emit('commentCreated', data.value!);
    content.value = ''
  }
}

</script>



<template>
  <div class="relative">
    <div class="h-16 py-2 flex justify-center items-center bg-[#efefef]">
      <CircleImage :src="currentUser.avatar || '/icons/none-profile.jpg'" :size="30" class="mx-4" />
      <form class="h-10 border-gray-300 border rounded-full flex-grow mr-4 bg-white px-5 flex items-center relative"
        @submit.prevent="createComment">
        <textarea ref="refTextComment" v-model="content" rows="1"
          class="focus:outline-none overflow-hidden resize-none h-5 w-full placeholder:text-gray-300"
          placeholder="Add a comment...">{{ content }}</textarea>
        <button type="submit" class="leading-none font-medium text-sm absolute right-5">Post</button>
      </form>
    </div>
    <div v-if="showUserSearch" class="absolute overflow-auto ml-9 h-52 w-60 bg-white bottom-full shadow-inner border">
      <UserSearchItem v-for="user in stateSearch.userSearchResults" :user="user" @select="selectUserTagged"
        :avatarSize="30" />
    </div>
  </div>
</template>

<style scoped>
textarea:placeholder-shown+button {
  color: #b8e1fc;
}

button {
  color: #4c9ef6;
}
</style>