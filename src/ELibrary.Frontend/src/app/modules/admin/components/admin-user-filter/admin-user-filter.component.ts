import { Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';
import { AbstractControl, FormControl, FormGroup } from '@angular/forms';
import { debounceTime, Subject, takeUntil } from 'rxjs';
import { AdminGetUserFilter, getDefaultAdminGetUserFilter, ValidationMessage } from '../../../shared';

@Component({
  selector: 'app-admin-user-filter',
  templateUrl: './admin-user-filter.component.html',
  styleUrl: './admin-user-filter.component.scss'
})
export class AdminUserFilterComponent implements OnInit, OnDestroy {
  @Output() filterChange = new EventEmitter<AdminGetUserFilter>();

  formGroup!: FormGroup;

  private destroy$ = new Subject<void>();

  get containsInfoInput() { return this.formGroup.get('containsInfo')! as FormControl; }

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
      containsInfo: new FormControl(''),
    });
  }
  onFilterChange(): void {
    if (this.formGroup.valid) {
      const formValues = { ...this.formGroup.value };
      const req: AdminGetUserFilter = {
        ...getDefaultAdminGetUserFilter(),
        containsInfo: formValues.containsInfo || "",
      };
      this.filterChange.emit(req);
    }
  }
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  validateInputField(input: AbstractControl<any, any>) {
    return this.validateInput.getValidationMessage(input);
  }
}
