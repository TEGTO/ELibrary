
export interface UpdateBookRequest {
    id: number;
    title: string;
    publicationDate: Date;
    authorId: number;
    genreId: number;
}