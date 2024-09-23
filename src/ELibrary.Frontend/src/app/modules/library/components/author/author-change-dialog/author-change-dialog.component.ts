import { Component, Inject, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Author, minDateValidator, noSpaces, notEmptyString, ValidationMessage } from '../../../../shared';

@Component({
  selector: 'app-author-change-dialog',
  templateUrl: './author-change-dialog.component.html',
  styleUrl: './author-change-dialog.component.scss'
})
export class AuthorChangeDialogComponent implements OnInit {
  formGroup!: FormGroup;

  get nameInput() { return this.formGroup.get('name')!; }
  get lastNameInput() { return this.formGroup.get('lastName')!; }
  get dateOfBirthInput() { return this.formGroup.get('dateOfBirth')!; }

  constructor(
    @Inject(MAT_DIALOG_DATA) public author: Author,
    private dialogRef: MatDialogRef<AuthorChangeDialogComponent>,
    private readonly validateInput: ValidationMessage
  ) { }

  ngOnInit(): void {
    this.formGroup = new FormGroup(
      {
        name: new FormControl(this.author.name, [Validators.required, notEmptyString, noSpaces, Validators.maxLength(256)]),
        lastName: new FormControl(this.author.lastName, [Validators.required, notEmptyString, noSpaces, Validators.maxLength(256)]),
        dateOfBirth: new FormControl(this.author.dateOfBirth, [Validators.required, minDateValidator()]),
      });
  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  validateInputField(input: AbstractControl<any, any>) {
    return this.validateInput.getValidationMessage(input);
  }
  sendDetails() {
    if (this.formGroup.valid) {
      const formValues = { ...this.formGroup.value };
      const author: Author = {
        id: this.author.id,
        name: formValues.name,
        lastName: formValues.lastName,
        dateOfBirth: formValues.dateOfBirth,
      };
      this.dialogRef.close(author);
    }
  }
}
