import { GenreResponse } from "../../../..";

export interface CreateGenreRequest {
    name: string;
}
export function genreToCreateRequest(genre: GenreResponse): CreateGenreRequest {
    return {
        name: genre.name
    }
}