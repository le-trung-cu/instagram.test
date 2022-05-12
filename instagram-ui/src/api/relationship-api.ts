import { IRelationship } from "@/models/IRelationship";
import { ISuggestedUser } from "@/models/ISuggestedUser";
import { IUserFollow } from "@/models/IUserFollow";
import api from "./api";
import { EndpointUrlBuilder } from "./helpers/endpointUrlBuilder";
import { getPageList } from "./helpers/getPageListResponse";
import { IRequestParameters } from "./request-features/IRequestParameters";

export async function followUserApi(userName: string, followed: boolean) {
  const apiMethod = followed? api.put : api.delete;
  apiMethod(EndpointUrlBuilder['relationship/:userId'](userName));
}

export async function fetchRelationshipStatusApi(userId: string): Promise<IRelationship['status']> {
  const response = await api.get( EndpointUrlBuilder['relationship/:userId'](userId));
  return response.data.status;
}

export async function fetchSuggestionFriendsApi(parameter: IRequestParameters){
  const response = await api.get(EndpointUrlBuilder['relationship/suggestion'](parameter));
  return getPageList<ISuggestedUser>(response);
}

export async function fetchFollowingApi(userName: string, parameter: IRequestParameters){
  const response = await api.get(EndpointUrlBuilder[':username/following'](userName, parameter));
  return getPageList<IUserFollow>(response);
}

export async function fetchFollowersApi(userName: string, parameter: IRequestParameters){
  const response = await api.get(EndpointUrlBuilder[':username/followers'](userName, parameter));
  return getPageList<IUserFollow>(response);
}