import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { UserInfoController, UserInfoRegistrationComponent, UserInfoService, UserInfoShowcaserComponent } from '.';

@NgModule({
  declarations: [UserInfoShowcaserComponent, UserInfoRegistrationComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatFormFieldModule,
    MatDatepickerModule,
    MatNativeDateModule,
  ],
  exports: [UserInfoShowcaserComponent, UserInfoRegistrationComponent],
  providers: [
    { provide: UserInfoService, useClass: UserInfoController },
  ]
})
export class UserInfoModule { }
