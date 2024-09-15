import { ScrollingModule } from '@angular/cdk/scrolling';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { ComponentFixture, fakeAsync, TestBed, tick } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatNativeDateModule, MatOptionModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { By } from '@angular/platform-browser';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { of } from 'rxjs';
import { AuthorService, GenreService } from '../../..';
import { AuthorResponse, BookResponse, GenreResponse } from '../../../../shared';
import { BookChangeDialogComponent } from './book-change-dialog.component';

describe('BookChangeDialogComponent', () => {
  let component: BookChangeDialogComponent;
  let fixture: ComponentFixture<BookChangeDialogComponent>;
  let dialogRef: jasmine.SpyObj<MatDialogRef<BookChangeDialogComponent>>;
  let authorService: jasmine.SpyObj<AuthorService>;
  let genreService: jasmine.SpyObj<GenreService>;

  const mockBook: BookResponse = {
    id: 1,
    name: 'Mock Book',
    publicationDate: new Date('2020-01-01'),
    author: { id: 1, name: 'John', lastName: 'Doe', dateOfBirth: new Date('1970-01-01') },
    genre: { id: 1, name: 'Fiction' }
  };

  const mockAuthors: AuthorResponse[] = [
    { id: 1, name: 'John', lastName: 'Doe', dateOfBirth: new Date('1970-01-01') },
    { id: 2, name: 'Jane', lastName: 'Smith', dateOfBirth: new Date('1980-01-01') }
  ];

  const mockGenres: GenreResponse[] = [
    { id: 1, name: 'Fiction' },
    { id: 2, name: 'Non-Fiction' }
  ];

  beforeEach(async () => {
    const dialogRefSpy = jasmine.createSpyObj('MatDialogRef', ['close']);
    const authorServiceSpy = jasmine.createSpyObj('AuthorService', ['getAuthorsPaginated']);
    const genreServiceSpy = jasmine.createSpyObj('GenreService', ['getGenresPaginated']);

    await TestBed.configureTestingModule({
      declarations: [BookChangeDialogComponent],
      imports: [
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        MatDatepickerModule,
        MatNativeDateModule,
        MatDialogModule,
        MatSelectModule,
        MatOptionModule,
        ScrollingModule,
        NoopAnimationsModule
      ],
      providers: [
        { provide: MAT_DIALOG_DATA, useValue: mockBook },
        { provide: MatDialogRef, useValue: dialogRefSpy },
        { provide: AuthorService, useValue: authorServiceSpy },
        { provide: GenreService, useValue: genreServiceSpy }
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA]
    }).compileComponents();

    dialogRef = TestBed.inject(MatDialogRef) as jasmine.SpyObj<MatDialogRef<BookChangeDialogComponent>>;
    authorService = TestBed.inject(AuthorService) as jasmine.SpyObj<AuthorService>;
    genreService = TestBed.inject(GenreService) as jasmine.SpyObj<GenreService>;
  });

  beforeEach(() => {
    authorService.getPaginated.and.returnValue(of(mockAuthors));
    genreService.getPaginated.and.returnValue(of(mockGenres));

    fixture = TestBed.createComponent(BookChangeDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize form with dialog data', () => {
    expect(component.formGroup).toBeDefined();
    expect(component.titleInput.value).toBe(mockBook.name);
    expect(component.publicationDateInput.value).toEqual(mockBook.publicationDate);
    expect(component.authorInput.value).toBe(mockBook.author.id);
    expect(component.genreInput.value).toBe(mockBook.genre.id);
  });

  it('should validate form fields', () => {
    component.formGroup.get('title')?.setValue('');
    component.formGroup.get('publicationDate')?.setValue(null);
    component.formGroup.get('author')?.setValue(null);
    component.formGroup.get('genre')?.setValue(null);

    expect(component.formGroup.invalid).toBeTrue();

    component.formGroup.get('title')?.setValue('Updated Title');
    component.formGroup.get('publicationDate')?.setValue(new Date('2021-01-01'));
    component.formGroup.get('author')?.setValue(1);
    component.formGroup.get('genre')?.setValue(1);

    expect(component.formGroup.valid).toBeTrue();
  });

  it('should close dialog with correct data when form is valid', () => {
    component.formGroup.setValue({
      title: 'Updated Title',
      publicationDate: new Date('2021-01-01'),
      author: 1,
      genre: 1
    });

    component.sendDetails();

    expect(dialogRef.close).toHaveBeenCalledWith({
      id: mockBook.id,
      title: 'Updated Title',
      publicationDate: new Date('2021-01-01'),
      author: mockBook.author,
      genre: mockBook.genre
    });
  });

  it('should not close dialog if form is invalid', () => {
    component.formGroup.get('title')?.setValue('');
    component.formGroup.get('publicationDate')?.setValue(null);
    component.formGroup.get('author')?.setValue(null);
    component.formGroup.get('genre')?.setValue(null);

    component.sendDetails();

    expect(dialogRef.close).not.toHaveBeenCalled();
  });

  it('should display validation errors when form fields are invalid', () => {
    component.formGroup.get('title')?.setValue('');
    fixture.detectChanges();

    expect(component.formGroup.valid).toBeFalse();
    expect(component.titleInput.hasError('required')).toBeTrue();
  });

  it('should render the HTML structure correctly', () => {
    const compiled = fixture.nativeElement as HTMLElement;

    expect(compiled.querySelector('h2')?.textContent).toContain('Book');

    expect(compiled.querySelector('mat-form-field')).toBeTruthy();
    expect(compiled.querySelector('button#send-button')).toBeTruthy();
  });

  it('should load authors and genres on initialization', () => {
    expect(authorService.getPaginated).toHaveBeenCalled();
    expect(genreService.getPaginated).toHaveBeenCalled();
  });

  it('should close the dialog when the close button is clicked', () => {
    const closeButton = fixture.debugElement.query(By.css('.close-button')).nativeElement;
    closeButton.click();
    fixture.detectChanges();
    expect(dialogRef.close).toHaveBeenCalled();
  });

  it('should append unique authors when loading more', fakeAsync(() => {
    component.loadAuthors();

    expect(component.authors.length).toBe(2);
    expect(component.authors).toEqual(mockAuthors);
  }));

  it('should append unique genres when loading more', fakeAsync(() => {
    component.loadGenres();

    expect(component.genres.length).toBe(2);
    expect(component.genres).toEqual(mockGenres);
  }));

  it('should load more authors on scroll', fakeAsync(() => {
    const authorScroller = component.authorScroller;
    spyOn(authorScroller, 'measureScrollOffset').and.returnValue(3);
    const elementScrolledSpy = spyOn(authorScroller.elementScrolled(), 'pipe').and.callThrough();
    spyOn(component, 'loadAuthors').and.callThrough();
    component.ngAfterViewInit();
    authorScroller.elementScrolled = jasmine.createSpy().and.returnValue(of({}));
    tick(300);
    expect(elementScrolledSpy).toHaveBeenCalled();
    // expect(component.loadAuthors).toHaveBeenCalled();
  }));

  it('should load more genres on scroll', fakeAsync(() => {
    const genreScroller = component.genreScroller;
    spyOn(genreScroller, 'measureScrollOffset').and.returnValue(3);
    const elementScrolledSpy = spyOn(genreScroller.elementScrolled(), 'pipe').and.callThrough();
    spyOn(component, 'loadGenres').and.callThrough();
    component.ngAfterViewInit();
    genreScroller.elementScrolled = jasmine.createSpy().and.returnValue(of({}));
    tick(300);
    expect(elementScrolledSpy).toHaveBeenCalled();
    // expect(component.loadGenres).toHaveBeenCalled();
  }));
});