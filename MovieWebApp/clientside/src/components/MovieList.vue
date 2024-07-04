<template>
    <div>
        <ul>
            <li v-for="movie in movies" :key="movie.imdbID">
                <router-link :to="`/movie/${movie.imdbId}`">{{ movie.title }}</router-link>
            </li>
        </ul>
    </div>
</template>

<script lang="ts">
    import { defineComponent, PropType } from 'vue';
    import { MovieSearchResult } from '../Models/MovieSearchResult';

    export default defineComponent({
        name: 'MovieList',
        props: {
            movies: {
                type: Array as PropType<MovieSearchResult[]>,
                required: true
            }
        }
    });
</script>
<style scoped>
    ul {
        text-align: left;
    }

    li {
        padding: 10px 0;
        border-bottom: 1px solid #ccc;
    }

    a {
        color: #007BFF;
        text-decoration: none;
        font-weight: bold;
    }

        a:hover {
            text-decoration: underline;
        }
</style>