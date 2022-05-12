import { IComment } from "@/models/IComment";
import api from "./api";
import { EndpointUrlBuilder } from "./helpers/endpointUrlBuilder";
import { getPageList } from "./helpers/getPageListResponse";
import { ICommentParameter } from "./request-features/ICommentParameter";

export async function fetchCommentsApi(postSlug: string, parameters: ICommentParameter) {
  const response = await api.get(EndpointUrlBuilder['posts/:slug/comments'](postSlug, parameters))
  return getPageList<IComment>(response);
}

export async function likeCommentApi(id: number, liked: boolean) {
  const apiMethod = liked ? api.put : api.delete;
  apiMethod(EndpointUrlBuilder['comments/:id/like'](id));
}

export async function createCommentApi(slug: string, comment: any){
  const response = await api.post(EndpointUrlBuilder['posts/:slug/comments'](slug), comment);
  return response.data as IComment;
}