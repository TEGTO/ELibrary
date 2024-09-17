import { Injectable } from "@angular/core";
import { LibraryEntityService } from "../../..";
import { CreatePublisherRequest, LibraryFilterRequest, PublisherResponse, UpdatePublisherRequest } from "../../../../shared";

@Injectable({
    providedIn: 'root'
})
export abstract class PublisherService extends LibraryEntityService<
    PublisherResponse,
    LibraryFilterRequest,
    CreatePublisherRequest,
    UpdatePublisherRequest> { }