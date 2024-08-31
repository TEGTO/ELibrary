import { Component, OnDestroy, OnInit } from '@angular/core';
import { Observable, Subject, takeUntil, tap } from 'rxjs';
import { AuthorService, LibraryDialogManager, TableColumn } from '../../..';
import { AuthorResponse, authorToCreateRequest, authorToUpdateRequest, LocalizedDatePipe, PaginatedRequest } from '../../../../shared';

@Component({
  selector: 'author-table',
  templateUrl: './author-table.component.html',
  styleUrl: './author-table.component.scss'
})
export class AuthorTableComponent implements OnInit, OnDestroy {

  pageSize: number = 10;

  items$!: Observable<AuthorResponse[]>;
  private destroy$ = new Subject<void>();

  columns: TableColumn[] = [
    { header: 'Name', field: 'name' },
    { header: 'Last Name', field: 'lastName' },
    { header: 'Date of Birth', field: 'dateOfBirth', pipe: new LocalizedDatePipe('en-US') }
  ];

  constructor(private readonly dialogManager: LibraryDialogManager, private readonly libraryEntityService: AuthorService) { }

  ngOnInit(): void {
    this.pageChange(1);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  pageChange(item: any) {
    let pageIndex = item as number;
    let req: PaginatedRequest = {
      pageNumber: pageIndex,
      pageSize: this.pageSize
    }
    this.items$ = this.libraryEntityService.getAuthorsPaginated(req);
  }
  createNew() {
    let author: AuthorResponse = {
      id: 0,
      name: "",
      lastName: "",
      dateOfBirth: new Date()
    }
    this.dialogManager.openAuthorDetailsMenu(author).afterClosed().pipe(
      takeUntil(this.destroy$)
    ).subscribe(author => {
      if (author) {
        let req = authorToCreateRequest(author);
        this.libraryEntityService.createAuthor(req);
      }
    });
  }
  update(item: any) {
    let author = item as AuthorResponse;
    this.dialogManager.openAuthorDetailsMenu(author).afterClosed().pipe(
      takeUntil(this.destroy$)
    ).subscribe(author => {
      if (author) {
        let req = authorToUpdateRequest(author);
        this.libraryEntityService.updateAuthor(req);
      }
    });
  }
  delete(item: any) {
    let author = item as AuthorResponse;
    this.dialogManager.openConfirmMenu().afterClosed().pipe(
      tap(result => {
        if (result === true) {
          this.libraryEntityService.deleteAuthorById(author.id);
        }
      }),
      takeUntil(this.destroy$)
    ).subscribe();
  }
}