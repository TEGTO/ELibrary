import { HttpClientTestingModule } from "@angular/common/http/testing";
import { TestBed } from "@angular/core/testing";
import { provideMockActions } from "@ngrx/effects/testing";
import { Observable, of, throwError } from "rxjs";
import { AuthorApiService, AuthorResponse, CreateAuthorRequest, UpdateAuthorRequest } from "../../shared";
import { authorActions } from "./library.actions";
import { AuthorEffects } from "./library.effects";

describe('AuthorEffects', () => {
    let actions$: Observable<any>;
    let effects: AuthorEffects;
    let mockApiService: jasmine.SpyObj<AuthorApiService>;

    beforeEach(() => {
        mockApiService = jasmine.createSpyObj('AuthorApiService', ['getPaginated', 'getItemTotalAmount', 'create', 'update', 'deleteById']);

        TestBed.configureTestingModule({
            imports: [HttpClientTestingModule],
            providers: [
                AuthorEffects,
                provideMockActions(() => actions$),
                { provide: AuthorApiService, useValue: mockApiService }
            ]
        });

        effects = TestBed.inject(AuthorEffects);
    });

    describe('getPaginated$', () => {
        it('should dispatch getPaginatedSuccess on successful getPaginated', (done) => {
            const paginatedRequest = { pageNumber: 1, pageSize: 10 };
            const entities: AuthorResponse[] = [{ id: 1, name: 'Author1', lastName: 'Lastname1', dateOfBirth: new Date() }];
            const action = authorActions.getPaginated({ request: paginatedRequest });
            const outcome = authorActions.getPaginatedSuccess({ entities });

            actions$ = of(action);
            mockApiService.getPaginated.and.returnValue(of(entities));

            effects.getPaginated$.subscribe(result => {
                expect(result).toEqual(outcome);
                expect(mockApiService.getPaginated).toHaveBeenCalledWith(paginatedRequest);
                done();
            });
        });

        it('should dispatch getPaginatedFailure on failed getPaginated', (done) => {
            const paginatedRequest = { pageNumber: 1, pageSize: 10 };
            const action = authorActions.getPaginated({ request: paginatedRequest });
            const error = new Error('Error!');
            const outcome = authorActions.getPaginatedFailure({ error: error.message });

            actions$ = of(action);
            mockApiService.getPaginated.and.returnValue(throwError(error));

            effects.getPaginated$.subscribe(result => {
                expect(result).toEqual(outcome);
                expect(mockApiService.getPaginated).toHaveBeenCalledWith(paginatedRequest);
                done();
            });
        });
    });

    describe('getTotalAmount$', () => {
        it('should dispatch getTotalAmountSuccess on successful getItemTotalAmount', (done) => {
            const amount = 100;
            const action = authorActions.getTotalAmount();
            const outcome = authorActions.getTotalAmountSuccess({ amount });

            actions$ = of(action);
            mockApiService.getItemTotalAmount.and.returnValue(of(amount));

            effects.getTotalAmount$.subscribe(result => {
                expect(result).toEqual(outcome);
                expect(mockApiService.getItemTotalAmount).toHaveBeenCalled();
                done();
            });
        });

        it('should dispatch getTotalAmountFailure on failed getItemTotalAmount', (done) => {
            const action = authorActions.getTotalAmount();
            const error = new Error('Error!');
            const outcome = authorActions.getTotalAmountFailure({ error: error.message });

            actions$ = of(action);
            mockApiService.getItemTotalAmount.and.returnValue(throwError(error));

            effects.getTotalAmount$.subscribe(result => {
                expect(result).toEqual(outcome);
                expect(mockApiService.getItemTotalAmount).toHaveBeenCalled();
                done();
            });
        });
    });

    describe('create$', () => {
        it('should dispatch createSuccess on successful create', (done) => {
            const createRequest: CreateAuthorRequest = { name: 'Author1', lastName: "Lastname1", dateOfBirth: new Date() };
            const entity: AuthorResponse = { id: 1, name: 'Author1', lastName: 'Lastname1', dateOfBirth: new Date() };
            const action = authorActions.create({ request: createRequest });
            const outcome = authorActions.createSuccess({ entity });

            actions$ = of(action);
            mockApiService.create.and.returnValue(of(entity));

            effects.create$.subscribe(result => {
                expect(result).toEqual(outcome);
                expect(mockApiService.create).toHaveBeenCalledWith(createRequest);
                done();
            });
        });

        it('should dispatch createFailure on failed create', (done) => {
            const createRequest: CreateAuthorRequest = { name: 'Author1', lastName: "Lastname1", dateOfBirth: new Date() };
            const action = authorActions.create({ request: createRequest });
            const error = new Error('Error!');
            const outcome = authorActions.createFailure({ error: error.message });

            actions$ = of(action);
            mockApiService.create.and.returnValue(throwError(error));

            effects.create$.subscribe(result => {
                expect(result).toEqual(outcome);
                expect(mockApiService.create).toHaveBeenCalledWith(createRequest);
                done();
            });
        });
    });

    describe('update$', () => {
        it('should dispatch updateSuccess on successful update', (done) => {
            const updateRequest: UpdateAuthorRequest = { id: 1, name: 'UpdatedAuthor', lastName: 'UpdatedLastname', dateOfBirth: new Date() };
            const entity: AuthorResponse = { id: 1, name: 'Author1', lastName: 'Lastname1', dateOfBirth: new Date() };
            const action = authorActions.update({ request: updateRequest });
            const outcome = authorActions.updateSuccess({ entity: entity });

            actions$ = of(action);
            mockApiService.update.and.returnValue(of(entity));

            effects.update$.subscribe(result => {
                expect(result).toEqual(outcome);
                expect(mockApiService.update).toHaveBeenCalledWith(updateRequest);
                done();
            });
        });

        it('should dispatch updateFailure on failed update', (done) => {
            const updateRequest: UpdateAuthorRequest = { id: 1, name: 'UpdatedAuthor', lastName: 'UpdatedLastname', dateOfBirth: new Date() };
            const action = authorActions.update({ request: updateRequest });
            const error = new Error('Error!');
            const outcome = authorActions.updateFailure({ error: error.message });

            actions$ = of(action);
            mockApiService.update.and.returnValue(throwError(error));

            effects.update$.subscribe(result => {
                expect(result).toEqual(outcome);
                expect(mockApiService.update).toHaveBeenCalledWith(updateRequest);
                done();
            });
        });
    });

    describe('deleteById$', () => {
        it('should dispatch deleteByIdSuccess on successful deleteById', (done) => {
            const id = 1;
            const action = authorActions.deleteById({ id });
            const outcome = authorActions.deleteByIdSuccess({ id });

            actions$ = of(action);
            mockApiService.deleteById.and.returnValue(of({}));

            effects.deleteById$.subscribe(result => {
                expect(result).toEqual(outcome);
                expect(mockApiService.deleteById).toHaveBeenCalledWith(id);
                done();
            });
        });

        it('should dispatch deleteByIdFailure on failed deleteById', (done) => {
            const id = 1;
            const action = authorActions.deleteById({ id });
            const error = new Error('Error!');
            const outcome = authorActions.deleteByIdFailure({ error: error.message });

            actions$ = of(action);
            mockApiService.deleteById.and.returnValue(throwError(error));

            effects.deleteById$.subscribe(result => {
                expect(result).toEqual(outcome);
                expect(mockApiService.deleteById).toHaveBeenCalledWith(id);
                done();
            });
        });
    });
});