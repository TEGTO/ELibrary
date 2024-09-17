import { CdkVirtualScrollViewport } from '@angular/cdk/scrolling';
import { AfterViewInit, Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { Subject, debounceTime, filter, map, pairwise, switchMap, takeUntil, throttleTime } from 'rxjs';
import { GenreService } from '../../../..';
import { GenreResponse, ValidationMessage, defaultLibraryFilterRequest } from '../../../../../shared';

@Component({
  selector: 'app-genre-input',
  templateUrl: './genre-input.component.html',
  styleUrl: './genre-input.component.scss'
})
export class GenreInputComponent implements OnInit, OnDestroy, AfterViewInit {
  @Input({ required: true }) formGroup!: FormGroup;
  @ViewChild('scroller') genreScroller!: CdkVirtualScrollViewport;

  readonly itemHeight = 48;
  readonly pageAmount = 12;
  readonly amountItemsInView = 3;

  genres: GenreResponse[] = [];
  genrePageIndex = 0;
  private fetchedGenreIds = new Set<number>();

  private destroy$ = new Subject<void>();

  get input() { return this.formGroup.get('genre')! as FormControl; }
  get selectionSize() {
    return this.calculateSelectionSize(this.genres.length);
  }

  constructor(
    private readonly genreService: GenreService,
    private readonly validateInput: ValidationMessage
  ) { }

  ngOnInit(): void {
    this.initializeForm();
    this.initValueChange();
    this.loadGenres();
  }
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
  ngAfterViewInit(): void {
    this.setupScrollListeners(this.genreScroller, () => this.loadGenres());
  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  validateInputField(input: AbstractControl<any, any>) {
    return this.validateInput.getValidationMessage(input);
  }
  private initValueChange() {
    this.input.valueChanges.pipe(
      takeUntil(this.destroy$),
      debounceTime(50),
      switchMap(value => {
        const containsName = typeof value === 'string' ? value : '';
        const req = {
          ...defaultLibraryFilterRequest(),
          pageNumber: 1,
          pageSize: this.pageAmount,
          containsName: containsName || ''
        };
        return this.genreService.getPaginated(req);
      })
    ).subscribe(genres => {
      this.fetchedGenreIds = new Set<number>();
      this.genrePageIndex = 0;
      this.genres = this.getUniqueItems(genres, this.fetchedGenreIds);
    });
  }
  private initializeForm(): void {
    if (!this.formGroup.contains('genre')) {
      this.formGroup.addControl('genre', new FormControl("", [Validators.required]));
    }
  }

  private loadGenres(): void {
    this.genrePageIndex++;
    const containsName = typeof this.input.value === 'string' ? this.input.value : '';
    const req = {
      ...defaultLibraryFilterRequest(),
      pageNumber: this.genrePageIndex,
      pageSize: this.pageAmount,
      containsName: containsName || ''
    };

    this.genreService.getPaginated(req).pipe(
      takeUntil(this.destroy$)
    ).subscribe(genres => {
      this.genres = [...this.genres, ...this.getUniqueItems(genres, this.fetchedGenreIds)];
    });
  }

  private setupScrollListeners(scroller: CdkVirtualScrollViewport, loadMoreCallback: () => void): void {
    scroller.elementScrolled().pipe(
      map(() => scroller.measureScrollOffset('bottom')),
      pairwise(),
      filter(([previous, current]) => current < previous && current < 2 * this.itemHeight),
      throttleTime(200),
      takeUntil(this.destroy$)
    ).subscribe(() => loadMoreCallback());
  }

  private getUniqueItems<T extends { id: number }>(items: T[], fetchedIds: Set<number>): T[] {
    const uniqueItems: T[] = [];
    items.forEach(item => {
      if (!fetchedIds.has(item.id)) {
        fetchedIds.add(item.id);
        uniqueItems.push(item);
      }
    });
    return uniqueItems;
  }

  private calculateSelectionSize(length: number): number {
    return length > this.amountItemsInView
      ? this.amountItemsInView * this.itemHeight
      : length * this.itemHeight + 5;
  }
  displayWith(genre?: GenreResponse): string {
    return genre ? `${genre.name}` : '';
  }

}