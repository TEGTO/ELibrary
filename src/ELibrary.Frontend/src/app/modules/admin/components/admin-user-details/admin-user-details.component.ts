import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { AdminService } from '../..';
import { AdminUser, RouteReader, ValidationMessage } from '../../../shared';

@Component({
  selector: 'app-admin-user-details',
  templateUrl: './admin-user-details.component.html',
  styleUrl: './admin-user-details.component.scss'
})
export class AdminUserDetailsComponent implements OnInit {
  private formGroup!: FormGroup;

  user$!: Observable<AdminUser>;

  constructor(
    private readonly validateInput: ValidationMessage,
    private readonly adminService: AdminService,
    private readonly routeReader: RouteReader,
    private readonly route: ActivatedRoute,
  ) { }

  ngOnInit(): void {
    this.user$ = this.routeReader.readId(
      this.route,
      (id: string) => this.adminService.getUserById(id),
    );
  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  validateInputField(input: AbstractControl<any, any>) {
    return this.validateInput.getValidationMessage(input);
  }
}
