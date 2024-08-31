
export interface CreateBookRequest {
    title: string;
    publicationDate: Date;
    authorId: number;
    genreId: number;
}