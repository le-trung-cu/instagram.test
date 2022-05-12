export interface IPostThumbnail {
  id: number,
  slug: string,
  postType: 'Post' | 'PostPhoto' | 'PostVideo' | 'PostCarousel'
  commentCount: number,
  likeCount: number,
  thumbnail: string
}