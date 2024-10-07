import { Component, Inject, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AdminRegisterUserCommand } from '../..';
import { confirmPasswordValidator, passwordValidator } from '../../../authentication';
import { CommandHandler, getRoleArray, noSpaces, notEmptyString, Roles, ValidationMessage } from '../../../shared';
import { ADMIN_REGISTER_USER_COMMAND_HANDLER } from '../../command/tokens';

@Component({
  selector: 'app-admin-register-user-dialog',
  templateUrl: './admin-register-user-dialog.component.html',
  styleUrl: './admin-register-user-dialog.component.scss'
})
export class AdminRegisterUserDialogComponent implements OnInit {
  formGroup!: FormGroup;
  hidePassword = true;

  get emailInput() { return this.formGroup.get('email')! as FormControl; }
  get passwordInput() { return this.formGroup.get('password')! as FormControl; }
  get passwordConfirmInput() { return this.formGroup.get('passwordConfirm')! as FormControl; }
  get rolesInput() { return this.formGroup.get('roles')! as FormControl; }
  get roles() { return getRoleArray(); }

  constructor(
    private readonly validateInput: ValidationMessage,
    @Inject(ADMIN_REGISTER_USER_COMMAND_HANDLER) private readonly registerHanalder: CommandHandler<AdminRegisterUserCommand>,
    private readonly dialogRef: MatDialogRef<AdminRegisterUserDialogComponent>,
  ) { }

  ngOnInit(): void {
    this.formGroup = new FormGroup(
      {
        email: new FormControl('', [Validators.required, notEmptyString, noSpaces, Validators.email, Validators.maxLength(256)]),
        password: new FormControl('', [Validators.required, notEmptyString, noSpaces, passwordValidator, Validators.maxLength(256)]),
        passwordConfirm: new FormControl('', [Validators.required, notEmptyString, noSpaces, confirmPasswordValidator, Validators.maxLength(256)]),
        roles: new FormControl([Roles.CLIENT], [Validators.required])
      });
  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  validateInputField(input: AbstractControl<any, any>) {
    return this.validateInput.getValidationMessage(input);
  }
  hidePasswordOnKeydown(event: KeyboardEvent): void {
    if (event.key === 'Enter' || event.key === ' ') {
      event.preventDefault();
      this.hidePassword = !this.hidePassword;
    }
  }
  registerUser() {
    if (this.formGroup.valid) {
      const formValues = { ...this.formGroup.value };
      const command: AdminRegisterUserCommand =
      {
        email: formValues.email,
        password: formValues.password,
        confirmPassword: formValues.passwordConfirm,
        roles: formValues.roles,
        matDialogRef: this.dialogRef
      }
      this.registerHanalder.dispatch(command);
    }
  }
}
