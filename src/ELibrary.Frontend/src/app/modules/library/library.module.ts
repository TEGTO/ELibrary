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
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { AuthorChangeDialogComponent, AuthorControllerService, AuthorEffects, authorReducer, AuthorService, AuthorTableComponent, BookChangeDialogComponent, BookControllerService, BookEffects, bookReducer, BookService, BookTableComponent, ConfirmMenuComponent, GenericTableComponent, GenreChangeDialogComponent, GenreControllerService, GenreEffects, genreReducer, GenreService, GenreTableComponent, LibraryCommand, LibraryCommandService, LibraryDialogManager, LibraryDialogManagerService, LibraryTablesComponent, PublisherControllerService, PublisherEffects, publisherReducer, PublisherService } from '.';

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
    StoreModule.forFeature('author', authorReducer),
    StoreModule.forFeature('genre', genreReducer),
    StoreModule.forFeature('publisher', publisherReducer),
    StoreModule.forFeature('book', bookReducer),
    EffectsModule.forFeature([AuthorEffects, GenreEffects, PublisherEffects, BookEffects]),
  ],
  providers: [
    { provide: LibraryDialogManager, useClass: LibraryDialogManagerService },
    { provide: BookService, useClass: BookControllerService },
    { provide: AuthorService, useClass: AuthorControllerService },
    { provide: GenreService, useClass: GenreControllerService },
    { provide: PublisherService, useClass: PublisherControllerService },
    { provide: LibraryCommand, useClass: LibraryCommandService },
  ],
  exports: [LibraryTablesComponent]
})
export class LibraryModule { }
