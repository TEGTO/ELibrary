import { createAction, props } from "@ngrx/store";
import { AuthorResponse, CreateAuthorRequest, PaginatedRequest, UpdateAuthorRequest } from "../../../shared";

export const getPaginatedAuthors = createAction(
    '[Library] Get Paginated Authors',
    props<{ request: PaginatedRequest }>()
);
export const getPaginatedAuthorsSuccess = createAction(
    '[Library] Get Paginated Authors Success',
    props<{ authors: AuthorResponse[] }>()
);
export const getPaginatedAuthorsFailure = createAction(
    '[Library] Get Paginated Authors Failure',
    props<{ error: any }>()
);

export const createAuthor = createAction(
    '[Library] Create New Author',
    props<{ request: CreateAuthorRequest }>()
);
export const createAuthorSuccess = createAction(
    '[Library] Create New Author Success',
    props<{ author: AuthorResponse }>()
);
export const createAuthorFailure = createAction(
    '[Library] Create New Author Failure',
    props<{ error: any }>()
);

export const updateAuthor = createAction(
    '[Library] Update Author',
    props<{ request: UpdateAuthorRequest }>()
);
export const updateAuthorSuccess = createAction(
    '[Library] Update Author Success',
    props<{ author: AuthorResponse }>()
);
export const updateAuthorFailure = createAction(
    '[Library] Update Author Failure',
    props<{ error: any }>()
);

export const deleteAuthorById = createAction(
    '[Library] Delete Author By Id',
    props<{ id: number }>()
);
export const deleteAuthorByIdSuccess = createAction(
    '[Library] Delete Author By Id Success',
    props<{ id: number }>()
);
export const deleteAuthorByIdFailure = createAction(
    '[Library] Delete Author By Id Failure',
    props<{ error: any }>()
);