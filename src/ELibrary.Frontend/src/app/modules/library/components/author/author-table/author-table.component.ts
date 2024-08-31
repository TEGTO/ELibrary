import { Component, OnDestroy } from '@angular/core';
import { Subject, takeUntil, tap } from 'rxjs';
import { LibraryDialogManager } from '../../..';
import { AuthorResponse } from '../../../../shared';

@Component({
  selector: 'author-table',
  templateUrl: './author-table.component.html',
  styleUrl: './author-table.component.scss'
})
export class AuthorTableComponent implements OnDestroy {
  private destroy$ = new Subject<void>();

  items = [
    { name: 'Steve', lastName: 'Jobs', dateOfBirth: '1955-02-24' },
    { name: 'Bill', lastName: 'Gates', dateOfBirth: '1955-10-28' },
    { name: 'Steve', lastName: 'Wozniak', dateOfBirth: '1950-08-11' },
  ];

  columns = [
    { header: 'Name', field: 'name' },
    { header: 'Last Name', field: 'lastName' },
    { header: 'Date of Birth', field: 'dateOfBirth' }
  ];

  constructor(private readonly dialogManager: LibraryDialogManager) { }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  createNewAuthor() {
    let author: AuthorResponse = {
      id: 0,
      name: "",
      lastName: "",
      dateOfBirth: new Date()
    }
    this.dialogManager.openDetailsMenu(author).afterClosed().pipe(
      takeUntil(this.destroy$)
    ).subscribe();
  }
  updateAuthor(item: any) {
    let author = item as AuthorResponse;
    this.dialogManager.openDetailsMenu(author).afterClosed().pipe(
      takeUntil(this.destroy$)
    ).subscribe();
  }
  deleteAuthor(item: any) {
    let author = item as AuthorResponse;
    this.dialogManager.openConfirmMenu().afterClosed().pipe(
      tap(result => {
        if (result === true) {
        }
      }),
      takeUntil(this.destroy$)
    ).subscribe();
  }
}
