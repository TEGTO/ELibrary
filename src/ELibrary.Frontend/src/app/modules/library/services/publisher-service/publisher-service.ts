import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { CreatePublisherRequest, LibraryFilterRequest, PublisherResponse, UpdatePublisherRequest } from "../../../shared";

@Injectable({
    providedIn: 'root'
})
export abstract class PublisherService {
    abstract getById(id: number): Observable<PublisherResponse>;
    abstract getPaginated(request: LibraryFilterRequest): Observable<PublisherResponse[]>;
    abstract getItemTotalAmount(request: LibraryFilterRequest): Observable<number>;
    abstract create(request: CreatePublisherRequest): void;
    abstract update(request: UpdatePublisherRequest): void;
    abstract deleteById(id: number): void;
}