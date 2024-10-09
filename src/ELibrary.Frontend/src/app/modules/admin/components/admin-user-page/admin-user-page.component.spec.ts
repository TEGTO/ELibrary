import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NO_ERRORS_SCHEMA } from '@angular/core';
import { AdminUserPageComponent } from './admin-user-page.component';

describe('AdminUserPageComponent', () => {
  let component: AdminUserPageComponent;
  let fixture: ComponentFixture<AdminUserPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdminUserPageComponent],
      schemas: [NO_ERRORS_SCHEMA]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminUserPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should render app-admin-user-details component', () => {
    const userDetailsElement = fixture.debugElement.nativeElement.querySelector('app-admin-user-details');
    expect(userDetailsElement).toBeTruthy();
  });

  it('should render app-admin-client-details component', () => {
    const clientDetailsElement = fixture.debugElement.nativeElement.querySelector('app-admin-client-details');
    expect(clientDetailsElement).toBeTruthy();
  });
});