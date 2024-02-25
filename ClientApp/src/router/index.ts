import {createRouter, createWebHistory} from 'vue-router'
import Index from "@/components/Index.vue";
import Auth from "@/components/Login.vue";
import Register from "@/components/Register.vue";
import Film from "@/components/Film.vue";
import User from "@/components/User.vue";

const router = createRouter({
    history: createWebHistory(),
    routes: [
        {
            path: '/',
            name: 'index',
            component: Index,
            meta: {
                section: 'index'
            }
        },
        {
            path: '/login',
            name: 'login',
            component: Auth,
            meta: {
                section: 'login'
            }
        },
        {
            path: '/register',
            name: 'register',
            component: Register,
            meta: {
                section: 'register'
            }
        },
        {
            path: '/film/:id',
            name: 'film',
            component: Film,
            meta: {
                section: 'film'
            }
        },
        {
            path: '/user',
            name: 'user',
            component: User,
            meta: {
                section: 'user'
            }
        }
    ]
})

export default router
