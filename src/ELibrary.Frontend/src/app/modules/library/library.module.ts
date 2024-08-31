import { ScrollingModule } from '@angular/cdk/scrolling';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSelectModule } from '@angular/material/select';
import { RouterModule, Routes } from '@angular/router';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { AuthorChangeDialogComponent, AuthorServiceControllerService as AuthorControllerService, AuthorEffects, AuthorService, AuthorTableComponent, BookServiceControllerService as BookControllerService, BookEffects, BookService, BookTableComponent, ConfirmMenuComponent, GenericTableComponent, GenreServiceControllerService as GenreControllerService, GenreEffects, GenreService, GenreTableComponent, LibraryDialogManager, LibraryDialogManagerService, libraryReducer, LibraryTablesComponent } from '.';
import { BookChangeDialogComponent } from './components/book/book-change-dialog/book-change-dialog.component';
import { GenreChangeDialogComponent } from './components/genre/genre-change-dialog/genre-change-dialog.component';

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
    GenreChangeDialogComponent,
    BookChangeDialogComponent,
  ],
  imports: [
    RouterModule.forChild(routes),
    MatDialogModule,
    MatInputModule,
    FormsModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    MatButtonModule,
    ScrollingModule,
    MatSelectModule,
    CommonModule,
    MatButtonModule,
    MatPaginatorModule,
    MatDatepickerModule,
    StoreModule.forFeature('library', libraryReducer),
    EffectsModule.forFeature([AuthorEffects, BookEffects, GenreEffects]),
  ],
  providers: [
    { provide: LibraryDialogManager, useClass: LibraryDialogManagerService },
    { provide: BookService, useClass: BookControllerService },
    { provide: AuthorService, useClass: AuthorControllerService },
    { provide: GenreService, useClass: GenreControllerService },
  ],
  exports: [LibraryTablesComponent]
})
export class LibraryModule { }
