import { createRouter, createWebHistory } from 'vue-router';
import MovieSearch from '../components/MovieSearch.vue';
import MovieDetails from '../components/MovieDetails.vue';

const routes = [
    {
        path: '/',
        name: 'MovieSearch',
        component: MovieSearch
    },
    {
        path: '/movie/:imdbId',
        name: 'MovieDetails',
        component: MovieDetails
    }
];

const router = createRouter({
    history: createWebHistory(),
    routes
});

export default router;
