import { IComment } from "./IComment";

export type IPost = {
  id: number,
  slug: string,
  postType: 'Post' | 'PostPhoto' | 'PostVideo' | 'PostCarousel'
  content: string,
  enableComment: boolean,
  ownerId: string,
  userName: string,
  title: string,
  avatar: string,
  createdAt: Date,
  liked: boolean,
  saved: boolean,
  comments?: IComment[],
  commentCount: number,
  likeCount: number,
  mediaItems: IMediaItem[],
  thumbnail: string
}

export type IMediaItem = {
  id: number,
  path: string,
  mediaType: 'PhotoItem' | 'VideoItem',
  userTags: IUserTagMedia[]
}

export type IUserTagMedia = {
  userId: string,
  userName: string,
  top?: number,
  left?: number,
}