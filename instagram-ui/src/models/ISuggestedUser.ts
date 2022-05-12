import { IUser } from "@/composable/use-user";

export type ISuggestedUser = Pick<IUser, 'avatar' | 'userName' | 'id' | 'fullName'> & {subtile?: string}