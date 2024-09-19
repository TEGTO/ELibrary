import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientChangeDialogComponent } from './client-change-dialog.component';

describe('ClientChangeDialogComponent', () => {
  let component: ClientChangeDialogComponent;
  let fixture: ComponentFixture<ClientChangeDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ClientChangeDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ClientChangeDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
