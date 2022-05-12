import { useApi } from "@/api/composable/use-api";
import { fetchFollowersApi, fetchFollowingApi, fetchRelationshipStatusApi, fetchSuggestionFriendsApi, followUserApi } from "@/api/relationship-api";
import { IRequestParameters } from "@/api/request-features/IRequestParameters";
import { WithFollowStatus } from "@/helpers";
import { IRelationship } from "@/models/IRelationship";
import { IUserFollow } from "@/models/IUserFollow";
import { computed, onMounted, onUnmounted, reactive, readonly } from "vue";
import { whenScrollAtBottom } from "./use-scroll-at-bottom";
import { IUser } from "./use-user";

type UserId = IUser['id'];

const _mainUserFollow = reactive(new Map<UserId, IRelationship['status']>());
const _suggestionFriendsOnHomePage = reactive(new Map<UserId, IRelationship['status']>());
const _suggestionFriendsOnPeoplePage = reactive(new Map<UserId, IRelationship['status']>());
const _userFollowing = reactive(new Map<UserId, IRelationship['status']>());
const _userFollowers = reactive(new Map<UserId, IRelationship['status']>());

const _store = [_mainUserFollow, _suggestionFriendsOnHomePage, _suggestionFriendsOnPeoplePage, _userFollowing, _userFollowers]

export const mainUserFollow = readonly(_mainUserFollow);

const addFollowUser = (userId: string, followStore: Map<string, IRelationship['status']>) => {
  followStore.set(userId, 'following');
  _store.filter(t => t !== followStore)
    .forEach(t => {
      t.get(userId) && t.set(userId, 'following');
    })
}

const addUnFollowUser = (userId: string, followStore: Map<string, IRelationship['status']>) => {
  followStore.set(userId, 'unFollowing');

  _store.filter(t => t !== followStore)
    .forEach(t => {
      t.get(userId) && t.set(userId, 'unFollowing');
    })
}

const updateFollowStatus = (userId: string, status: IRelationship['status']) => {
  _store.forEach(t => {
    if (t.has(userId)) {
      t.set(userId, status);
    }
  })
}

export const useFetchSuggestionFriendsOnHomePage = () => {
  const { data, status, exec } = useApi(fetchSuggestionFriendsApi);
  let suggestionFriends = computed(() => {
    return data.value!.items.map(user => ({
      data: user,
      followStatus: _suggestionFriendsOnHomePage.get(user.id),
    }));
  });

  return {
    fetchSuggestionFriends: async (parameters: IRequestParameters) => {
      await exec(parameters);
      if (status.value.Success && data.value?.items) {
        data.value.items.forEach(t => addUnFollowUser(t.id, _suggestionFriendsOnHomePage));
      }
    },
    status,
    suggestionFriends,
  }
}

export const registerFetchSuggestionFriendsOnPeoplePage = () => {
  const { data, status, exec } = useApi(fetchSuggestionFriendsApi);
  const requestParameters: IRequestParameters = {
    pageNumber: 1,
    pageSize: 10,
  }
  let hasNext = true;

  const _suggestionFriends = reactive(new Array<IUser>());

  let suggestionFriends = computed(() => {
    return _suggestionFriends.map(user => ({
      data: user,
      followStatus: _suggestionFriendsOnPeoplePage.get(user.id),
    }));
  });

  const fetchSuggestionFriends = async () => {
    if (!status.value.Pending && hasNext) {
      await exec({ ...requestParameters, pageNumber: requestParameters.pageNumber + 1 });
      if (status.value.Success && data.value?.items) {
        data.value.items.forEach(t => addUnFollowUser(t.id, _suggestionFriendsOnPeoplePage));
        _suggestionFriends.push(...data.value.items);
        requestParameters.pageNumber++;
        hasNext = data.value.pagination.hasNext;
      }
    }
  }

  const autoLoadSuggestionFriends = whenScrollAtBottom(fetchSuggestionFriends)

  onMounted(async () => {
    await fetchSuggestionFriends();
    autoLoadSuggestionFriends.listener()
  });

  onUnmounted(() => {
    _suggestionFriendsOnPeoplePage.clear();
    autoLoadSuggestionFriends.clearListener();
  });

  return {
    status,
    suggestionFriends,
  }
}

