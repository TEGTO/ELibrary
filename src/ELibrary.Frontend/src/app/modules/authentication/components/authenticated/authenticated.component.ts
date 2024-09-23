import { ChangeDetectionStrategy, Component, OnDestroy, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { Subject, takeUntil } from 'rxjs';
import { AuthenticationCommand, AuthenticationCommandType, AuthenticationService, passwordValidator } from '../..';
import { noSpaces, notEmptyString, UserUpdateRequest, ValidationMessage } from '../../../shared';

@Component({
  selector: 'app-authenticated',
  templateUrl: './authenticated.component.html',
  styleUrl: './authenticated.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AuthenticatedComponent implements OnInit, OnDestroy {
  hideNewPassword = true;
  formGroup: FormGroup = null!;
  private destroy$ = new Subject<void>();

  get emailInput() { return this.formGroup.get('email')!; }
  get oldPasswordInput() { return this.formGroup.get('oldPassword')!; }
  get passwordInput() { return this.formGroup.get('password')!; }

  constructor(
    private readonly authService: AuthenticationService,
    private readonly authCommand: AuthenticationCommand,
    private readonly dialogRef: MatDialogRef<AuthenticatedComponent>,
    private readonly validateInput: ValidationMessage
  ) { }

  ngOnInit(): void {
    this.authService.getUserData()
      .pipe(takeUntil(this.destroy$))
      .subscribe(data => {
        this.formGroup = new FormGroup(
          {
            email: new FormControl(data.email, [Validators.required, notEmptyString, noSpaces, Validators.email, Validators.maxLength(256)]),
            oldPassword: new FormControl('', [Validators.required, notEmptyString, noSpaces, Validators.maxLength(256)]),
            password: new FormControl('', [Validators.required, notEmptyString, noSpaces, passwordValidator, Validators.maxLength(256)])
          });
      })
  }
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  validateInputField(input: AbstractControl<any, any>) {
    return this.validateInput.getValidationMessage(input);
  }
  hidePasswordOnKeydown(event: KeyboardEvent): void {
    if (event.key === 'Enter' || event.key === ' ') {
      event.preventDefault();
      this.hideNewPassword = !this.hideNewPassword;
    }
  }
  updateUser() {
    if (this.formGroup.valid) {
      const req: UserUpdateRequest = {
        email: this.formGroup.value.email,
        oldPassword: this.formGroup.value.oldPassword,
        password: this.formGroup.value.password,
      };
      this.authCommand.dispatchCommand(AuthenticationCommandType.Update, this, req, this.dialogRef);
    }
  }
  logOutUser() {
    this.authCommand.dispatchCommand(AuthenticationCommandType.LogOut, this);
  }
}