import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { publisherActions, selectPublisherAmount, selectPublishers } from '../../..';
import { CreatePublisherRequest, LibraryFilterRequest, Publisher, PublisherApiService, UpdatePublisherRequest } from '../../../../shared';
import { BaseControllerService } from '../base-entity-service/base-entity.service';
import { PublisherService } from './publisher-service';

@Injectable({
  providedIn: 'root'
})
export class PublisherControllerService extends BaseControllerService<
  Publisher,
  LibraryFilterRequest,
  CreatePublisherRequest,
  UpdatePublisherRequest
> implements PublisherService {
  constructor(
    apiService: PublisherApiService,
    store: Store
  ) {
    super(apiService, store, publisherActions, {
      selectItems: selectPublishers,
      selectTotalAmount: selectPublisherAmount
    });
  }
}