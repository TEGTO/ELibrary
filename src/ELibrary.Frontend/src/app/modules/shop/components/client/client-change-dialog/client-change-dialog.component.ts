import { ChangeDetectionStrategy, Component, Inject, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Client, dateInPastValidator, noSpaces, notEmptyString, ValidationMessage } from '../../../../shared';

@Component({
  selector: 'app-client-change-dialog',
  templateUrl: './client-change-dialog.component.html',
  styleUrl: './client-change-dialog.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ClientChangeDialogComponent implements OnInit {
  formGroup!: FormGroup;

  get nameInput() { return this.formGroup.get('name')!; }
  get middleNameInput() { return this.formGroup.get('middleName')!; }
  get lastNameInput() { return this.formGroup.get('lastName')!; }
  get dateOfBirthInput() { return this.formGroup.get('dateOfBirth')!; }
  get addressInput() { return this.formGroup.get('address')!; }
  get phoneInput() { return this.formGroup.get('phone')!; }
  get emailInput() { return this.formGroup.get('email')!; }

  constructor(
    @Inject(MAT_DIALOG_DATA) public client: Client,
    private dialogRef: MatDialogRef<ClientChangeDialogComponent>,
    private readonly validateInput: ValidationMessage
  ) { }

  ngOnInit(): void {
    this.formGroup = new FormGroup(
      {
        name: new FormControl(this.client.name, [Validators.required, notEmptyString, noSpaces, Validators.maxLength(256)]),
        middleName: new FormControl(this.client.middleName, [Validators.maxLength(256)]),
        lastName: new FormControl(this.client.lastName, [Validators.required, notEmptyString, noSpaces, Validators.maxLength(256)]),
        dateOfBirth: new FormControl(this.client.dateOfBirth, [Validators.required, dateInPastValidator()]),
        address: new FormControl(this.client.address, [Validators.maxLength(512)]),
        phone: new FormControl(this.client.phone, [Validators.maxLength(256)]),
        email: new FormControl(this.client.email, [Validators.required, notEmptyString, noSpaces, Validators.email, Validators.maxLength(256)]),
      });
  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  validateInputField(input: AbstractControl<any, any>) {
    return this.validateInput.getValidationMessage(input);
  }
  onSubmit() {
    if (this.formGroup.valid) {
      const formValues = { ...this.formGroup.value };
      const client: Client = {
        ...this.client,
        name: formValues.name,
        middleName: formValues.middleName,
        lastName: formValues.lastName,
        dateOfBirth: formValues.dateOfBirth,
        address: formValues.address,
        phone: formValues.phone,
        email: formValues.email,
      };
      this.dialogRef.close(client);
    }
  }
}
