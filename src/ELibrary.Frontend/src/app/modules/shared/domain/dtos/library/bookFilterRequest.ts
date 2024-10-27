import { CoverType, LibraryFilterRequest } from "../../..";

export enum BookSorting {
    MostPopular, LeastPopular
}

export interface BookFilterRequest extends LibraryFilterRequest {
    publicationFrom: Date | null;
    publicationTo: Date | null;
    minPrice: number | null;
    maxPrice: number | null;
    coverType: CoverType | null;
    onlyInStock: boolean | null;
    minPageAmount: number | null;
    maxPageAmount: number | null;
    authorId: number | null;
    genreId: number | null;
    publisherId: number | null;
    sorting: BookSorting | null;
}

export function defaultBookFilterRequest(): BookFilterRequest {
    return {
        containsName: "",
        pageNumber: 0,
        pageSize: 0,
        publicationFrom: null,
        publicationTo: null,
        minPrice: null,
        maxPrice: null,
        coverType: null,
        onlyInStock: false,
        minPageAmount: null,
        maxPageAmount: null,
        authorId: null,
        genreId: null,
        publisherId: null,
        sorting: null,
    };
}