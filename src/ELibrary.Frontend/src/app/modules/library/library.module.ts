import { ScrollingModule } from '@angular/cdk/scrolling';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { AuthorChangeDialogComponent, AuthorControllerService, AuthorEffects, AuthorInputComponent, authorReducer, AuthorService, BookChangeDialogComponent, BookControllerService, BookEffects, bookReducer, BookService, GenreChangeDialogComponent, GenreControllerService, GenreEffects, GenreInputComponent, genreReducer, GenreService, LibraryCommand, LibraryCommandService, LibraryDialogManager, LibraryDialogManagerService, PublisherChangeDialogComponent, PublisherControllerService, PublisherEffects, PublisherInputComponent, publisherReducer, PublisherService } from '.';

@NgModule({
  declarations: [
    AuthorChangeDialogComponent,
    GenreChangeDialogComponent,
    BookChangeDialogComponent,
    PublisherChangeDialogComponent,
    AuthorInputComponent,
    GenreInputComponent,
    PublisherInputComponent,
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
    MatNativeDateModule,
    MatRadioModule,
    MatAutocompleteModule,
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
  exports: [AuthorChangeDialogComponent, GenreChangeDialogComponent, BookChangeDialogComponent]
})
export class LibraryModule { }
