import { getUser } from "@/composable/use-user";
import { useLayout } from "@/composable/useLayout"

const {layout, LAYOUTS} = useLayout();
const userName = getUser().userName;

export const PageUrlBuilder = {
  '/': () => '/',
  '/:username/posts': (userName: string) => `/${userName}/posts`,
  '/p/:slug': (slug: string) =>`/p/${slug}`,
  '/p/:slug/comments': (slug: string) => layout.value == LAYOUTS.mobile? `/p/${slug}/comments` : `/p/${slug}`,
  'activity': () => 'account/activity',
  '/:username/': (userName: string) => `/${userName}/`,
  '/:username/feed': (userName: string) => `/${userName}/feed`,
  '/:username/saved': (userName: string) => `/${userName}/saved`,
  '/:username/tagged': (userName: string) => `/${userName}/tagged`,
  '/:username/following': (userName: string) => `/${userName}/following`,
  '/:username/followers': (userName: string) => `/${userName}/followers`,
  '/explore/people': () => '/explore/people',
  'saved': `/${userName}/saved`,
  '/login': () => '/login',
}