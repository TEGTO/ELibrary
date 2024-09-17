import { CdkVirtualScrollViewport } from '@angular/cdk/scrolling';
import { AfterViewInit, Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { debounceTime, filter, map, pairwise, Subject, switchMap, takeUntil, throttleTime } from 'rxjs';
import { AuthorService } from '../../../..';
import { AuthorResponse, defaultLibraryFilterRequest, ValidationMessage } from '../../../../../shared';

@Component({
  selector: 'app-author-input',
  templateUrl: './author-input.component.html',
  styleUrl: './author-input.component.scss'
})
export class AuthorInputComponent implements OnInit, OnDestroy, AfterViewInit {
  @Input({ required: true }) formGroup!: FormGroup;
  @ViewChild('scroller') authorScroller!: CdkVirtualScrollViewport;

  readonly itemHeight = 48;
  readonly pageAmount = 12;
  readonly amountItemsInView = 3;

  authors: AuthorResponse[] = [];
  authorPageIndex = 0;
  private fetchedAuthorIds = new Set<number>();

  private destroy$ = new Subject<void>();

  get input() { return this.formGroup.get('author')! as FormControl; }
  get selectionSize() {
    return this.calculateSelectionSize(this.authors.length);
  }

  constructor(
    private readonly authorService: AuthorService,
    private readonly validateInput: ValidationMessage
  ) { }

  ngOnInit(): void {
    this.initializeForm();
    this.initValueChange();
    this.loadAuthors();
  }
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
  ngAfterViewInit(): void {
    this.setupScrollListeners(this.authorScroller, () => this.loadAuthors());
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
          containsName: containsName
        };
        return this.authorService.getPaginated(req);
      })
    ).subscribe(authors => {
      this.fetchedAuthorIds = new Set<number>();
      this.authorPageIndex = 0;
      this.authors = this.getUniqueItems(authors, this.fetchedAuthorIds);
    });
  }
  private initializeForm(): void {
    if (!this.formGroup.contains('author')) {
      this.formGroup.addControl('author', new FormControl("", [Validators.required]));
    }
  }

  private loadAuthors(): void {
    this.authorPageIndex++;
    const containsName = typeof this.input.value === 'string' ? this.input.value : '';
    const req = {
      ...defaultLibraryFilterRequest(),
      pageNumber: this.authorPageIndex,
      pageSize: this.pageAmount,
      containsName: containsName || ''
    };
    this.authorService.getPaginated(req).pipe(
      takeUntil(this.destroy$)
    ).subscribe(authors => {
      this.authors = [...this.authors, ...this.getUniqueItems(authors, this.fetchedAuthorIds)];
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
  displayWith(author?: AuthorResponse): string {
    return author && author?.name && author?.lastName ? `${author.name} ${author.lastName}` : '';
  }
}
