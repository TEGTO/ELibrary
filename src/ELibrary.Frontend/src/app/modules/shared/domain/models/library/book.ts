import { Author, Genre, getDefaultAuthor, getDefaultGenre, getDefaultPublisher, Publisher } from "../../..";

export enum CoverType {
    Any = 0, Hard = 1, Soft = 2
}
export interface Book {
    id: number;
    name: string;
    publicationDate: Date;
    price: number;
    coverType: CoverType;
    pageAmount: number;
    stockAmount: number;
    author: Author;
    genre: Genre;
    publisher: Publisher;
}
export function getDefaultBook(): Book {
    return {
        id: 0,
        name: "",
        publicationDate: new Date(),
        price: 0,
        coverType: CoverType.Hard,
        pageAmount: 0,
        stockAmount: 0,
        author: getDefaultAuthor(),
        genre: getDefaultGenre(),
        publisher: getDefaultPublisher()
    }
}