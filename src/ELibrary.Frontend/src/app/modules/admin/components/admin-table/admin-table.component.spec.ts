import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NO_ERRORS_SCHEMA } from '@angular/core';
import { AdminTableComponent } from './admin-table.component';

describe('AdminTableComponent', () => {
  let component: AdminTableComponent;
  let fixture: ComponentFixture<AdminTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdminTableComponent],
      schemas: [NO_ERRORS_SCHEMA]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should contain a router-outlet', () => {
    const routerOutletElement = fixture.debugElement.nativeElement.querySelector('router-outlet');
    expect(routerOutletElement).toBeTruthy();
  });
});
