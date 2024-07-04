import { MovieDetails } from './MovieDetails'
import { MovieSearchResult } from './MovieSearchResult'

export interface State {
    recentSearches: string[];
    searchResults: MovieSearchResult[] | null;
    movieDetails: MovieDetails | null;
}