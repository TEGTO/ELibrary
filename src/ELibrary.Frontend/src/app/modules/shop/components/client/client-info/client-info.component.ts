import { Component, Input, OnDestroy, OnInit, signal } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { debounceTime, Subject, takeUntil } from 'rxjs';
import { ShopCommand, ShopCommandObject, ShopCommandType } from '../../..';
import { Client, minDateValidator, noSpaces, notEmptyString, ValidationMessage } from '../../../../shared';

@Component({
  selector: 'app-client-info',
  templateUrl: './client-info.component.html',
  styleUrl: './client-info.component.scss'
})
export class ClientInfoComponent implements OnInit, OnDestroy {
  @Input({ required: true }) client!: Client;

  readonly panelOpenState = signal(false);
  formGroup!: FormGroup;

  private destroy$ = new Subject<void>();

  get nameInput() { return this.formGroup.get('name')!; }
  get middleNameInput() { return this.formGroup.get('middleName')!; }
  get lastNameInput() { return this.formGroup.get('lastName')!; }
  get dateOfBirthInput() { return this.formGroup.get('dateOfBirth')!; }
  get addressInput() { return this.formGroup.get('address')!; }
  get phoneInput() { return this.formGroup.get('phone')!; }
  get emailInput() { return this.formGroup.get('email')!; }

  constructor(
    private readonly validateInput: ValidationMessage,
    private readonly shopCommand: ShopCommand
  ) { }

  ngOnInit(): void {
    this.initializeForm();

    this.formGroup.valueChanges.pipe(
      takeUntil(this.destroy$),
      debounceTime(300)
    ).subscribe(() => {
      this.updateClient();
    });
  }
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  initializeForm(): void {
    this.formGroup = new FormGroup(
      {
        name: new FormControl(this.client.name, [Validators.required, notEmptyString, noSpaces, Validators.maxLength(256)]),
        middleName: new FormControl(this.client.middleName, [Validators.maxLength(256)]),
        lastName: new FormControl(this.client.lastName, [Validators.required, notEmptyString, noSpaces, Validators.maxLength(256)]),
        dateOfBirth: new FormControl(this.client.dateOfBirth, [Validators.required, minDateValidator()]),
        address: new FormControl(this.client.address, [Validators.maxLength(256)]),
        phone: new FormControl(this.client.phone, [Validators.maxLength(256)]),
        email: new FormControl(this.client.email, [Validators.required, notEmptyString, noSpaces, Validators.email, Validators.maxLength(256)]),
      });
  }
  updateClient() {
    if (this.formGroup.valid) {
      const formValues = { ...this.formGroup.value };
      const client: Client = {
        id: this.client.id,
        name: formValues.name,
        middleName: formValues.middleName,
        lastName: formValues.lastName,
        dateOfBirth: formValues.dateOfBirth,
        address: formValues.address,
        phone: formValues.phone,
        email: formValues.email,
      };
      this.shopCommand.dispatchCommand(ShopCommandObject.Client, ShopCommandType.Update, this, client);
    }
  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  validateInputField(input: AbstractControl<any, any>) {
    return this.validateInput.getValidationMessage(input);
  }
}