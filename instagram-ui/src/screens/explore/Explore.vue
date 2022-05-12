<script setup lang="ts">
import { useApi } from '@/api/composable/use-api';
import { fetchExplorePostsApi } from '@/api/post-api';
import { IRequestParameters } from '@/api/request-features/IRequestParameters';
import { whenScrollAtBottom } from '@/composable/use-scroll-at-bottom';
import LayoutBase from '@/layouts/LayoutBase.vue';
import { IPostThumbnail } from '@/models/IPostThumbnail';
import { onActivated, onDeactivated, onMounted, reactive } from 'vue';
import AppBar from './AppBar.vue';
import Thumbnail from '@/components/Thumbnail.vue';

onMounted(() => {
  console.log('%c onMounted Explore', 'font-weight: 700; color:amber');
})

const collectionExplorePost = reactive(new Array<IPostThumbnail>())
const { data, status, exec } = useApi(fetchExplorePostsApi);
const exploreParameter: IRequestParameters = {
  pageNumber: 1,
  pageSize: 10,
}
let hasNextPage = true;
onMounted(() => {
  collectionExplorePost.length === 0 && fetchExplorePosts();
})

const autoLoadExplorePost = whenScrollAtBottom(fetchExplorePosts);
onActivated(() => autoLoadExplorePost.listener());
onDeactivated(() => autoLoadExplorePost.clearListener());

async function fetchExplorePosts() {
  if (!status.value.Pending && hasNextPage) {

    await exec({...exploreParameter, pageNumber: exploreParameter.pageNumber + 1});
    if (status.value.Success && data.value) {
      collectionExplorePost.push(...data.value.items);
      hasNextPage == data.value.pagination.hasNext;
      exploreParameter.pageNumber++;
    }
  }
}

function isBigger(i: number) {
  let a = 0;
  while (true) {
    let t = 2 + 18 * a;
    if (t == i) return true;
    let k = 10 + 18 * a;
    if (k == i) return true;
    if (t > i && k > i) return false;
    a++;
  }
}

</script>

<template>
  <LayoutBase>
    <template #appBar>
      <AppBar />
    </template>
    <template #body>
      <div class="grid grid-cols-3 gap-0.5">
        <Thumbnail class="item aspect-square bg-red-50" :class="{ 'col-span-2 row-span-2': isBigger(index + 1) }"
          v-for="(item, index) in collectionExplorePost" :key="item.id" :commentCount="item.commentCount"
          :likeCount="item.likeCount" :postType="item.postType" :thumbnail="item.thumbnail" :slug="item.slug" />
      </div>
    </template>
  </LayoutBase>
</template>

<!-- 
<script setup lang="ts">
import { useApi } from '@/api/composable/use-api';
import { fetchExplorePostsApi } from '@/api/post-api';
import { IRequestParameters } from '@/api/request-features/IRequestParameters';
import { whenScrollAtBottom } from '@/composable/use-scroll-at-bottom';
import LayoutBase from '@/layouts/LayoutBase.vue';
import { IPostThumbnail } from '@/models/IPostThumbnail';
import { onActivated, onDeactivated, onMounted, reactive } from 'vue';
import AppBar from './AppBar.vue';
import Thumbnail from '@/components/Thumbnail.vue';
</script>
<template>
  <LayoutBase>
    <template #appBar>
      <AppBar />
    </template>
    <template #body>
  <h1>w</h1>
    </template>
  </LayoutBase>
</template> -->