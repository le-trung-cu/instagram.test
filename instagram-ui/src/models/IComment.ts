export interface IComment {
  userName: string,
  avatar: string,
  ownerId: string,
  content: string,
  id: number,
  postId: number,
  createdAt: Date,
  updatedAt: Date,
  liked: boolean,
  likeCount: number,
}