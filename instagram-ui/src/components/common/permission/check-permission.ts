import { getUser, IUser as CurrentUser } from "@/composable/use-user";
// import { addUserFollow, addUserUnFollow, getFollowStatus, getUserFollowCollection } from "@/composable/use-relationship-manager";
import { IComment } from "@/models/IComment";
import { IPost } from "@/models/IPost";
import { DeepReadonly } from "@vue/runtime-core";
import { Awaitable } from "vitest";
import { bool, TypeOf } from "yup";
import { computed, ref } from "vue";

type User = DeepReadonly<CurrentUser>;
type UserId = User['id'];
type UserName = User['userName'];
type CommentId = IComment['id'];
type PostId = IPost['id'];

type PolicyResource = {
  'canViewSavedPost': { userName: UserName },
  'isPostOwner': { ownerId: UserId },
  'isCommentOwner': { ownerId: UserId },
  'canDeletePost': PolicyResource['isPostOwner'],
  'canDeleteComment': { ownerId: UserId, postOwnerId: UserId },
  'canFollowUser': UserId,
  'maxAge18': null
};
type Policy = keyof PolicyResource;

export async function checkPermission<T extends Policy>(policy: T, user: Partial<User>, resource?: PolicyResource[T]): Promise<boolean> {
  switch (policy) {
    case 'canViewSavedPost':
      const _resource = resource as PolicyResource['canViewSavedPost'];
      return _resource.userName === user.userName;
    default:
      return false;
  }
}

const permissionStatus = {
  'Idle': Symbol('IDLE'),
  'Pending': Symbol('PENDING'),
  'Success': Symbol('SUCCESS'),
  'Error': Symbol('ERROR'),
}

const { Idle, Success, Pending, Error } = permissionStatus;

export function useCheckPermission() {
  const status = ref(Idle);
  console.log('status check', status.value);

  const result = ref<boolean>();
  const _checkPermission = async (...args: Parameters<typeof checkPermission>) => {
    status.value = Pending;
    result.value = undefined;
    console.log('status check', status.value);

    try {
      result.value = await checkPermission(...args);
      status.value = Success;
    } catch (e) {
      console.log();
      status.value = Error;
    } finally {

    }
  }
  return {
    checkPermission: _checkPermission,
    result,
    status: computed(() => {
      return {
        Idle: status.value === Idle,
        Pending: status.value === Pending,
        Success: status.value === Success,
        Error: status.value === Error,
      }
    })
  }
}
