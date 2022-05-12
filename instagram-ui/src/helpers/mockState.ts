import { IPost } from "@/models/IPost"
let id = 0;
export const getPostMock = (): IPost => {
  id = id+1;
  return {
    id: id,
    slug: 'a',
    commentCount: 1,
    content: 'content',
    createdAt: new Date(Date.now()),
    enableComment: true,
    likeCount: 3,
    liked: true,
    avatar: '',
    ownerFullName: 'x',
    ownerId: 'aa',
    userName: 'ss',
    postType: 'PostCarousel',
    saved: true,
    thumbnail: '',
    mediaItems: [
      {
        id: 1,
        mediaType: 'PhotoItem',
        path: '/post/post-01.jpg',
        userTags: [{
          userId: '1',
          userName: 'jemoe',
          left: 0.5,
          top: 0.5
        }
        ]
      },
      {
        id: 2,
        mediaType: 'PhotoItem',
        path: '/post/post-02.jpg',
        userTags: [{
          userId: '1',
          userName: 'jemoe',
          left: 0.5,
          top: 0.5
        }
        ]
      },
      {
        id: 3,
        mediaType: 'PhotoItem',
        path: '/post/post-01b.jpg',
        userTags: [{
          userId: '1',
          userName: 'jemoe',
          left: 0.5,
          top: 0.5
        }
        ]
      }
    ]
  }
}