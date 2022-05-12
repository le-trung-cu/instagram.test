import { searchUserNameApi as searchUserNameApi } from "@/api/user-api";
import { withAsync } from "@/helpers";
import IUserSearchResult from "@/models/IUserSearchResult";
import { reactive, readonly, ref, watch } from "vue";

const USERS_SEARCHED_RECENTLY = 'USERS_SEARCHED_RECENTLY';

const usersRecent = ref<IUserSearchResult[]>([]);



function getUsers(): IUserSearchResult[] {
  return JSON.parse(localStorage.getItem(USERS_SEARCHED_RECENTLY) ?? '[]')
}
function loadUsers() {
  usersRecent.value = getUsers();
}

function saveUser(user: IUserSearchResult) {
  let users = usersRecent.value.filter(u => u.userName != user.userName);
  users.unshift(user);
  localStorage.setItem(USERS_SEARCHED_RECENTLY, JSON.stringify(users));
  usersRecent.value = users;
}

function deleteUser(user: IUserSearchResult) {
  let users = usersRecent.value.filter(u => u.userName != user.userName);
  localStorage.setItem(USERS_SEARCHED_RECENTLY, JSON.stringify(users));
  usersRecent.value = users;
}
function clearAll() {
  localStorage.removeItem(USERS_SEARCHED_RECENTLY);
  usersRecent.value = [];
}
export function useUserSearchRecent() {
  return {
    loadUsers,
    saveUser,
    clearAll,
    deleteUser,
    users: readonly(usersRecent)
  }
}


export function useSearchUser() {
  const state = reactive({
    searchQuery: '',
    userSearchResults: new Array<IUserSearchResult>(),
    isSearching: false,
  })

  let timer: number | undefined = undefined;
  let searchUserNameAbort: Function;
  
  function searchUser(userName: string) {
    if (timer) {
      clearTimeout(timer);
      timer = undefined
    }
    if (state.searchQuery.length > 0) {
      timer = setTimeout(async () => {
        state.userSearchResults = [];
        const { error, response } = await withAsync(searchUserNameApi, userName, {
          abort: (abort: Function) => searchUserNameAbort = abort,
        });

        if (error) {
          console.log(error);
          
          const errorAny = error as any;
          if (errorAny.aborted) {
            console.log('Aborted');
          }
          return;
        }
        state.userSearchResults = response ?? [];
      }, 500, state.searchQuery);
    }
  }

  watch(() => state.searchQuery,() => {
    state.userSearchResults = [];
    searchUserNameAbort?.();
    searchUser(state.searchQuery);
  });

  return {
    stateSearch: state,
  };
}