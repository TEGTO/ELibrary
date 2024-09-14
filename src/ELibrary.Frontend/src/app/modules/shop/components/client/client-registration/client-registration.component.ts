import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ShopCommand, ShopCommandObject, ShopCommandType } from '../../..';
import { CreateClientRequest, minDateValidator } from '../../../../shared';

@Component({
  selector: 'client-registration',
  templateUrl: './client-registration.component.html',
  styleUrl: './client-registration.component.scss'
})
export class ClientRegistrationComponent implements OnInit {
  formGroup!: FormGroup;

  get nameInput() { return this.formGroup.get('name')!; }
  get middleNameInput() { return this.formGroup.get('middleName')!; }
  get lastNameInput() { return this.formGroup.get('lastName')!; }
  get dateOfBirthInput() { return this.formGroup.get('dateOfBirth')!; }
  get addressInput() { return this.formGroup.get('address')!; }
  get phoneInput() { return this.formGroup.get('phone')!; }
  get emailInput() { return this.formGroup.get('email')!; }

  constructor(private readonly shopCommand: ShopCommand) { }

  ngOnInit(): void {
    this.formGroup = new FormGroup(
      {
        name: new FormControl('', [Validators.required, Validators.maxLength(256)]),
        middleName: new FormControl('', [Validators.maxLength(256)]),
        lastName: new FormControl('', [Validators.required, Validators.maxLength(256)]),
        dateOfBirth: new FormControl(null, [Validators.required, minDateValidator()]),
        address: new FormControl('', [Validators.maxLength(256)]),
        phone: new FormControl('', [Validators.maxLength(256)]),
        email: new FormControl('', [Validators.maxLength(256)]),
      });
  }

  createClient() {
    if (this.formGroup.valid) {
      const formValues = { ...this.formGroup.value };
      const req: CreateClientRequest = {
        name: formValues.name,
        middleName: formValues.middleName,
        lastName: formValues.lastName,
        dateOfBirth: formValues.dateOfBirth,
        address: formValues.address,
        phone: formValues.phone,
        email: formValues.email,
      };
      this.shopCommand.dispatchCommand(ShopCommandObject.Client, ShopCommandType.Create, this, req);
    }
  }
}
