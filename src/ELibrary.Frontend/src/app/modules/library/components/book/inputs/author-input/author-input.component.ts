import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthorService } from '../../../..';
import { AuthorResponse, defaultLibraryFilterRequest, ValidationMessage } from '../../../../../shared';
import { BaseSelectInputComponent } from "../base-select-input-component/base-select-input-component.component";

@Component({
  selector: 'app-author-input',
  templateUrl: './author-input.component.html',
  styleUrl: './author-input.component.scss'
})
export class AuthorInputComponent extends BaseSelectInputComponent<AuthorResponse> {

  constructor(
    private readonly authorService: AuthorService,
    protected override readonly validateInput: ValidationMessage
  ) {
    super(validateInput);
  }

  getControlName(): string {
    return 'author';
  }

  fetchItems(containsName: string, pageIndex: number, pageSize: number): Observable<AuthorResponse[]> {
    const req = {
      ...defaultLibraryFilterRequest(),
      pageNumber: pageIndex,
      pageSize: pageSize,
      containsName: containsName
    };
    return this.authorService.getPaginated(req);
  }

  displayWith(author?: AuthorResponse): string {
    return author && author.name && author.lastName ? `${author.name} ${author.lastName}` : '';
  }

  trackByAuthor(index: number, author: AuthorResponse): number {
    return author.id;
  }
}