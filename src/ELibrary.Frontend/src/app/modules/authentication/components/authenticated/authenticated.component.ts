import { ChangeDetectionStrategy, Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { Subject, takeUntil } from 'rxjs';
import { AuthenticationService, changePasswordValidator, LOG_OUT_COMMAND_HANDLER, LogOutCommand, UPDATE_USER_COMMAND_HANDLER, UpdateUserCommand } from '../..';
import { CommandHandler, noSpaces, notEmptyString, ValidationMessage } from '../../../shared';

@Component({
  selector: 'app-authenticated',
  templateUrl: './authenticated.component.html',
  styleUrl: './authenticated.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AuthenticatedComponent implements OnInit, OnDestroy {
  hideNewPassword = true;
  formGroup: FormGroup = null!;
  private readonly destroy$ = new Subject<void>();

  get emailInput() { return this.formGroup.get('email')!; }
  get oldPasswordInput() { return this.formGroup.get('oldPassword')!; }
  get passwordInput() { return this.formGroup.get('password')!; }

  constructor(
    private readonly authService: AuthenticationService,
    @Inject(UPDATE_USER_COMMAND_HANDLER) private readonly updateHandler: CommandHandler<UpdateUserCommand>,
    @Inject(LOG_OUT_COMMAND_HANDLER) private readonly logOutHandler: CommandHandler<LogOutCommand>,
    private readonly dialogRef: MatDialogRef<AuthenticatedComponent>,
    private readonly validateInput: ValidationMessage
  ) { }

  ngOnInit(): void {
    this.authService.getUserAuth()
      .pipe(takeUntil(this.destroy$))
      .subscribe(data => {
        this.formGroup = new FormGroup(
          {
            email: new FormControl(data.email, [Validators.required, notEmptyString, noSpaces, Validators.email, Validators.maxLength(256)]),
            oldPassword: new FormControl('', [Validators.required, notEmptyString, noSpaces, Validators.maxLength(256)]),
            password: new FormControl('', [noSpaces, changePasswordValidator, Validators.maxLength(256)])
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
      const formValues = { ...this.formGroup.value };
      const command: UpdateUserCommand = {
        email: formValues.email,
        oldPassword: formValues.oldPassword,
        password: formValues.password,
        matDialogRef: this.dialogRef
      };
      this.updateHandler.dispatch(command);
    }
  }
  logOutUser() {
    const command: LogOutCommand = {};
    this.logOutHandler.dispatch(command);
  }
}