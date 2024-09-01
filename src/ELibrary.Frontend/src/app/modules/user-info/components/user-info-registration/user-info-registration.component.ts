import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { minDateValidator } from '../../../shared';

@Component({
  selector: 'user-info-registration',
  templateUrl: './user-info-registration.component.html',
  styleUrls: ['./user-info-registration.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserInfoRegistrationComponent implements OnInit {
  @Input({ required: true }) parentFormGroup!: FormGroup;

  get firstNameInput() { return this.parentFormGroup.get('firstName')!; }
  get lastNameInput() { return this.parentFormGroup.get('lastName')!; }
  get dateOfBirthInput() { return this.parentFormGroup.get('dateOfBirth')!; }
  get addressInput() { return this.parentFormGroup.get('address')!; }

  ngOnInit(): void {
    this.addUserInfoControls();
  }

  private addUserInfoControls(): void {
    this.parentFormGroup.addControl('firstName', new FormControl('', [Validators.required, Validators.maxLength(256)]));
    this.parentFormGroup.addControl('lastName', new FormControl('', [Validators.maxLength(256)]));
    this.parentFormGroup.addControl('dateOfBirth', new FormControl(null, [Validators.required, minDateValidator()]));
    this.parentFormGroup.addControl('address', new FormControl('', [Validators.maxLength(256)]));
  }
}
