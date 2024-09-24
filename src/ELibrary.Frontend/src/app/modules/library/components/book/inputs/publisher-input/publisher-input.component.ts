import { ChangeDetectionStrategy, Component } from '@angular/core';
import { Observable } from 'rxjs';
import { PublisherService } from '../../../..';
import { Publisher, ValidationMessage, defaultLibraryFilterRequest } from '../../../../../shared';
import { BaseSelectInputComponent } from "../base-select-input-component/base-select-input-component.component";

@Component({
  selector: 'app-publisher-input',
  templateUrl: './publisher-input.component.html',
  styleUrl: './publisher-input.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PublisherInputComponent extends BaseSelectInputComponent<Publisher> {

  constructor(
    private readonly publisherService: PublisherService,
    protected override readonly validateInput: ValidationMessage
  ) {
    super(validateInput);
  }

  getControlName(): string {
    return 'publisher';
  }

  fetchItems(containsName: string, pageIndex: number, pageSize: number): Observable<Publisher[]> {
    const req = {
      ...defaultLibraryFilterRequest(),
      pageNumber: pageIndex,
      pageSize: pageSize,
      containsName: containsName
    };
    return this.publisherService.getPaginated(req);
  }

  displayWith(publisher?: Publisher): string {
    return publisher ? publisher.name : '';
  }

  trackByPublisher(index: number, publisher: Publisher): number {
    return publisher.id;
  }
}