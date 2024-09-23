import { Component, Inject, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { noSpaces, notEmptyString, Publisher, ValidationMessage } from '../../../../shared';

@Component({
  selector: 'app-publisher-change-dialog',
  templateUrl: './publisher-change-dialog.component.html',
  styleUrl: './publisher-change-dialog.component.scss'
})
export class PublisherChangeDialogComponent implements OnInit {
  formGroup!: FormGroup;

  get nameInput() { return this.formGroup.get('name')!; }

  constructor(
    @Inject(MAT_DIALOG_DATA) public publisher: Publisher,
    private dialogRef: MatDialogRef<PublisherChangeDialogComponent>,
    private readonly validateInput: ValidationMessage
  ) { }

  ngOnInit(): void {
    this.formGroup = new FormGroup(
      {
        name: new FormControl(this.publisher.name, [Validators.required, notEmptyString, noSpaces, Validators.maxLength(256)]),
      });
  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  validateInputField(input: AbstractControl<any, any>) {
    return this.validateInput.getValidationMessage(input);
  }
  sendDetails() {
    if (this.formGroup.valid) {
      const formValues = { ...this.formGroup.value };
      const publisher: Publisher = {
        id: this.publisher.id,
        name: formValues.name,
      };
      this.dialogRef.close(publisher);
    }
  }
}
