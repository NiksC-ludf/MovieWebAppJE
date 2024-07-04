<template>
    <div class="container">
        <h1>Movie Search</h1>
        <input v-model="title" @keyup.enter="searchMovies" placeholder="Search for movies...">
        <MovieList 
            v-if="searchResults"
            :movies="searchResults" />
        <div v-if="hasSearchHappened && !wereMoviesFound">
            No movies found
        </div>
    </div>
</template>

<script lang="ts">
import { defineComponent, ref, computed, onMounted } from 'vue';
import { useStore } from 'vuex';
import MovieList from './MovieList.vue';

export default defineComponent({

    name: 'MovieSearch',
    components: {
        MovieList
    },
    setup() {
        const store = useStore();
        const title = ref('');
        const hasSearchHappened = ref(false);

        const searchResults = computed(() => store.state.searchResults);

        onMounted(() => {
            store.dispatch('fetchRecentSearches');
        });

        const searchMovies = () => {
            if(title.value)
            {
                store.dispatch('searchMovies', title.value)
                    .then(() => {
                        store.dispatch('fetchRecentSearches');
                    })
                hasSearchHappened.value = true;
            }
        };
        const wereMoviesFound = computed(() => searchResults.value !== null);

        return {
            searchResults,
            searchMovies,
            title,
            wereMoviesFound,
            hasSearchHappened
        };
    }
});
</script>
<style scoped>
    .container {
        text-align: center;
    }

    input {
        width: 60%;
        margin-bottom: 20px;
    }
</style>