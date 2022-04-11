import {
  createRouter,
  createWebHistory,
} from "vue-router";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/",
      name: "home",
      // component: HomeView,
      redirect: "/admin",
    },
    {
      path: "/admin",
      name: "Admin",
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import("@/views/AdminView.vue"),
    },
  ],
});

export default router;
