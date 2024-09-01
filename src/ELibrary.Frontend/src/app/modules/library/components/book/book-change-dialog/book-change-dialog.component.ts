import { CdkVirtualScrollViewport } from '@angular/cdk/scrolling';
import { AfterViewInit, Component, Inject, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { filter, map, pairwise, Subject, takeUntil, throttleTime } from 'rxjs';
import { AuthorService, GenreService } from '../../..';
import { AuthorResponse, BookResponse, GenreResponse } from '../../../../shared';

@Component({
  selector: 'book-change-dialog',
  templateUrl: './book-change-dialog.component.html',
  styleUrl: './book-change-dialog.component.scss'
})
export class BookChangeDialogComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild('authorScroller') authorScroller!: CdkVirtualScrollViewport;
  @ViewChild('genreScroller') genreScroller!: CdkVirtualScrollViewport;

  readonly itemHeight = 45;
  readonly pageAmount = 12;
  readonly amountItemsInView = 3;

  formGroup!: FormGroup;

  authors: AuthorResponse[] = [];
  genres: GenreResponse[] = [];

  authorPageIndex = 0;
  genrePageIndex = 0;

  private fetchedAuthorIds = new Set<number>();
  private fetchedGenreIds = new Set<number>();

  private destroy$ = new Subject<void>();

  get titleInput() { return this.formGroup.get('title')!; }
  get authorInput() { return this.formGroup.get('author')!; }
  get genreInput() { return this.formGroup.get('genre')!; }
  get publicationDateInput() { return this.formGroup.get('publicationDate')!; }
  get authorSelectionSize() {
    return this.calculateSelectionSize(this.authors.length);
  }
  get genreSelectionSize() {
    return this.calculateSelectionSize(this.genres.length);
  }

  constructor(
    @Inject(MAT_DIALOG_DATA) private readonly book: BookResponse,
    private readonly dialogRef: MatDialogRef<BookChangeDialogComponent>,
    private readonly authorService: AuthorService,
    private readonly genreService: GenreService
  ) { }

  ngOnInit(): void {
    this.initializeForm();
    this.loadAuthors();
    this.loadGenres();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  ngAfterViewInit(): void {
    this.setupScrollListeners(this.authorScroller, () => this.loadAuthors());
    this.setupScrollListeners(this.genreScroller, () => this.loadGenres());
  }

  initializeForm(): void {
    this.formGroup = new FormGroup({
      title: new FormControl(this.book.title, [Validators.required, Validators.maxLength(256)]),
      publicationDate: new FormControl(this.book.publicationDate, [Validators.required]),
      author: new FormControl(this.book.author.id, [Validators.required, Validators.min(1)]),
      genre: new FormControl(this.book.genre.id, [Validators.required, Validators.min(1)])
    });
  }

  sendDetails(): void {
    if (this.formGroup.valid) {
      const formValues = { ...this.formGroup.value };
      const authorId = formValues.author;
      const genreId = formValues.genre;

      const updatedBook: BookResponse = {
        id: this.book.id,
        title: formValues.title,
        publicationDate: formValues.publicationDate,
        author: this.findItemById(this.authors, authorId, this.book.author),
        genre: this.findItemById(this.genres, genreId, this.book.genre),
      };

      this.dialogRef.close(updatedBook);
    }
  }

  loadAuthors(): void {
    this.authorPageIndex++;
    const req = {
      pageNumber: this.authorPageIndex,
      pageSize: this.pageAmount
    };

    this.authorService.getAuthorsPaginated(req).pipe(
      takeUntil(this.destroy$)
    ).subscribe(authors => {
      this.authors = [...this.authors, ...this.getUniqueItems(authors, this.fetchedAuthorIds)];
    });
  }

  loadGenres(): void {
    this.genrePageIndex++;
    const req = {
      pageNumber: this.genrePageIndex,
      pageSize: this.pageAmount
    };

    this.genreService.getGenresPaginated(req).pipe(
      takeUntil(this.destroy$)
    ).subscribe(genres => {
      this.genres = [...this.genres, ...this.getUniqueItems(genres, this.fetchedGenreIds)];
    });
  }

  setupScrollListeners(scroller: CdkVirtualScrollViewport, loadMoreCallback: () => void): void {
    scroller.elementScrolled().pipe(
      map(() => scroller.measureScrollOffset('bottom')),
      pairwise(),
      filter(([previous, current]) => current < previous && current < 2 * this.itemHeight),
      throttleTime(200),
      takeUntil(this.destroy$)
    ).subscribe(() => loadMoreCallback());
  }

  getUniqueItems<T extends { id: number }>(items: T[], fetchedIds: Set<number>): T[] {
    const uniqueItems: T[] = [];
    items.forEach(item => {
      if (!fetchedIds.has(item.id)) {
        fetchedIds.add(item.id);
        uniqueItems.push(item);
      }
    });
    return uniqueItems;
  }

  calculateSelectionSize(length: number): number {
    return length > this.amountItemsInView
      ? this.amountItemsInView * this.itemHeight
      : length * this.itemHeight + 5; //small margin for empty
  }

  findItemById<T>(items: T[], id: number, fallback: T): T {
    return items.find(item => (item as any).id === id) || fallback;
  }
}