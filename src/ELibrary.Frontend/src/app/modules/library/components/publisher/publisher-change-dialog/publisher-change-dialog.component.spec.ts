import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PublisherChangeDialogComponent } from './publisher-change-dialog.component';

describe('PublisherChangeDialogComponent', () => {
  let component: PublisherChangeDialogComponent;
  let fixture: ComponentFixture<PublisherChangeDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PublisherChangeDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PublisherChangeDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
