import { createAction, props } from "@ngrx/store";
import { AuthorResponse, BookResponse, CreateAuthorRequest, CreateBookRequest, CreateGenreRequest, GenreResponse, PaginatedRequest, UpdateAuthorRequest, UpdateBookRequest, UpdateGenreRequest } from "../../shared";

function createEntityActions<CreateEntityRequest, UpdateEntityRequest, EntityResponse>(entityName: string) {
    const capitalizedEntityName = entityName.charAt(0).toUpperCase() + entityName.slice(1);

    return {
        getPaginated: createAction(
            `[Library] Get Paginated ${capitalizedEntityName}s`,
            props<{ request: PaginatedRequest }>()
        ),
        getPaginatedSuccess: createAction(
            `[Library] Get Paginated ${capitalizedEntityName}s Success`,
            props<{ entities: EntityResponse[] }>()
        ),
        getPaginatedFailure: createAction(
            `[Library] Get Paginated ${capitalizedEntityName}s Failure`,
            props<{ error: any }>()
        ),

        create: createAction(
            `[Library] Create New ${capitalizedEntityName}`,
            props<{ request: CreateEntityRequest }>()
        ),
        createSuccess: createAction(
            `[Library] Create New ${capitalizedEntityName} Success`,
            props<{ entity: EntityResponse }>()
        ),
        createFailure: createAction(
            `[Library] Create New ${capitalizedEntityName} Failure`,
            props<{ error: any }>()
        ),

        update: createAction(
            `[Library] Update ${capitalizedEntityName}`,
            props<{ request: UpdateEntityRequest }>()
        ),
        updateSuccess: createAction(
            `[Library] Update ${capitalizedEntityName} Success`,
            props<{ entity: UpdateEntityRequest }>()
        ),
        updateFailure: createAction(
            `[Library] Update ${capitalizedEntityName} Failure`,
            props<{ error: any }>()
        ),

        deleteById: createAction(
            `[Library] Delete ${capitalizedEntityName} By Id`,
            props<{ id: number }>()
        ),
        deleteByIdSuccess: createAction(
            `[Library] Delete ${capitalizedEntityName} By Id Success`,
            props<{ id: number }>()
        ),
        deleteByIdFailure: createAction(
            `[Library] Delete ${capitalizedEntityName} By Id Failure`,
            props<{ error: any }>()
        )
    };
}

export const authorActions = createEntityActions<CreateAuthorRequest, UpdateAuthorRequest, AuthorResponse>('author');

export const bookActions = createEntityActions<CreateBookRequest, UpdateBookRequest, BookResponse>('book');

export const genreActions = createEntityActions<CreateGenreRequest, UpdateGenreRequest, GenreResponse>('genre');