import './assets/main.css'

import { createApp } from 'vue'
import App from './App.vue'
import router from './router'

import 'vuetify/styles'
import '@mdi/font/css/materialdesignicons.css'
import { createVuetify } from 'vuetify'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'
import { aliases, mdi } from 'vuetify/iconsets/mdi'
import VueCookies from 'vue-cookies';
import store from "@/store/store";

const vuetify= createVuetify({
    components,
    directives,
    icons: {
        defaultSet: 'mdi',
        aliases,
        sets: {
            mdi,
        },
    },
})


const app = createApp(App);

app.use(vuetify);
app.use(store);
app.use(router);
app.use(VueCookies, { expires: '7d'});

app.mount('#app');
