import { ICommentParameter } from "../request-features/ICommentParameter";
import { IPostParameter } from "../request-features/IPostParameter";
import { IRequestParameters } from "../request-features/IRequestParameters";

export function urlEncodeQueryParams(data: any) {
  if(data == null || data == undefined) return '';
  const params = Object.keys(data).map(key => data[key] ? `${encodeURIComponent(key)}=${encodeURIComponent(data[key])}` : '');
  const query = params.filter(value => !!value).join('&');
  
  return query.length > 0? '?' + query : '';
}

export const EndpointUrlBuilder = {
  /*Posts*/
  // get collection posts
  'posts': (parameter: IPostParameter) => `posts` + urlEncodeQueryParams(parameter),
  // get one posts by slug
  'posts/:lug': (slug: string) =>  `posts/${slug}`,
  // get collection posts of user
  ':username/posts': (userName: string, parameter: IPostParameter) => `${userName}/posts` + urlEncodeQueryParams(parameter),
  // get feed post of log in user
  'feed/posts': (parameter: IPostParameter) =>  'feed/posts' + urlEncodeQueryParams(parameter),
  // Like or Unlike post
  'posts/:slug/like': (slug: string) => `posts/${slug}/like`,
  // saved post
  'posts/:slug/save': (slug: string) => `posts/${slug}/save`,
  'posts/saved': (parameter: IPostParameter) => `posts/saved`+ urlEncodeQueryParams(parameter),

  /*comments*/
  'posts/:slug/comments': (slug: string, parameter?: ICommentParameter) => `posts/${slug}/comments` + urlEncodeQueryParams(parameter),
  'comments/:id': (id: number) => `comments/${id}`,
  'comments/:id/like': (id: number) => `comments/${id}/like`, 
  'users/:username': (userName: string) => `users/${userName}`,
  'users/search?username': (userName: string) => `users/search?username=${userName}`,
  
  ':username/tagged': (userName: string, parameter: IPostParameter) => `${userName}/tagged` + urlEncodeQueryParams(parameter),

  'relationship/suggestion': (parameter: IRequestParameters) => 'relationship/suggestion' + urlEncodeQueryParams(parameter),
  ':username/following': (userName: string, parameter: IRequestParameters) => `${userName}/following` + urlEncodeQueryParams(parameter),
  ':username/followers': (userName: string, parameter: IRequestParameters) => `${userName}/followers` + urlEncodeQueryParams(parameter),
  'relationship/:userId': (userName: string) => `relationship/${userName}`,

  'posts/explore': (parameter: IRequestParameters) => 'posts/explore' + urlEncodeQueryParams(parameter),
  'notification': (parameter: IRequestParameters) => 'notification' + urlEncodeQueryParams(parameter),
} as const;