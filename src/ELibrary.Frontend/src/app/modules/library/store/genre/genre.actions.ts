import { createAction, props } from "@ngrx/store";
import { CreateGenreRequest, GenreResponse, PaginatedRequest, UpdateGenreRequest } from "../../../shared";

export const getPaginatedGenres = createAction(
    '[Library] Get Paginated Genres',
    props<{ request: PaginatedRequest }>()
);
export const getPaginatedGenresSuccess = createAction(
    '[Library] Get Paginated Genres Success',
    props<{ genres: GenreResponse[] }>()
);
export const getPaginatedGenresSuccessFailure = createAction(
    '[Library] Get Paginated Genres Failure',
    props<{ error: any }>()
);

export const createGenre = createAction(
    '[Library] Create New Genre',
    props<{ request: CreateGenreRequest }>()
);
export const createGenreSuccess = createAction(
    '[Library] Create New Genre Success',
    props<{ genre: GenreResponse }>()
);
export const createGenreFailure = createAction(
    '[Library] Create New Genre Failure',
    props<{ error: any }>()
);

export const updateGenre = createAction(
    '[Library] Update Genre',
    props<{ request: UpdateGenreRequest }>()
);
export const updateGenreSuccess = createAction(
    '[Library] Update Genre Success',
    props<{ genre: GenreResponse }>()
);
export const updateGenreFailure = createAction(
    '[Library] Update Genre Failure',
    props<{ error: any }>()
);

export const deleteGenreById = createAction(
    '[Library] Delete Genre By Id',
    props<{ id: number }>()
);
export const deleteGenreByIdSuccess = createAction(
    '[Library] Delete Genre By Id Success',
    props<{ id: number }>()
);
export const deleteGenreByIdFailure = createAction(
    '[Library] Delete Genre By Id Failure',
    props<{ error: any }>()
);