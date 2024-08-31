import { Observable } from "rxjs";
import { PaginatedRequest } from "../../..";

export interface LibraryEntityApi<Response, Create, Update> {
    getById(id: number): Observable<Response>;
    getPaginated(request: PaginatedRequest): Observable<Response[]>;
    create(request: Create): Observable<Response>;
    update(request: Update): Observable<Object>;
    deleteById(id: number): Observable<Object>;
}