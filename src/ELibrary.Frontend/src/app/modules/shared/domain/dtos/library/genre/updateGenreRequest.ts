import { GenreResponse } from "../../../..";

export interface UpdateGenreRequest {
    id: number;
    name: string;
}
export function genreToUpdateRequest(genre: GenreResponse): UpdateGenreRequest {
    return {
        id: genre.id,
        name: genre.name
    }
}