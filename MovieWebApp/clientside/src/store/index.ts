import { createStore } from 'vuex';
import axios from 'axios';
import { MovieDetails } from '../Models/MovieDetails';
import { MovieSearchResult } from '../Models/MovieSearchResult';
import { State } from '../Models/State';
import { apiService } from '../services/apiService';


const state: State = {
    recentSearches: [],
    searchResults: null,
    movieDetails: null
};

export default createStore<State>({
    state,
    mutations: {
        setRecentSearches(state, searches: string[]) {
            state.recentSearches = searches;
        },
        setSearchResults(state, results: MovieSearchResult[] | null) {
            state.searchResults = results;
        },
        setMovieDetails(state, details: MovieDetails) {
            state.movieDetails = details;
        }
    },
    actions: {
        async fetchRecentSearches({ commit }) {
            try {
                const recentSearches = await apiService.fetchRecentSearches();
                commit('setRecentSearches', recentSearches);
            }
            catch (error: any) {
                console.error(error);
            }
        },
        async searchMovies({ commit }, title: string) {
            try {
                const searchResults = await apiService.searchMovies(title);
                commit('setSearchResults', searchResults);
            }
            catch (error: any) {
                console.error(error);
                commit('setSearchResults', null);
            }
        },
        async fetchMovieDetails({ commit }, imdbId: string) {
            try
            {
                const response = await apiService.fetchMovieDetails(imdbId);
                commit('setMovieDetails', response);
            }
            catch(error: any)
            {
                console.error(error);
            }
        }
    },
    modules: {},
    getters: {
        movieDetails: state => state.movieDetails,
    },
});
