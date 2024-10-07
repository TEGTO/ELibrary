import { ChangeDetectionStrategy, Component, EventEmitter, OnDestroy, OnInit, Output, signal } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { debounceTime, Subject, takeUntil } from 'rxjs';
import { BookFilterRequest, CoverType, dateRangeValidatorFrom, dateRangeValidatorTo, defaultBookFilterRequest as getDefaultBookFilterRequest, rangeMaxValidator, rangeMinValidator, ValidationMessage } from '../../../../shared';

@Component({
  selector: 'app-book-filter',
  templateUrl: './book-filter.component.html',
  styleUrl: './book-filter.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class BookFilterComponent implements OnInit, OnDestroy {
  @Output() filterChange = new EventEmitter<BookFilterRequest>();

  formGroup!: FormGroup;
  readonly panelOpenState = signal(false);

  private destroy$ = new Subject<void>();

  get containsNameInput() { return this.formGroup.get('containsName')! as FormControl; }
  get publicationFromUTCInput() { return this.formGroup.get('publicationFromUTC')! as FormControl; }
  get publicationToUTCInput() { return this.formGroup.get('publicationToUTC')! as FormControl; }
  get minPriceInput() { return this.formGroup.get('minPrice')! as FormControl; }
  get maxPriceInput() { return this.formGroup.get('maxPrice')! as FormControl; }
  get minPageAmountInput() { return this.formGroup.get('minPageAmount')! as FormControl; }
  get maxPageAmountInput() { return this.formGroup.get('maxPageAmount')! as FormControl; }
  get coverTypeInput() { return this.formGroup.get('coverType')! as FormControl; }
  get onlyInStockInput() { return this.formGroup.get('onlyInStock')! as FormControl; }

  constructor(
    private readonly validateInput: ValidationMessage,
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
    this.formGroup = new FormGroup({
      containsName: new FormControl(''),
      publicationFromUTC: new FormControl(null, [dateRangeValidatorFrom('publicationToUTC')]),
      publicationToUTC: new FormControl(null, [dateRangeValidatorTo('publicationFromUTC')]),
      minPrice: new FormControl(null, [rangeMinValidator(0, 'maxPrice')]),
      maxPrice: new FormControl(null, [rangeMaxValidator(Number.MAX_SAFE_INTEGER, 'minPrice')]),
      onlyInStock: new FormControl(false),
      coverType: new FormControl(CoverType.Any, [Validators.required]),
      minPageAmount: new FormControl(null, [rangeMinValidator(0, 'maxPageAmount')]),
      maxPageAmount: new FormControl(null, [rangeMaxValidator(Number.MAX_VALUE, 'minPageAmount')]),
      author: new FormControl(null),
      genre: new FormControl(null),
      publisher: new FormControl(null),
    });
  }
  onFilterChange(): void {
    if (this.formGroup.valid) {
      const formValues = { ...this.formGroup.value };
      const req: BookFilterRequest = {
        ...getDefaultBookFilterRequest(),
        pageNumber: 0,
        pageSize: 0,
        containsName: formValues.containsName || "",
        publicationFrom: formValues.publicationFromUTC,
        publicationTo: formValues.publicationToUTC,
        minPrice: formValues.minPrice,
        maxPrice: formValues.maxPrice,
        coverType: formValues.coverType,
        onlyInStock: formValues.onlyInStock,
        minPageAmount: formValues.minPageAmount,
        maxPageAmount: formValues.maxPageAmount,
        authorId: formValues.author?.id ?? null,
        genreId: formValues.genre?.id ?? null,
        publisherId: formValues.publisher?.id ?? null,
      };
      this.filterChange.emit(req);
    }
  }
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  validateInputField(input: AbstractControl<any, any>) {
    return this.validateInput.getValidationMessage(input);
  }
}
