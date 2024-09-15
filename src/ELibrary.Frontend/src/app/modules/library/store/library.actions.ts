import { createAction, props } from "@ngrx/store";
import { AuthorResponse, BookFilterRequest, BookResponse, CreateAuthorRequest, CreateBookRequest, CreateGenreRequest, CreatePublisherRequest, GenreResponse, LibraryFilterRequest, PublisherResponse, UpdateAuthorRequest, UpdateBookRequest, UpdateGenreRequest, UpdatePublisherRequest } from "../../shared";

function createEntityActions<CreateEntityRequest, UpdateEntityRequest, EntityResponse, FilterRequest>(entityName: string) {
    const capitalizedEntityName = entityName.charAt(0).toUpperCase() + entityName.slice(1);

    return {
        getPaginated: createAction(
            `[Library] Get Paginated ${capitalizedEntityName}s`,
            props<{ request: FilterRequest }>()
        ),
        getPaginatedSuccess: createAction(
            `[Library] Get Paginated ${capitalizedEntityName}s Success`,
            props<{ entities: EntityResponse[] }>()
        ),
        getPaginatedFailure: createAction(
            `[Library] Get Paginated ${capitalizedEntityName}s Failure`,
            props<{ error: any }>()
        ),

        getTotalAmount: createAction(
            `[Library] Get Total ${capitalizedEntityName}s Amount`,
            props<{ request: FilterRequest }>()
        ),
        getTotalAmountSuccess: createAction(
            `[Library] Get Total ${capitalizedEntityName}s Amount Success`,
            props<{ amount: number }>()
        ),
        getTotalAmountFailure: createAction(
            `[Library] Get Total ${capitalizedEntityName}s Amount Failure`,
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
            props<{ entity: EntityResponse }>()
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

export const authorActions = createEntityActions<CreateAuthorRequest, UpdateAuthorRequest, AuthorResponse, LibraryFilterRequest>('author');

export const genreActions = createEntityActions<CreateGenreRequest, UpdateGenreRequest, GenreResponse, LibraryFilterRequest>('genre');

export const publisherActions = createEntityActions<CreatePublisherRequest, UpdatePublisherRequest, PublisherResponse, LibraryFilterRequest>('genre');

export const bookActions = createEntityActions<CreateBookRequest, UpdateBookRequest, BookResponse, BookFilterRequest>('book');