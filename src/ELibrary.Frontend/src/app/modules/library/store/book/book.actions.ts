import { createAction, props } from "@ngrx/store";
import { BookResponse, CreateBookRequest, PaginatedRequest, UpdateBookRequest } from "../../../shared";

export const getPaginatedBooks = createAction(
    '[Library] Get Paginated Books',
    props<{ request: PaginatedRequest }>()
);
export const getPaginatedBooksSuccess = createAction(
    '[Library] Get Paginated Books Success',
    props<{ books: BookResponse[] }>()
);
export const getPaginatedBooksSuccessFailure = createAction(
    '[Library] Get Paginated Books Failure',
    props<{ error: any }>()
);

export const createBook = createAction(
    '[Library] Create New Book',
    props<{ request: CreateBookRequest }>()
);
export const createBookSuccess = createAction(
    '[Library] Create New Book Success',
    props<{ book: BookResponse }>()
);
export const createBookFailure = createAction(
    '[Library] Create New Book Failure',
    props<{ error: any }>()
);

export const updateBook = createAction(
    '[Library] Update Book',
    props<{ request: UpdateBookRequest }>()
);
export const updateBookSuccess = createAction(
    '[Library] Update Book Success',
    props<{ book: BookResponse }>()
);
export const updateBookFailure = createAction(
    '[Library] Update Book Failure',
    props<{ error: any }>()
);

export const deleteBookById = createAction(
    '[Library] Delete Book By Id',
    props<{ id: number }>()
);
export const deleteBookByIdSuccess = createAction(
    '[Library] Delete Book By Id Success',
    props<{ id: number }>()
);
export const deleteBookByIdFailure = createAction(
    '[Library] Delete Book By Id Failure',
    props<{ error: any }>()
);