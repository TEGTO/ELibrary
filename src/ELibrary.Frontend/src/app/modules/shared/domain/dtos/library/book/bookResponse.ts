import { AuthorResponse } from "../author/authorResponse";
import { GenreResponse } from "../genre/genreResponse";

export interface BookResponse {
    id: number;
    title: string;
    publicationDate: Date;
    author: AuthorResponse;
    genre: GenreResponse;
}