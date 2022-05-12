import { setHasNewNotification } from "@/composable/use-notification-manger";
import { IUser } from "@/composable/use-user";
import { INotification } from "@/models/INotification";
import { HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { DeepReadonly } from "vue";

let connection: HubConnection;

export function initConnection(user: DeepReadonly<IUser>) {
  connection = new HubConnectionBuilder()
    .withUrl("https://localhost:7299/notify", { accessTokenFactory: () => user.accessToken! })
    .configureLogging(LogLevel.Information)
    .build();

  connection.on('ReceiveMessage', (message: INotification | string) => {
    console.log('message ', message)
    if(typeof message =='object'){
      setHasNewNotification(true)
    }
  })

  connection.onclose(async () => {
    await start();
  })

  start();

  async function start() {
    try {
      await connection.start();
      await connection.invoke("RegisterReceiveNotification", user.id);
    } catch (error) {
      console.log((error as any).toString());
    }
  }

}

export async function connectToPostGroup(slug: string) {
  try {
    await connection.invoke("AddUserToGroup", `PostGroup:${slug}`);
  }catch(error){

  }
}

export async function removeFromPostGroup(slug: string) {
  try {
    await connection.invoke("RemoveUserFromGroup", `PostGroup:${slug}`);
  }catch(error){

  }
}