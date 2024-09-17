import { CdkVirtualScrollViewport } from '@angular/cdk/scrolling';
import { AfterViewInit, Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { Subject, debounceTime, filter, map, pairwise, switchMap, takeUntil, throttleTime } from 'rxjs';
import { PublisherService } from '../../../..';
import { PublisherResponse, ValidationMessage, defaultLibraryFilterRequest } from '../../../../../shared';

@Component({
  selector: 'app-publisher-input',
  templateUrl: './publisher-input.component.html',
  styleUrl: './publisher-input.component.scss'
})
export class PublisherInputComponent implements OnInit, OnDestroy, AfterViewInit {
  @Input({ required: true }) formGroup!: FormGroup;
  @ViewChild('scroller') publisherScroller!: CdkVirtualScrollViewport;

  readonly itemHeight = 48;
  readonly pageAmount = 12;
  readonly amountItemsInView = 3;

  publishers: PublisherResponse[] = [];
  publisherPageIndex = 0;
  private fetchedPublisherIds = new Set<number>();

  private destroy$ = new Subject<void>();

  get input() { return this.formGroup.get('publisher')! as FormControl; }
  get selectionSize() {
    return this.calculateSelectionSize(this.publishers.length);
  }

  constructor(
    private readonly publisherService: PublisherService,
    private readonly validateInput: ValidationMessage
  ) { }

  ngOnInit(): void {
    this.initializeForm();
    this.initValueChange();
    this.loadPublishers();
  }
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
  ngAfterViewInit(): void {
    this.setupScrollListeners(this.publisherScroller, () => this.loadPublishers());
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
        return this.publisherService.getPaginated(req);
      })
    ).subscribe(publishers => {
      this.fetchedPublisherIds = new Set<number>();
      this.publisherPageIndex = 0;
      this.publishers = this.getUniqueItems(publishers, this.fetchedPublisherIds);
    });
  }
  private initializeForm(): void {
    if (!this.formGroup.contains('publisher')) {
      this.formGroup.addControl('publisher', new FormControl("", [Validators.required]));
    }
  }

  private loadPublishers(): void {
    this.publisherPageIndex++;
    const containsName = typeof this.input.value === 'string' ? this.input.value : '';
    const req = {
      ...defaultLibraryFilterRequest(),
      pageNumber: this.publisherPageIndex,
      pageSize: this.pageAmount,
      containsName: containsName || ''
    };

    this.publisherService.getPaginated(req).pipe(
      takeUntil(this.destroy$)
    ).subscribe(publishers => {
      this.publishers = [...this.publishers, ...this.getUniqueItems(publishers, this.fetchedPublisherIds)];
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
  displayWith(publisher?: PublisherResponse): string {
    return publisher ? `${publisher.name}` : '';
  }
}
