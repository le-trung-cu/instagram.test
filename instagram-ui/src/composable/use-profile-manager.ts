import { useApi } from "@/api/composable/use-api";
import { fetchMediasOfUserApi, fetchPostsOfUserApi } from "@/api/post-api";
import { fetchUserProfileApi } from "@/api/user-api";
import { IPostParameter } from "@/api/request-features/IPostParameter";
import { IPostThumbnail } from "@/models/IPostThumbnail";
import { computed, onActivated, onDeactivated, onMounted, onUnmounted, reactive, readonly, ref, watch } from "vue";

import { whenScrollAtBottom } from "./use-scroll-at-bottom";
import { collectionPostFeedPage, clearPostFeedPage, addPostFeed } from "@/composable/use-post-store";
import { IUserProfile } from "@/models/IUserProfile";
import { fetchRelationshipStatus, mainUserFollow } from "./use-relationship-manager";
import { useCheckPermission } from "@/components/common/permission/check-permission";
import { getUser } from "./use-user";
import { IMediaOfUser } from "@/models/IMediaOfUser";
const _currentUser = getUser();

const _collectionGridPost = reactive(new Array<IPostThumbnail>());
const _collectionMediasOfUser = reactive(new Array<IMediaOfUser>());

const collectionGridPost = readonly(_collectionGridPost);
const collectionMediasOfUser  = readonly(_collectionMediasOfUser);

const _userName = ref('');
const _userProfile = ref<IUserProfile>();
let _gridPostParameter: IPostParameter = { pageNumber: 1, pageSize: 10, thumbnail: true }
let _gridPostHasNextPage: boolean = true;
let _feedPostParameter: IPostParameter = { pageNumber: 1, pageSize: 10 }
let _feedPostHasNextPage: boolean = true;
let _mediaOfUserParameter: IPostParameter = { pageNumber: 1, pageSize: 10, thumbnail: true }
let _mediaOfUserHasNextPage: boolean = true;

const userName = readonly(_userName);
export const userProfile = computed(() => ({
  data: _userProfile.value,
  followStatus: _userProfile.value?.id && mainUserFollow.get(_userProfile.value.id),
}));

const setUserName = (userName?: string) => {
  if (userName !== _userName.value && userName) {
    _collectionGridPost.length = 0;
    _gridPostHasNextPage = true;
    _feedPostHasNextPage = true;
    clearPostFeedPage();
    _gridPostParameter = { pageNumber: 1, pageSize: 10, thumbnail: true },
      _gridPostHasNextPage = true;
    _feedPostParameter = { pageNumber: 1, pageSize: 10 };
    _feedPostHasNextPage = true;
    _userName.value = userName;
    _userProfile.value = undefined;

    _collectionMediasOfUser.length = 0;
    _mediaOfUserHasNextPage = true,
    _mediaOfUserParameter = { pageNumber: 1, pageSize: 10, thumbnail: true }
  }
}

export function registerFetchUserProfile() {

  const { data, exec, status } = useApi(fetchUserProfileApi);
  const { checkPermission, result: canViewSavedPost } = useCheckPermission();

  const fetUserProfile = async () => {
    await exec(_userName.value);
    _userProfile.value = data.value
    _userProfile.value && await fetchRelationshipStatus(_userProfile.value.id);
    await checkPermission('canViewSavedPost', _currentUser, { userName: _userName.value });
  }

  onMounted(async () => _userProfile.value === undefined && await fetUserProfile());

  watch(() => _userName.value, async () => {
    await fetUserProfile()
  })
  return { status, canViewSavedPost, userName, setUserName, userProfile }
}

export function registerFetchGridPost() {
  const { data, exec, status } = useApi(fetchPostsOfUserApi);
  const fetchPostsOfUser = async () => {
    if (!status.value.Pending && _gridPostHasNextPage) {
      await exec(_userName.value, _gridPostParameter);
      console.log(data.value?.items);
      if (status.value.Success) {
        data.value && (_gridPostHasNextPage = data.value.pagination.hasNext);
        data.value?.items && _collectionGridPost.push(...data.value.items);
        _gridPostHasNextPage && _gridPostParameter.pageNumber!++;
      }
    }
  }

  const { listener, clearListener } = whenScrollAtBottom(fetchPostsOfUser);
  onMounted(async () => { _collectionGridPost.length === 0 && await fetchPostsOfUser(); });
  onActivated(() => { listener(); });
  onDeactivated(() => clearListener());
  return { status, collectionGridPost };
}

export function registerFetchFeedPost() {
  const { data, exec, status } = useApi(fetchPostsOfUserApi);
  const fetchPostsOfUser = async () => {
    if (!status.value.Pending && _feedPostHasNextPage) {
      await exec(_userName.value, _feedPostParameter);
      if (status.value.Success && data?.value) {
        _feedPostHasNextPage = data.value.pagination.hasNext;
        data.value.items && addPostFeed(...data.value.items);
        _feedPostHasNextPage && _feedPostParameter.pageNumber!++;

        const userIds = data.value.items.map(post => post.ownerId);
        await fetchRelationshipStatus(userIds);
      }
    }
  }

  const { listener, clearListener } = whenScrollAtBottom(fetchPostsOfUser);
  onMounted(async () => { await fetchPostsOfUser(); listener(); });

  onUnmounted(() => clearListener());

  watch(() => _userName.value, async () => {
    await fetchPostsOfUser()
  });
  return { status, collectionPostFeedPage };
}

export function registerFetchMediasOfUser(){
  const { data, exec, status } = useApi(fetchMediasOfUserApi);
  const fetchMediasOfUser = async () => {
    if (!status.value.Pending && _mediaOfUserHasNextPage) {
      await exec(_userName.value, _mediaOfUserParameter);
      if (status.value.Success && data?.value) {
        _mediaOfUserHasNextPage = data.value.pagination.hasNext;
        data.value.items && _collectionMediasOfUser.push(...data.value.items);
        _mediaOfUserHasNextPage && _mediaOfUserParameter.pageNumber!++;
      }
    }
  }

  const { listener, clearListener } = whenScrollAtBottom(fetchMediasOfUser);
  onMounted(async () => { await fetchMediasOfUser(); listener(); });

  onUnmounted(() => clearListener());

  watch(() => _userName.value, async () => {
    await fetchMediasOfUser()
  });
  return { status, collectionMediasOfUser };
}
