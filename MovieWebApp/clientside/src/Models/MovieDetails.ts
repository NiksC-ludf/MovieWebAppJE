export interface MovieDetails {
    title: string;
    year: string;
    rated: string;
    released: string;
    runtime: string;
    genre: string;
    director: string;
    actors: string;
    plot: string;
    language: string;
    country: string;
    poster: string;
    ratings: Array<{ Source: string; Value: string }>;
    imdbRating: number | null;
    imdbID: string;
    boxOffice: string | null;
    response: string;
}
