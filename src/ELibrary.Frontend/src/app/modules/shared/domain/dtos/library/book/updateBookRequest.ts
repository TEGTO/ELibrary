import { Book, CoverType } from "../../../..";

export interface UpdateBookRequest {
    id: number;
    name: string;
    publicationDate: Date;
    price: number;
    coverType: CoverType;
    pageAmount: number;
    stockAmount: number;
    coverImgUrl: string,
    authorId: number;
    genreId: number;
    publisherId: number;
}
export function mapBookToUpdateBookRequest(book: Book): UpdateBookRequest {
    return {
        id: book.id,
        name: book.name,
        publicationDate: book.publicationDate,
        price: book.price,
        coverType: book.coverType,
        pageAmount: book.pageAmount,
        stockAmount: book.stockAmount,
        coverImgUrl: book.coverImgUrl,
        authorId: book.author.id,
        genreId: book.genre.id,
        publisherId: book.publisher.id
    }
}