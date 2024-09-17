import { Component, EventEmitter, OnDestroy, OnInit, Output, signal } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { debounceTime, Subject, takeUntil } from 'rxjs';
import { dateRangeValidatorFrom, dateRangeValidatorTo, pageAmountRangeMaxValidator, pageAmountRangeMinValidator, priceRangeMaxValidator, priceRangeMinValidator } from '../../..';
import { BookFilterRequest, CoverType, defaultBookFilterRequest, ValidationMessage } from '../../../../shared';

@Component({
  selector: 'app-book-filter',
  templateUrl: './book-filter.component.html',
  styleUrl: './book-filter.component.scss'
})
export class BookFilterComponent implements OnInit, OnDestroy {
  @Output() filterChange = new EventEmitter<BookFilterRequest>();

  formGroup!: FormGroup;
  readonly panelOpenState = signal(false);

  private destroy$ = new Subject<void>();

  get containsNameInput() { return this.formGroup.get('containsName')! as FormControl; }
  get publicationFromUTCInput() { return this.formGroup.get('publicationFromUTC')!; }
  get publicationToUTCInput() { return this.formGroup.get('publicationToUTC')!; }
  get minPriceInput() { return this.formGroup.get('minPrice')!; }
  get maxPriceInput() { return this.formGroup.get('maxPrice')!; }
  get minPageAmountInput() { return this.formGroup.get('minPageAmount')!; }
  get maxPageAmountInput() { return this.formGroup.get('maxPageAmount')!; }

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
      minPrice: new FormControl(null, [priceRangeMinValidator('maxPrice')]),
      maxPrice: new FormControl(null, [priceRangeMaxValidator('minPrice')]),
      onlyInStock: new FormControl(false),
      coverType: new FormControl(CoverType.Any, [Validators.required]),
      minPageAmount: new FormControl(null, [pageAmountRangeMinValidator('maxPageAmount')]),
      maxPageAmount: new FormControl(null, [pageAmountRangeMaxValidator('minPageAmount')]),
      author: new FormControl(null),
      genre: new FormControl(null),
      publisher: new FormControl(null),
    });
  }
  onFilterChange(): void {
    if (this.formGroup.valid) {
      const formValues = { ...this.formGroup.value };
      const req: BookFilterRequest = {
        ...defaultBookFilterRequest(),
        pageNumber: 0,
        pageSize: 0,
        containsName: formValues.containsName || "",
        publicationFromUTC: formValues.publicationFromUTC,
        publicationToUTC: formValues.publicationToUTC,
        minPrice: formValues.minPrice,
        maxPrice: formValues.maxPrice,
        coverType: formValues.coverType,
        onlyInStock: formValues.onlyInStock,
        minPageAmount: formValues.minPageAmount,
        maxPageAmount: formValues.maxPageAmount,
        authorId: formValues.author?.id,
        genreId: formValues.genre?.id,
        publisherId: formValues.publisher?.id,
      };
      this.filterChange.emit(req);
    }
  }
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  validateInputField(input: AbstractControl<any, any>) {
    return this.validateInput.getValidationMessage(input);
  }
}
