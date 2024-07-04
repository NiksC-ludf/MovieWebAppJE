import axios from 'axios';
import { MovieDetails } from '../Models/MovieDetails';
import { MovieSearchResult } from '../Models/MovieSearchResult';

const API_BASE_URL = '/api/movie';

export const apiService = {
    async fetchRecentSearches(): Promise<string[]> {
        const response = await axios.get(`${API_BASE_URL}/recent-searches`);
        return response.data;
    },
    async searchMovies(title: string): Promise<MovieSearchResult[]> { 
        const response = await axios.get(`${API_BASE_URL}/search?title=${title}`);
        return response.data as MovieSearchResult[];
    },
    async fetchMovieDetails(imdbId: string): Promise<MovieDetails> {
        const response = await axios.get(`${API_BASE_URL}/details?imdbId=${imdbId}`);
        return response.data as MovieDetails;
    }
};
