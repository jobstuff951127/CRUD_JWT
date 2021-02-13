import Vue from 'vue'
import VueRouter from 'vue-router'
import Home from '../Home.vue'
import Login from '../Login.vue'

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home,
  },
  {
    path: '/Login',
    name: 'Login',
    component: Login,
  }
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

router.beforeEach((to, from, next) => {
  if (to.name !== 'Login' && localStorage.getItem('jwt') == null)
    next({ name: 'Login' })
  else if (to.name === 'Login' && localStorage.getItem('jwt') !== null)
    next({ name: 'Home' })
  else
    next()
})

export default router
