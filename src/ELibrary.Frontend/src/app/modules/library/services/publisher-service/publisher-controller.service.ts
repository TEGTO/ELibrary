import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { publisherActions, selectPublisherAmount, selectPublishers } from '../..';
import { CreatePublisherRequest, LibraryFilterRequest, PublisherApiService, PublisherResponse, UpdatePublisherRequest } from '../../../shared';
import { PublisherService } from './publisher-service';

@Injectable({
  providedIn: 'root'
})
export class PublisherControllerService implements PublisherService {

  constructor(
    private readonly apiService: PublisherApiService,
    private readonly store: Store,
  ) { }

  getById(id: number): Observable<PublisherResponse> {
    return this.apiService.getById(id);
  }
  getPaginated(request: LibraryFilterRequest): Observable<PublisherResponse[]> {
    this.store.dispatch(publisherActions.getPaginated({ request: request }));
    return this.store.select(selectPublishers);
  }
  getItemTotalAmount(request: LibraryFilterRequest): Observable<number> {
    this.store.dispatch(publisherActions.getTotalAmount({ request: request }));
    return this.store.select(selectPublisherAmount);
  }
  create(request: CreatePublisherRequest): void {
    this.store.dispatch(publisherActions.create({ request: request }));
  }
  update(request: UpdatePublisherRequest): void {
    this.store.dispatch(publisherActions.update({ request: request }));
  }
  deleteById(id: number): void {
    this.store.dispatch(publisherActions.deleteById({ id: id }));
  }
}