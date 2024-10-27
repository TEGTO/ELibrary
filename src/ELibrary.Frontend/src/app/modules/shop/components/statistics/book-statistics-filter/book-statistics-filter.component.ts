import { ChangeDetectionStrategy, Component, EventEmitter, OnDestroy, OnInit, Output, signal } from '@angular/core';
import { AbstractControl, FormControl, FormGroup } from '@angular/forms';
import { debounceTime, Subject, takeUntil } from 'rxjs';
import { environment } from '../../../../../../environment/environment';
import { Book, CurrencyPipeApplier, dateRangeWithTimeValidatorFrom, dateRangeWithTimeValidatorTo, GetBookStatistics, getDateOrNull, getDefaultGetBookStatistics, getProductInfoPath, inputSelectValidator, ValidationMessage } from '../../../../shared';

@Component({
  selector: 'app-book-statistics-filter',
  templateUrl: './book-statistics-filter.component.html',
  styleUrl: './book-statistics-filter.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class BookStatisticsFilterComponent implements OnInit, OnDestroy {
  @Output() filterChange = new EventEmitter<GetBookStatistics>();

  readonly itemHeight = 200;
  readonly scollSize = 420;
  readonly panelOpenState = signal(false);
  formGroup!: FormGroup;
  bookFormControl!: FormControl;
  includeBooks: Book[] = [];

  private readonly destroy$ = new Subject<void>();

  get fromUTCDateInput() { return this.formGroup.get('fromUTC')! as FormControl; }
  get fromTimeInput() { return this.formGroup.get('fromTime')! as FormControl; }
  get toUTCDateInput() { return this.formGroup.get('toUTC')! as FormControl; }
  get toTimeInput() { return this.formGroup.get('toTime')! as FormControl; }

  constructor(
    private readonly validateInput: ValidationMessage,
    private readonly currenctyApplier: CurrencyPipeApplier,
  ) { }

  ngOnInit(): void {
    this.initializeForm();

    this.formGroup.valueChanges.pipe(
      takeUntil(this.destroy$),
      debounceTime(100)
    ).subscribe(() => {
      this.onFilterChange();
    });
  }
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  initializeForm(): void {
    this.bookFormControl = new FormControl("", [inputSelectValidator]);

    this.bookFormControl.valueChanges
      .pipe(takeUntil(this.destroy$))
      .subscribe((newValue) => {
        if (this.bookFormControl.valid && newValue) {
          if (!this.includeBooks.some(book => book.id === newValue.id)) {
            this.includeBooks = [newValue, ... this.includeBooks];
          }
        }
      });

    this.formGroup = new FormGroup({
      fromUTC: new FormControl(null, [dateRangeWithTimeValidatorFrom('fromUTC', 'fromTime', 'toUTC', 'toTime')]),
      fromTime: new FormControl(null, [dateRangeWithTimeValidatorFrom('fromUTC', 'fromTime', 'toUTC', 'toTime')]),
      toUTC: new FormControl(null, [dateRangeWithTimeValidatorTo('fromUTC', 'fromTime', 'toUTC', 'toTime')]),
      toTime: new FormControl(null, [dateRangeWithTimeValidatorTo('fromUTC', 'fromTime', 'toUTC', 'toTime')]),
      book: this.bookFormControl,
    });
  }
  onFilterChange(): void {
    if (this.formGroup.valid) {
      const formValues = { ...this.formGroup.value };

      const req: GetBookStatistics = {
        ...getDefaultGetBookStatistics(),
        fromUTC: getDateOrNull(formValues.fromUTC, formValues.fromTime),
        toUTC: getDateOrNull(formValues.toUTC, formValues.toTime),
        includeBooks: this.includeBooks
      };
      this.filterChange.emit(req);
    }
  }
  deleteBook(book: Book) {
    this.includeBooks = this.includeBooks.filter(x => x.id !== book.id);
    this.onFilterChange();
  }
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  validateInputField(input: AbstractControl<any, any>) {
    return this.validateInput.getValidationMessage(input);
  }
  calculateSelectionSize(): number {
    return this.scollSize;
  }
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  applyCurrencyPipe(value: any): any {
    return this.currenctyApplier.applyCurrencyPipe(value);
  }
  getBookPage(book: Book): string[] {
    return [`/${getProductInfoPath(book.id)}`];
  }
  trackById(index: number, book: Book): number {
    return book.id;
  }
  onErrorImage(event: Event) {
    const element = event.target as HTMLImageElement;
    element.src = environment.bookCoverPlaceholder;
  }
}