export const registerFetchFollowing = (userName: string) => {
  const _following = reactive(new Array<IUserFollow>());
  const following = computed<WithFollowStatus<IUserFollow>[]>(() => {
    return _following.map(user => ({
      data: user,
      followStatus: _userFollowing.get(user.id)
    }))
  });
  const requestParameters: IRequestParameters = {
    pageNumber: 1,
    pageSize: 10,
  }
  let hasNext = true;

  const { data, exec, status } = useApi(fetchFollowingApi);
  const fetchFollowing = async () => {
    if (!status.value.Pending && hasNext) {
      await exec(userName, requestParameters);
      if (status.value.Success && data.value?.items) {
        data.value.items.forEach(user => {
          _following.push(user);
          if (user.status === 'following') addFollowUser(user.id, _userFollowing);
          else if (user.status === 'unFollowing') addUnFollowUser(user.id, _userFollowing);
        });
        hasNext = data.value.pagination.hasNext;
        hasNext && requestParameters.pageNumber++;
      }
    }
  }
  const autoLoadFollowing = whenScrollAtBottom(fetchFollowing);
  onMounted(async () => {
    fetchFollowing();
    autoLoadFollowing.listener();
  })

  onUnmounted(() => {
    autoLoadFollowing.clearListener();
    _following.length = 0;
    _userFollowing.clear();
    requestParameters.pageNumber = 1;
  })

  return {
    following,
    status,
  }
}

export const registerFetchFollowers = (userName: string) => {
  const _followers = reactive(new Array<IUserFollow>());
  const followers = computed<WithFollowStatus<IUserFollow>[]>(() => {
    return _followers.map(user => ({
      data: user,
      followStatus: _userFollowers.get(user.id)
    }))
  });
  const requestParameters: IRequestParameters = {
    pageNumber: 1,
    pageSize: 10,
  }
  let hasNext = true;

  const { data, exec, status } = useApi(fetchFollowersApi);
  const fetchFollowers = async () => {
    if (!status.value.Pending && hasNext) {
      await exec(userName,requestParameters );
      if (status.value.Success && data.value?.items) {
        data.value.items.forEach(user => {
          _followers.push(user);
          if (user.status === 'following') addFollowUser(user.id, _userFollowers);
          else if (user.status === 'unFollowing') addUnFollowUser(user.id, _userFollowers);
        });
        hasNext = data.value.pagination.hasNext;
        hasNext &&  requestParameters.pageNumber++;
      }
    }
  }
  const autoLoadFollowers = whenScrollAtBottom(fetchFollowers);
  onMounted(async () => {
    fetchFollowers();
    autoLoadFollowers.listener();
  })

  onUnmounted(() => {
    autoLoadFollowers.clearListener();
    _followers.length = 0;
    _userFollowers.clear();
  })

  return {
    followers,
    status,
  }
}

export const useFollowersManager = () => {
  const { data, exec, status } = useApi(fetchFollowersApi);
  const fetchFollowing = async (userName: string, parameter: IRequestParameters) => {
    await exec(userName, parameter);
    if (status.value.Success && data.value?.items) {
      data.value.items.forEach(t => {
        if (t.status === 'following')
          addFollowUser(t.id, _userFollowers);
        if (t.status === 'unFollowing')
          addUnFollowUser(t.id, _userFollowers);
      });
    }
  }

  return {
    fetchFollowing,
    data,
    status,
  }
}

export const useFollowingManager = () => {
  const { data, exec, status } = useApi(fetchFollowingApi);
  const fetchFollowing = async (userName: string, parameter: IRequestParameters) => {
    await exec(userName, parameter);
    if (status.value.Success && data.value?.items) {
      data.value.items.forEach(t => {
        if (t.status === 'following')
          addFollowUser(t.id, _userFollowing);
        if (t.status === 'unFollowing')
          addUnFollowUser(t.id, _userFollowing);
      });
    }
  }

  return {
    fetchFollowing,
    data,
    status,
  }
}

export async function fetchRelationshipStatus(userIds: string[] | string) {
  userIds = Array.isArray(userIds) ? userIds : [userIds]
  userIds = [...new Set(userIds)];
  userIds = userIds.filter(userId => !_mainUserFollow.has(userId));
  const promises = userIds.map(userId => fetchRelationshipStatusApi(userId));
  const results = await Promise.allSettled(promises);

  userIds.forEach((userId, index) => {
    if (results[index].status === 'fulfilled') {
      _mainUserFollow.set(userId, (results[index] as any).value)
    }
  });
}

export const useCheckRelationship = (userId: string) => {
  const relationship = computed(() => _mainUserFollow.get(userId));
  const isFollowing = computed(() => relationship.value == 'following');
  return {
    isFollowing,
  }
}

export const useFollowUser = () => {
  const { exec, status } = useApi(followUserApi);
  return {
    followUser: async (userName: string, userId: string, followed: boolean) => {
      await exec(userName, followed);
      updateFollowStatus(userId, followed ? 'following' : 'unFollowing');
    },
    status,
  }
}