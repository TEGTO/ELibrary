import { Genre } from "../../../..";

export interface CreateGenreRequest {
    name: string;
}
export function mapGenreToCreateGenreRequest(genre: Genre): CreateGenreRequest {
    return {
        name: genre.name
    }
}