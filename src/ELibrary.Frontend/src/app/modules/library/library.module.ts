import { ScrollingModule } from '@angular/cdk/scrolling';
import { TextFieldModule } from '@angular/cdk/text-field';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { MatSliderModule } from '@angular/material/slider';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { AuthorChangeDialogComponent, AuthorControllerService, AuthorEffects, AuthorInputComponent, authorReducer, AuthorService, BookChangeDialogComponent, BookControllerService, BookEffects, BookFallbackCoverPipe, BookFilterComponent, BookInputComponent, bookReducer, BookService, CREATE_AUTHOR_COMMAND_HANDLER, CREATE_BOOK_COMMAND_HANDLER, CREATE_GENRE_COMMAND_HANDLER, CREATE_PUBLISHER_COMMAND_HANDLER, CreateAuthorCommandHandlerService, CreateBookCommandHandlerService, CreateGenreCommandHandlerService, CreatePublisherCommandHandlerService, DELETE_AUTHOR_COMMAND_HANDLER, DELETE_BOOK_COMMAND_HANDLER, DELETE_GENRE_COMMAND_HANDLER, DELETE_PUBLISHER_COMMAND_HANDLER, DeleteAuthorCommandHandlerService, DeleteBookCommandHandlerService, DeleteGenreCommandHandlerService, DeletePublisherCommandHandlerService, GenreChangeDialogComponent, GenreControllerService, GenreEffects, GenreInputComponent, genreReducer, GenreService, LibraryDialogManager, LibraryDialogManagerService, LibraryFilterComponent, PublisherChangeDialogComponent, PublisherControllerService, PublisherEffects, PublisherInputComponent, publisherReducer, PublisherService, UPDATE_AUTHOR_COMMAND_HANDLER, UPDATE_BOOK_COMMAND_HANDLER, UPDATE_GENRE_COMMAND_HANDLER, UPDATE_PUBLISHER_COMMAND_HANDLER, UpdateAuthorCommandHandlerService, UpdateBookCommandHandlerService, UpdateGenreCommandHandlerService, UpdatePublisherCommandHandlerService } from '.';

@NgModule({
  declarations: [
    AuthorChangeDialogComponent,
    GenreChangeDialogComponent,
    BookChangeDialogComponent,
    PublisherChangeDialogComponent,
    AuthorInputComponent,
    GenreInputComponent,
    PublisherInputComponent,
    BookFilterComponent,
    LibraryFilterComponent,
    BookInputComponent,
    BookFallbackCoverPipe
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatInputModule,
    ScrollingModule,
    TextFieldModule,
    MatFormFieldModule,
    MatDialogModule,
    MatButtonModule,
    MatSelectModule,
    MatPaginatorModule,
    MatSliderModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatExpansionModule,
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

    { provide: CREATE_AUTHOR_COMMAND_HANDLER, useClass: CreateAuthorCommandHandlerService },
    { provide: UPDATE_AUTHOR_COMMAND_HANDLER, useClass: UpdateAuthorCommandHandlerService },
    { provide: DELETE_AUTHOR_COMMAND_HANDLER, useClass: DeleteAuthorCommandHandlerService },

    { provide: CREATE_GENRE_COMMAND_HANDLER, useClass: CreateGenreCommandHandlerService },
    { provide: UPDATE_GENRE_COMMAND_HANDLER, useClass: UpdateGenreCommandHandlerService },
    { provide: DELETE_GENRE_COMMAND_HANDLER, useClass: DeleteGenreCommandHandlerService },

    { provide: CREATE_PUBLISHER_COMMAND_HANDLER, useClass: CreatePublisherCommandHandlerService },
    { provide: UPDATE_PUBLISHER_COMMAND_HANDLER, useClass: UpdatePublisherCommandHandlerService },
    { provide: DELETE_PUBLISHER_COMMAND_HANDLER, useClass: DeletePublisherCommandHandlerService },

    { provide: CREATE_BOOK_COMMAND_HANDLER, useClass: CreateBookCommandHandlerService },
    { provide: UPDATE_BOOK_COMMAND_HANDLER, useClass: UpdateBookCommandHandlerService },
    { provide: DELETE_BOOK_COMMAND_HANDLER, useClass: DeleteBookCommandHandlerService },
  ],
  exports: [
    AuthorChangeDialogComponent,
    GenreChangeDialogComponent,
    BookChangeDialogComponent,
    BookFilterComponent,
    LibraryFilterComponent,
    BookInputComponent,
    BookFallbackCoverPipe
  ]
})
export class LibraryModule { }
