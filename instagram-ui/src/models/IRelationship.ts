export type IRelationship = {
  userId: string,
  status: 'following' | 'unFollowing' | undefined;
}