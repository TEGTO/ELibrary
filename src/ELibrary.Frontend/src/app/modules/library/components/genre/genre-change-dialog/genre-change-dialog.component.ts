import { Component, Inject, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { GenreResponse, ValidationMessage } from '../../../../shared';

@Component({
  selector: 'app-genre-change-dialog',
  templateUrl: './genre-change-dialog.component.html',
  styleUrl: './genre-change-dialog.component.scss'
})
export class GenreChangeDialogComponent implements OnInit {
  formGroup!: FormGroup;

  get nameInput() { return this.formGroup.get('name')!; }

  constructor(
    @Inject(MAT_DIALOG_DATA) public genre: GenreResponse,
    private dialogRef: MatDialogRef<GenreChangeDialogComponent>,
    private readonly validateInput: ValidationMessage
  ) { }

  ngOnInit(): void {
    this.formGroup = new FormGroup(
      {
        name: new FormControl(this.genre.name, [Validators.required, Validators.maxLength(256)]),
      });
  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  validateInputField(input: AbstractControl<any, any>) {
    return this.validateInput.getValidationMessage(input);
  }
  sendDetails() {
    if (this.formGroup.valid) {
      const formValues = { ...this.formGroup.value };
      const genre: GenreResponse = {
        id: this.genre.id,
        name: formValues.name,
      };
      this.dialogRef.close(genre);
    }
  }
}
