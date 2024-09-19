import { Injectable } from "@angular/core";
import { LibraryEntityService } from "../../..";
import { CreatePublisherRequest, LibraryFilterRequest, Publisher, UpdatePublisherRequest } from "../../../../shared";

@Injectable({
    providedIn: 'root'
})
export abstract class PublisherService extends LibraryEntityService<
    Publisher,
    LibraryFilterRequest,
    CreatePublisherRequest,
    UpdatePublisherRequest> { }