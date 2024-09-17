import { UpdateGenreRequest } from "./updateGenreRequest";

export interface GenreResponse {
    id: number;
    name: string;
}
export function getGenreFromUpdateRequest(request: UpdateGenreRequest): GenreResponse {
    return {
        id: request.id,
        name: request.name,
    }
}
export function getDefaultGenreResponse(): GenreResponse {
    return {
        id: 0,
        name: "",
    }
}