import { ScrollingModule } from '@angular/cdk/scrolling';
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
import { AuthorChangeDialogComponent, AuthorControllerService, AuthorEffects, AuthorInputComponent, authorReducer, AuthorService, BookChangeDialogComponent, BookControllerService, BookEffects, BookFilterComponent, BookInputComponent, bookReducer, BookService, GenreChangeDialogComponent, GenreControllerService, GenreEffects, GenreInputComponent, genreReducer, GenreService, LibraryCommand, LibraryCommandService, LibraryDialogManager, LibraryDialogManagerService, LibraryFilterComponent, PublisherChangeDialogComponent, PublisherControllerService, PublisherEffects, PublisherInputComponent, publisherReducer, PublisherService } from '.';

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
    { provide: LibraryCommand, useClass: LibraryCommandService },
  ],
  exports: [AuthorChangeDialogComponent, GenreChangeDialogComponent, BookChangeDialogComponent, BookFilterComponent, LibraryFilterComponent, BookInputComponent]
})
export class LibraryModule { }
