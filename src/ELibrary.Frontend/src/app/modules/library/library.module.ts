import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule } from '@angular/material/paginator';
import { RouterModule, Routes } from '@angular/router';
import { AuthorChangeDialogComponent, AuthorTableComponent, BookTableComponent, GenericTableComponent, GenreTableComponent, LibraryDialogManager, LibraryDialogManagerService, LibraryTablesComponent } from '.';
import { ConfirmMenuComponent } from './components/confirm-menu/confirm-menu.component';

const routes: Routes = [
  {
    path: "", component: LibraryTablesComponent,
    children: [
      {
        path: "", redirectTo: "books", pathMatch: "full"
      },
      { path: "books", component: BookTableComponent },
      { path: "genres", component: GenreTableComponent },
      { path: "authors", component: AuthorTableComponent }
    ]
  }
];
@NgModule({
  declarations: [
    LibraryTablesComponent,
    GenreTableComponent,
    BookTableComponent,
    AuthorTableComponent,
    GenericTableComponent,
    AuthorChangeDialogComponent,
    ConfirmMenuComponent,
  ],
  imports: [
    RouterModule.forChild(routes),
    MatDialogModule,
    MatInputModule,
    FormsModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    MatButtonModule,
    CommonModule,
    MatButtonModule,
    MatPaginatorModule,
    MatDatepickerModule,
  ],
  providers: [
    { provide: LibraryDialogManager, useClass: LibraryDialogManagerService },
  ],
  exports: [LibraryTablesComponent]
})
export class LibraryModule { }
