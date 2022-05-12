export enum NotificationTypes {
    UserLikePost,
    UserLikeComment,
    UserCommentInPost,
    FollowingNewPost,
}

export interface INotification {
    type: NotificationTypes,
    message: string,
    userName: string,
    userId: string,
    createdAt: Date
}
