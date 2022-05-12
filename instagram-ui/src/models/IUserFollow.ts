export interface IUserFollow {
  id: string,
  userName: string,
  avatar: string,
  fullName: string,
  status: 'following' | 'unFollowing'
}