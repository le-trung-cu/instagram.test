import { IUserProfile } from "@/models/IUserProfile";
import IUserSearchResult from "@/models/IUserSearchResult";
import { AxiosRequestConfig } from "axios";
import api from "./api";
import { EndpointUrlBuilder } from "./helpers/endpointUrlBuilder";


type Abort = {
  abort: Function
}

export async function searchUserNameApi(userName: string, config?: AxiosRequestConfig & Abort) {
  const response = await api.get(EndpointUrlBuilder['users/search?username'](userName), config);
  const users: IUserSearchResult[] = response.data;
  return users;
}

export async function fetchUserProfileApi(userName: string) {
  const response = await api.get(EndpointUrlBuilder['users/:username'](userName));
  const user: IUserProfile = response.data;
  return user;
}