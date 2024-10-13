import { Genre } from "../../../..";

export interface UpdateGenreRequest {
    id: number;
    name: string;
}
export function mapGenreToUpdateGenreRequest(genre: Genre): UpdateGenreRequest {
    return {
        id: genre.id,
        name: genre.name
    }
}