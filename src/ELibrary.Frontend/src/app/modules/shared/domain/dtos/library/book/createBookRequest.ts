import { Book, CoverType } from "../../../..";

export interface CreateBookRequest {
    name: string;
    publicationDate: Date;
    price: number;
    coverType: CoverType;
    pageAmount: number;
    coverImgUrl: string,
    description: string | null,
    authorId: number;
    genreId: number;
    publisherId: number;
}
export function mapBookToCreateBookRequest(book: Book): CreateBookRequest {
    return {
        name: book.name,
        publicationDate: book.publicationDate,
        price: book.price,
        coverType: book.coverType,
        pageAmount: book.pageAmount,
        coverImgUrl: book.coverImgUrl,
        description: book.description,
        authorId: book.author.id,
        genreId: book.genre.id,
        publisherId: book.publisher.id
    }
}