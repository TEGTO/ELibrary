import { BookResponse } from "../../../..";

export interface UpdateBookRequest {
    id: number;
    name: string;
    publicationDate: Date;
    authorId: number;
    genreId: number;
}
export function bookToUpdateRequest(book: BookResponse): UpdateBookRequest {
    return {
        id: book.id,
        name: book.name,
        publicationDate: book.publicationDate,
        authorId: book.author.id,
        genreId: book.genre.id,
    }
}