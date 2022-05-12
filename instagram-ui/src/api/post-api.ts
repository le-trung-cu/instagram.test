import api from './api'
import { IPostParameter } from './request-features/IPostParameter';
import { EndpointUrlBuilder } from './helpers/endpointUrlBuilder';
import { IPost } from '@/models/IPost';
import { getPageList, MetaData } from './helpers/getPageListResponse';
import { IMediaOfUser } from '@/models/IMediaOfUser';
import { IRequestParameters } from './request-features/IRequestParameters';
import { IPostThumbnail } from '@/models/IPostThumbnail';


export async function fetchExplorePostsApi(parameter: IRequestParameters) {
  const response = await api.get(EndpointUrlBuilder['posts/explore'](parameter));
  return getPageList<IPostThumbnail>(response);
}

export async function fetchPostBySlugApi(slug: string): Promise<IPost> {
  const response = await api.get(EndpointUrlBuilder['posts/:lug'](slug));
  return response.data;
}

export async function fetchPostsOfUserApi(userName: string, parameter: IPostParameter) {
  try {
    const response = await api.get(EndpointUrlBuilder[':username/posts'](userName, parameter));
    return getPageList<IPost>(response)
  } catch (error) {
    console.log((error as any).response.data);
    throw error;
  }
}

export async function fetchSavedPostsApi(parameter: IPostParameter){
  try {
    const response = await api.get(EndpointUrlBuilder['posts/saved'](parameter));
    return getPageList<IPost>(response)
  } catch (error) {
    console.log((error as any).response.data);
    throw error;
  }
}

export async function fetchMediasOfUserApi(userName: string, parameter: IPostParameter) {
  try {
    const response = await api.get(EndpointUrlBuilder[':username/tagged'](userName, parameter));
    return getPageList<IMediaOfUser>(response);
  } catch (error) {
    console.log((error as any).response.data);
    throw error;
  }
}

export async function fetchFeedPostsHomePageApi(parameter: IPostParameter): Promise<{ pagination: MetaData, items: IPost[] }> {
  const response = await api.get(EndpointUrlBuilder['feed/posts'](parameter));
  return getPageList<IPost>(response);
}

export async function likePostApi(slug: string, liked: boolean) {
  const apiMethod = liked ? api.put : api.delete;
  await apiMethod(EndpointUrlBuilder['posts/:slug/like'](slug));
}

export async function savePostApi(slug: string, saved: boolean) {
  const apiMethod = saved ? api.put : api.delete;
  await apiMethod(EndpointUrlBuilder['posts/:slug/save'](slug));
}