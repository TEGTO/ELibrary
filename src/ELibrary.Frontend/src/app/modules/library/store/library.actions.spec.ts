// import { Author, CreateAuthorRequest, LibraryFilterRequest, UpdateAuthorRequest } from "../../shared";
// import { authorActions } from "./library.actions";

// const error = { message: 'An error occurred' };
// describe('Author Actions', () => {
//     // Get Paginated
//     it('should create getPaginated action for authors', () => {
//         const request: LibraryFilterRequest = { containsName: "", pageNumber: 1, pageSize: 10 };
//         const action = authorActions.getPaginated({ request: request });
//         expect(action.type).toBe('[Library] Get Paginated Authors');
//         expect(action.request).toBe(request);
//     });

//     it('should create getPaginatedSuccess action for authors', () => {
//         const entities: Author[] = [{ id: 1, name: 'John', lastName: 'Doe', dateOfBirth: new Date() }];
//         const action = authorActions.getPaginatedSuccess({ entities });
//         expect(action.type).toBe('[Library] Get Paginated Authors Success');
//         expect(action.entities).toBe(entities);
//     });

//     it('should create getPaginatedFailure action for authors', () => {
//         const action = authorActions.getPaginatedFailure({ error });
//         expect(action.type).toBe('[Library] Get Paginated Authors Failure');
//         expect(action.error).toEqual(error);
//     });

//     // Get Total Amount
//     it('should create getTotalAmount action for authors', () => {
//         const request: LibraryFilterRequest = { containsName: "", pageNumber: 1, pageSize: 10 };
//         const action = authorActions.getTotalAmount({ request: request });
//         expect(action.type).toBe('[Library] Get Total Authors Amount');
//     });

//     it('should create getTotalAmountSuccess action for authors', () => {
//         const amount = 42;
//         const action = authorActions.getTotalAmountSuccess({ amount });
//         expect(action.type).toBe('[Library] Get Total Authors Amount Success');
//         expect(action.amount).toBe(amount);
//     });

//     it('should create getTotalAmountFailure action for authors', () => {
//         const action = authorActions.getTotalAmountFailure({ error });
//         expect(action.type).toBe('[Library] Get Total Authors Amount Failure');
//         expect(action.error).toEqual(error);
//     });

//     // Create
//     it('should create create action for authors', () => {
//         const request: CreateAuthorRequest = { name: 'John', lastName: 'Doe', dateOfBirth: new Date() };
//         const action = authorActions.create({ request });
//         expect(action.type).toBe('[Library] Create New Author');
//         expect(action.request).toBe(request);
//     });

//     it('should create createSuccess action for authors', () => {
//         const entity: Author = { id: 1, name: 'John', lastName: 'Doe', dateOfBirth: new Date() };
//         const action = authorActions.createSuccess({ entity });
//         expect(action.type).toBe('[Library] Create New Author Success');
//         expect(action.entity).toBe(entity);
//     });

//     it('should create createFailure action for authors', () => {
//         const action = authorActions.createFailure({ error });
//         expect(action.type).toBe('[Library] Create New Author Failure');
//         expect(action.error).toEqual(error);
//     });

//     // Update
//     it('should create update action for authors', () => {
//         const request: UpdateAuthorRequest = { id: 1, name: 'John', lastName: 'Doe', dateOfBirth: new Date() };
//         const action = authorActions.update({ request });
//         expect(action.type).toBe('[Library] Update Author');
//         expect(action.request).toBe(request);
//     });

//     it('should create updateSuccess action for authors', () => {
//         const request: Author = { id: 1, name: 'John', lastName: 'Doe', dateOfBirth: new Date() };
//         const action = authorActions.updateSuccess({ entity: request });
//         expect(action.type).toBe('[Library] Update Author Success');
//         expect(action.entity).toBe(request);
//     });

//     it('should create updateFailure action for authors', () => {
//         const action = authorActions.updateFailure({ error });
//         expect(action.type).toBe('[Library] Update Author Failure');
//         expect(action.error).toEqual(error);
//     });

//     // Delete by ID
//     it('should create deleteById action for authors', () => {
//         const id = 1;
//         const action = authorActions.deleteById({ id });
//         expect(action.type).toBe('[Library] Delete Author By Id');
//         expect(action.id).toBe(id);
//     });

//     it('should create deleteByIdSuccess action for authors', () => {
//         const id = 1;
//         const action = authorActions.deleteByIdSuccess({ id });
//         expect(action.type).toBe('[Library] Delete Author By Id Success');
//         expect(action.id).toBe(id);
//     });

//     it('should create deleteByIdFailure action for authors', () => {
//         const action = authorActions.deleteByIdFailure({ error });
//         expect(action.type).toBe('[Library] Delete Author By Id Failure');
//         expect(action.error).toEqual(error);
//     });
// });

// // Tests for book and genre are the same