import { Genre } from "../../../..";

export interface GenreResponse {
    id: number;
    name: string;
}

export function mapGenreResponseToGenre(response: GenreResponse): Genre {
    return {
        id: response.id,
        name: response.name,
    }
}