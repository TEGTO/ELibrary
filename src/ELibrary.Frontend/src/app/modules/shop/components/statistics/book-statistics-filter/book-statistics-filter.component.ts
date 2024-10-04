import { Component, EventEmitter, OnDestroy, OnInit, Output, signal } from '@angular/core';
import { AbstractControl, FormControl, FormGroup } from '@angular/forms';
import { debounceTime, Subject, takeUntil } from 'rxjs';
import { dateRangeValidatorFrom, dateRangeValidatorTo, GetBookStatistics, getDefaultGetBookStatistics, ValidationMessage } from '../../../../shared';

@Component({
  selector: 'app-book-statistics-filter',
  templateUrl: './book-statistics-filter.component.html',
  styleUrl: './book-statistics-filter.component.scss'
})
export class BookStatisticsFilterComponent implements OnInit, OnDestroy {
  @Output() filterChange = new EventEmitter<GetBookStatistics>();

  formGroup!: FormGroup;
  readonly panelOpenState = signal(false);

  private destroy$ = new Subject<void>();

  get fromUTCInput() { return this.formGroup.get('fromUTC')!; }
  get toUTCInput() { return this.formGroup.get('toUTC')!; }

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
      fromUTC: new FormControl(null, [dateRangeValidatorFrom('toUTC')]),
      toUTC: new FormControl(null, [dateRangeValidatorTo('fromUTC')]),
    });
  }
  onFilterChange(): void {
    if (this.formGroup.valid) {
      const formValues = { ...this.formGroup.value };
      const req: GetBookStatistics = {
        ...getDefaultGetBookStatistics(),
        fromUTC: formValues.fromUTC,
        toUTC: formValues.toUTC,
      };
      this.filterChange.emit(req);
    }
  }
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  validateInputField(input: AbstractControl<any, any>) {
    return this.validateInput.getValidationMessage(input);
  }
}
