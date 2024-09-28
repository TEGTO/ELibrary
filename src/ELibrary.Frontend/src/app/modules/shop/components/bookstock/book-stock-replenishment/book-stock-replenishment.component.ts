import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { environment } from '../../../../../../environment/environment';
import { getDefaultStockBookChange, StockBookChange, ValidationMessageService } from '../../../../shared';

@Component({
  selector: 'app-book-stock-replenishment',
  templateUrl: './book-stock-replenishment.component.html',
  styleUrl: './book-stock-replenishment.component.scss'
})
export class BookStockReplenishmentComponent implements OnInit {
  items: FormGroup[] = [];
  hasBeenTriedSubmitted = false;

  get maxAmount() { return environment.maxOrderAmount; }

  constructor(
    private readonly validateInput: ValidationMessageService,
    private readonly dialogRef: MatDialogRef<BookStockReplenishmentComponent>,
  ) { }

  ngOnInit(): void {
    this.addFormGroup();
  }

  addFormGroup() {
    this.items = [...this.items,
    new FormGroup({
      amount: new FormControl(1, [Validators.required, Validators.max(this.maxAmount)]),
    })
    ]
  }
  deleteFormGroup(formGroup: FormGroup) {
    this.items = this.items.filter(x => x !== formGroup);
  }
  validateInputField(field: string, formGroup: FormGroup) {
    const input = formGroup.get(field);

    if (!input) {
      return { hasError: false, message: "" };
    }

    return this.validateInput.getValidationMessage(input);
  }
  submitForms() {
    const allFormsValid = this.items.every(x => x.valid);
    if (allFormsValid) {
      const changes: StockBookChange[] = this.items.map(formGroup => ({
        ...getDefaultStockBookChange(),
        book: formGroup.get('book')?.value,
        changeAmount: formGroup.get('amount')?.value
      }));
      this.dialogRef.close(changes);
    }
    this.items.forEach(x => x.markAllAsTouched());
  }
}
