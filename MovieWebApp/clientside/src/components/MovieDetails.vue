<template>
    <div class="container" v-if="movieDetails && areDetailsFound">
        <h1>{{ movieDetails.title }}</h1>
        <div class="details">
            <img :src="movieDetails.poster" alt="Movie Poster" class="poster" />
            <div class="details-text">
                <p>{{ movieDetails.plot }}</p>
                <p><strong>Release Date:</strong> {{ movieDetails.released }}</p>
                <p v-if="movieDetails.rated"><strong>Rating:</strong> {{ movieDetails.rated }}</p>
                <p v-if="movieDetails.runtime"><strong>Runtime:</strong> {{ movieDetails.runtime }}</p>
                <p v-if="movieDetails.genre"><strong>Genre:</strong> {{ movieDetails.genre }}</p>
                <p v-if="movieDetails.director"><strong>Director:</strong> {{ movieDetails.director }}</p>
                <p v-if="movieDetails.actors"><strong>Actors:</strong> {{ movieDetails.actors }}</p>
                <p v-if="movieDetails.language"><strong>Language:</strong> {{ movieDetails.language }}</p>
                <p v-if="movieDetails.boxOffice"><strong>Box Office:</strong> {{ movieDetails.boxOffice }}</p>
                <p v-if="movieDetails.imdbRating !== null"><strong>IMDB Rating:</strong> {{ movieDetails.imdbRating }}</p>
            </div>
        </div>
    </div>
    <div class="container" v-else>
        Loading...
    </div>
</template>

<script lang="ts">
import { defineComponent, computed, onMounted } from 'vue';
import { useStore } from 'vuex';
import { useRoute } from 'vue-router';
import { MovieDetails } from '../Models/MovieDetails';

    export default defineComponent({
    name: 'MovieDetails',
    setup() {
        const store = useStore();
        const route = useRoute();
        const imdbId = route.params.imdbId as string;

        onMounted(() => {
            store.dispatch('fetchMovieDetails', imdbId);
        });
        const movieDetails = computed<MovieDetails | null>(() => store.state.movieDetails);
        const areDetailsFound = computed(() => movieDetails.value !== null && movieDetails.value.response !== 'False');

        return {
            movieDetails,
            areDetailsFound
        };
    }
});
</script>
<style scoped>
    .container {
        text-align: center;
    }

    .details {
        display: flex;
        align-items: flex-start;
        justify-content: center;
        gap: 20px;
        margin-top: 20px;
    }

    .poster {
        max-width: 300px;
        height: auto;
        border: 1px solid #ccc;
        border-radius: 4px;
    }

    .details-text {
        max-width: 600px;
        text-align: left;
    }

    p {
        color: #333;
        margin: 10px 0;
    }
</style>