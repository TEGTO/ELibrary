import { Component, OnDestroy, OnInit } from '@angular/core';
import { map, Observable, Subject, takeUntil } from 'rxjs';
import { AuthorService, LibraryDialogManager, TableColumn } from '../../..';
import { AuthorResponse, authorToCreateRequest, authorToUpdateRequest, LocalizedDatePipe, PaginatedRequest } from '../../../../shared';

@Component({
  selector: 'author-table',
  templateUrl: './author-table.component.html',
  styleUrl: './author-table.component.scss'
})
export class AuthorTableComponent implements OnInit, OnDestroy {
  items$!: Observable<AuthorResponse[]>;
  totalAmount$!: Observable<number>;
  private destroy$ = new Subject<void>();

  columns: TableColumn[] = [
    { header: 'Name', field: 'name' },
    { header: 'Last Name', field: 'lastName' },
    { header: 'Date of Birth', field: 'dateOfBirth', pipe: new LocalizedDatePipe('en-US') }
  ];

  constructor(private readonly dialogManager: LibraryDialogManager, private readonly libraryEntityService: AuthorService) { }

  ngOnInit(): void {
    this.totalAmount$ = this.libraryEntityService.getItemTotalAmount();
    this.pageChange({ pageIndex: 1, pageSize: 10 });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  pageChange(item: any) {
    let pageParams = item as { pageIndex: number, pageSize: number };
    let req: PaginatedRequest = {
      pageNumber: pageParams.pageIndex,
      pageSize: pageParams.pageSize
    }
    this.items$ = this.libraryEntityService.getAuthorsPaginated(req).pipe(
      map(items => items.slice(0, pageParams.pageSize))
    );
  }
  createNew() {
    let entity: AuthorResponse = {
      id: 0,
      name: "",
      lastName: "",
      dateOfBirth: new Date()
    }
    this.dialogManager.openAuthorDetailsMenu(entity).afterClosed().pipe(
      takeUntil(this.destroy$)
    ).subscribe(author => {
      if (author) {
        let req = authorToCreateRequest(author);
        this.libraryEntityService.createAuthor(req);
      }
    });
  }
  update(item: any) {
    let entity = item as AuthorResponse;
    this.dialogManager.openAuthorDetailsMenu(entity).afterClosed().pipe(
      takeUntil(this.destroy$)
    ).subscribe(author => {
      if (author) {
        let req = authorToUpdateRequest(author);
        this.libraryEntityService.updateAuthor(req);
      }
    });
  }
  delete(item: any) {
    let entity = item as AuthorResponse;
    this.dialogManager.openConfirmMenu().afterClosed().pipe(
      takeUntil(this.destroy$)
    ).subscribe(result => {
      if (result === true) {
        this.libraryEntityService.deleteAuthorById(entity.id);
      }
    });
  }
}