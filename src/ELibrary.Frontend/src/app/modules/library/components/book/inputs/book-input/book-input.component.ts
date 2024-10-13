import { ChangeDetectionStrategy, Component } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseSelectInputComponent, BookService } from '../../../..';
import { Book, defaultBookFilterRequest, ValidationMessage } from '../../../../../shared';

@Component({
  selector: 'app-book-input',
  templateUrl: './book-input.component.html',
  styleUrl: './book-input.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class BookInputComponent extends BaseSelectInputComponent<Book> {

  constructor(
    private readonly bookService: BookService,
    protected override readonly validateInput: ValidationMessage
  ) {
    super(validateInput);
  }

  getControlName(): string {
    return 'book';
  }

  fetchItems(containsName: string, pageIndex: number, pageSize: number): Observable<Book[]> {
    const req = {
      ...defaultBookFilterRequest(),
      pageNumber: pageIndex,
      pageSize: pageSize,
      containsName: containsName
    };
    return this.bookService.getPaginated(req);
  }

  displayWith(book?: Book): string {
    return book ? `${book.name}` : '';
  }

  trackByBook(index: number, book: Book): number {
    return book.id;
  }
}
