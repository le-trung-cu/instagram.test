import { createRouter, createWebHistory, RouteLocationNormalized, RouteRecordRaw, RouterScrollBehavior, } from "vue-router"
import Home from '@/screens/home/Home.vue'
import { mobileCheck } from "@/helpers"
import { useLayout } from "@/composable/useLayout"
import { getUser } from "@/composable/use-user";


const { layout, LAYOUTS } = useLayout();
const generalRoutes: RouteRecordRaw[] = [
  {
    path: "/",
    name: "Home",
    component: Home,
    meta: {
      keepAlive: true,
    },
  },
  {
    path: '/explore',
    name: 'Explore',
    meta: {
      keepAlive: true,
    },
    component: () => import('@/screens/explore/Explore.vue')
  },
  {
    path: '/p/:slug',
    name: 'PostDetail',
    props: route => ({ slug: route.params.slug }),
    component: () => import('@/screens/post/PostDetail.vue')
  },
  {
    path: '/p/:slug/comments',
    name: 'Comments',
    props: route => ({ slug: route.params.slug }),
    component: () => import('@/screens/comment/Comment.vue')
  },
  {
    path: '/explore/People',
    name: 'People',
    meta: {
      keepAlive: true,
    },
    component: () => import('@/screens/explore/people/People.vue')
  },
  {
    path: '/explore/search',
    name: 'Search',
    component: () => import('@/screens/explore/search/Search.vue')
  },
  {
    path: '/accounts/activity',
    name: 'Activity',
    component: () => import('@/screens/activity/Activity.vue')
  },
  {
    path: '/:userName',
    name: 'Profile',
    component: () => import('@/screens/profile/Profile.vue'),
    props: route => ({ userName: route.params.userName }),
    meta: {
      // keepAlive: true
    },
    children: [
      {
        path: '',
        name: 'GridPost',
        component: () => import('@/screens/profile/GridPost.vue'),
        meta: {
          showPostModal: true,
        }
      },
      {
        path: 'feed',
        name: 'Feed',
        component: () => import('@/screens/profile/Feed.vue'),
        meta: {
          showPostModal: true,
        },
      },
      {
        path: 'saved',
        name: 'Saved',
        component: () => import('@/screens/profile/Saved.vue'),
        meta: {
          showPostModal: true,
        },
      },
      {
        path: 'tagged',
        name: 'Tagged',
        component: () => import('@/screens/profile/Tagged.vue'),
        meta: {
          showPostModal: true,
        },
      },
    ]
  },
  {
    path: '/:userName/following',
    name: 'Following',
    component: () => import('@/screens/following/Following.vue'),
    props: route => ({ userName: route.params.userName }),
  },
  {
    path: '/:userName/followers',
    name: 'Followers',
    component: () => import('@/screens/followers/Followers.vue'),
    props: route => ({ userName: route.params.userName }),
  },
  {
    path: '/login',
    name: 'Login',
    component: () => import('@/screens/login/Login.vue')
  },
  {
    path: '/account/signup',
    name: 'Register',
    component: () => import('@/screens/login/RegisterScreen.vue')
  }
]

const routes = mobileCheck() ? getMobileRoutes() : getDesktopRoutes();

const router = createRouter({
  history: createWebHistory(/*process.env.BASE_URL*/),
  routes,
});
router.beforeEach((to, from, next) => {
  if (to.name == 'Login' || to.name == 'Register')
    next()
  else if (!getUser().isAuthenticated && to.name !== 'Login')
    next({ name: 'Login' });
  else next();
})

router.afterEach((to, from) => {
  document.getElementsByTagName('title')[0].textContent = `Instagram | ${to.name?.toString()}`
})

export default router;

function getMobileRoutes() {
  const routes: Array<RouteRecordRaw> = [
    ...generalRoutes,
  ];
  return routes;
}

function getDesktopRoutes() {
  const routes: Array<RouteRecordRaw> = [
    ...generalRoutes,
  ];
  return routes;
}