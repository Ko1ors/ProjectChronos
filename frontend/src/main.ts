<<<<<<< HEAD
import "./assets/main.css";
import "./assets/cdn.jsdelivr.net_npm_bootstrap@5.3.2_dist_css_bootstrap.min.css";
import PrimeVue from "primevue/config";
import "primevue/resources/themes/lara-light-purple/theme.css";
import ProfileInfo from "./components/ProfileInfo.vue";
import AuthorizationPage from "./components/AuthorizationPage.vue";
import MainPage from "./components/MainPage.vue";
import AboutChronos from "./components/AboutChronos.vue";
import DecksVue from "./components/Decks.vue";
import { createRouter, createWebHistory } from "vue-router";

import { createApp } from "vue";
import { createPinia } from "pinia";

import App from "./App.vue";

const app = createApp(App);
app.use(PrimeVue);
=======
import './assets/main.css'
import './assets/cdn.jsdelivr.net_npm_bootstrap@5.3.2_dist_css_bootstrap.min.css'
import PrimeVue from 'primevue/config'
import 'primevue/resources/themes/lara-light-purple/theme.css'
import ProfileInfo from './components/ProfileInfo.vue'
import AuthorizationPage from './components/AuthorizationPage.vue'
import MainPage from './components/MainPage.vue'
import AboutChronos from './components/AboutChronos.vue'
import { createRouter, createWebHistory } from 'vue-router'

import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'

const app = createApp(App)
app.use(PrimeVue)
>>>>>>> 11af2ddaac86eb6f433efc0a129729af9e80f8fd

const router = createRouter({
  history: createWebHistory(),
  routes: [
<<<<<<< HEAD
    { path: "/", component: MainPage },
    { path: "/AuthorizationPage", component: AuthorizationPage },
    { path: "/ProfileInfo", component: ProfileInfo },
    { path: "/AboutChronos", component: AboutChronos },
    { path: "/DecksVue", component: DecksVue },
  ],
});

app.use(createPinia());
app.use(router);

app.mount("#app");
=======
    { path: '/', component: MainPage },
    { path: '/AuthorizationPage', component: AuthorizationPage },
    { path: '/ProfileInfo', component: ProfileInfo },
    { path: '/AboutChronos', component: AboutChronos }
  ]
})

app.use(createPinia())
app.use(router)

app.mount('#app')
>>>>>>> 11af2ddaac86eb6f433efc0a129729af9e80f8fd
