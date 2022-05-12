<script setup lang="ts">
import { onMounted } from 'vue';
import { refreshToken } from './api/auth-api';
import PostModalProvide from './provide-inject/PostModalProvide.vue';
import { initConnection } from "@/signalR/notification";
import { getUser } from './composable/use-user';
onMounted(async () => {
  await refreshToken();
  const currentUser = getUser();
  if(currentUser?.isAuthenticated){
    initConnection(currentUser);
  }
})
</script>
<template>
  <PostModalProvide>
    <RouterView v-slot="{ Component }">
      <template v-if="Component">
        <!-- <Transition name="fade" mode="out-in"> -->
        <KeepAlive>
            <component v-if="$route.meta.keepAlive" :is="Component" :key="$route.name"></component>
        </KeepAlive>
        <!-- </Transition> -->
        <!-- <Transition name="fade" mode="out-in"> -->
        <component v-if="!$route.meta.keepAlive" :is="Component"></component>
        <!-- </Transition> -->
      </template>
    </RouterView>
  </PostModalProvide>

</template>

<style>
#app {
  /* height: 100vh; */
  /* overflow: hidden; */
}

/* width */
::-webkit-scrollbar {
  width: 10px;
}

/* Track */
::-webkit-scrollbar-track {
  box-shadow: inset 0 0 10px rgb(224, 224, 224);
}

/* Handle */
::-webkit-scrollbar-thumb {
  background: rgb(206, 205, 205);
}

/* Handle on hover */
/* ::-webkit-scrollbar-thumb:hover {
  background: rgb(102, 101, 101);
} */

/* we will explain what these classes do next! */
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.25s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
