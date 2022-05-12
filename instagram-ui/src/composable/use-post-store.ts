
import { useApi } from "@/api/composable/use-api";
import { fetchFeedPostsHomePageApi, fetchPostBySlugApi, likePostApi, savePostApi } from "@/api/post-api";
import { IPostParameter } from "@/api/request-features/IPostParameter";
import { WithFollowStatus } from "@/helpers";
import { IPost } from "@/models/IPost";
import { computed, onActivated, onDeactivated, onMounted, reactive, readonly, ref } from "vue";
import { fetchRelationshipStatus, mainUserFollow } from "./use-relationship-manager";
import { whenScrollAtBottom } from "./use-scroll-at-bottom";

export class PostKeyNotFoundException extends Error {
  constructor(slug: string) {
    super(`PostKey with slug : ${slug} not found`);
  }
}

const _mapPostHomePage = reactive(new Map<IPost['slug'], IPost>());
const _arrayPostKeyHomePage = reactive(new Array<IPost['slug']>());
const _mapPostFeedPage = reactive(new Map<IPost['slug'], IPost>());
const _arrayPostFeedPage = reactive(new Array<IPost['slug']>());

const _currentPost = ref<IPost>();

export const setCurrentPost = (post: IPost) => {
  _currentPost.value = post;
};

export const currentPost = computed<WithFollowStatus<IPost>>(() => ({
  data: _currentPost.value,
  followStatus: _currentPost.value && mainUserFollow.get(_currentPost.value?.ownerId),
}));

export const collectionPostHomePage = computed<WithFollowStatus<IPost>[]>(() => {
  return _arrayPostKeyHomePage.map(slug => {
    const data = _mapPostHomePage.get(slug);
    return {
      data,
      followStatus: data && mainUserFollow.get(data.ownerId)
    }
  });
})

export const collectionPostFeedPage = computed<WithFollowStatus<IPost>[]>(() => {
  return _arrayPostFeedPage.map(slug => {
    const data = _mapPostFeedPage.get(slug);
    return {
      data,
      followStatus: data && mainUserFollow.get(data.ownerId)
    }
  });
})

export const clearPostFeedPage = () => {
  _mapPostFeedPage.clear();
  _arrayPostFeedPage.length = 0;
}

export const addPostHome = (...args: IPost[]) => {
  args.forEach(post => {
    _mapPostHomePage.set(post.slug, post);
    _arrayPostKeyHomePage.push(post.slug);
  })
}

export const addPostFeed = (...args: IPost[]) => {
  args.forEach(post => {
    _mapPostFeedPage.set(post.slug, post);
    _arrayPostFeedPage.push(post.slug);
  })
}

export function useFetchPostBySlug() {
  const { exec, statusSuccess, status, data } = useApi(fetchPostBySlugApi);
  return {
    exec: async (slug: string) => {
      await exec(slug);
      if (statusSuccess.value && data.value) {
        setCurrentPost(data.value);
        await fetchRelationshipStatus(data.value.ownerId);
      }
    },
    data: computed<WithFollowStatus<IPost>>(() => ({
      post: data.value,
      followStatus: data.value && mainUserFollow.get(data.value.ownerId),
    })),
    status,
  }
}



const likePost = (slug: IPost['slug'], liked: boolean) => {
  let likeChange = liked ? 1 : -1;
  let post = _mapPostHomePage.get(slug);
  if (post) {
    post.liked = liked;
    post.likeCount += likeChange;
  }
  post = _mapPostFeedPage.get(slug);
  if (post) {
    post.liked = liked,
      post.likeCount += likeChange;
  }
  if (_currentPost.value && slug === _currentPost.value.slug) {
    const likeCount = _currentPost.value.likeCount + likeChange;
    _currentPost.value.likeCount = likeCount;
    _currentPost.value.liked = liked
    //  = {
    //   ..._currentPost.value,
    //   liked,
    //   likeCount,
    // }
  }
}

export function useLikePost() {
  const { exec, statusSuccess, status } = useApi(likePostApi);
  return {
    likePost: async (slug: IPost['slug'], liked: boolean) => {
      await exec(slug, liked);
      if (statusSuccess.value) {
        likePost(slug, liked);
      }
    },
    likePostStatus: status,
  }
}

const savePost = (slug: IPost['slug'], saved: boolean) => {
  let post = _mapPostHomePage.get(slug);
  post && (post.saved = saved);
  post = _mapPostFeedPage.get(slug);
  post && (post.saved = saved);
  (_currentPost.value?.slug === slug) && (_currentPost.value.saved = saved);
}

export function useSavePost() {
  const { exec, statusSuccess, status } = useApi(savePostApi);
  return {
    savePost: async (slug: IPost['slug'], liked: boolean) => {
      await exec(slug, liked);
      if (statusSuccess.value)
        savePost(slug, liked);
    },
    savePostStatus: status,
  }
}

export function registerFetchPostHomePage() {
  const { data, exec, status } = useApi(fetchFeedPostsHomePageApi);

  let postParameter: IPostParameter = {
    pageNumber: 0,
    pageSize: 4,
  };
  let hasNext = true;

  const fetchFeedPosts = async () => {
    if (!status.value.Pending && hasNext) {
      await exec({ ...postParameter, pageNumber: postParameter.pageNumber + 1 });
      if (status.value.Success && data.value) {
        hasNext = data.value.pagination.hasNext;
        hasNext && postParameter.pageNumber!++;
        const userIds = data.value.items.map(t => t.ownerId);
        await fetchRelationshipStatus(userIds);
        addPostHome(...data.value.items);
      }
    }
  }
  const autoLoad = whenScrollAtBottom(fetchFeedPosts);

  onActivated(() => {
    fetchFeedPosts();
    autoLoad.listener()
  });

  onDeactivated(() => autoLoad.clearListener());

  return {
    collectionPostHomePage,
    status,
  }
}