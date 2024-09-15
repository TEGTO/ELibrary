import { HttpResponse } from "@angular/common/http";
import { Observable } from "rxjs";

export interface LibraryEntityApi<Response, Create, Update, Filter> {
    getById(id: number): Observable<Response>;
    getPaginated(request: Filter): Observable<Response[]>;
    getItemTotalAmount(request: Filter): Observable<number>;
    create(request: Create): Observable<Response>;
    update(request: Update): Observable<Response>;
    deleteById(id: number): Observable<HttpResponse<void>>;
}