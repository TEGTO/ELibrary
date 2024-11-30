/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import { environment } from '../../../../../environment/environment';
import { BookFallbackCoverPipe } from './book-fallback-cover.pipe';

describe('BookFallbackCoverPipe', () => {
  let pipe: BookFallbackCoverPipe;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [BookFallbackCoverPipe],
    });
    pipe = TestBed.inject(BookFallbackCoverPipe);
  });

  it('should create the pipe', () => {
    expect(pipe).toBeTruthy();
  });

  it('should return the original URL if the book ID is not failed', () => {
    const coverImgUrl = 'http://example.com/book-cover.jpg';
    const bookId = 1;

    const result = pipe.transform(coverImgUrl, bookId);
    expect(result).toBe(coverImgUrl);
  });

  it('should return placeholder URL if the book ID is failed', () => {
    const coverImgUrl = 'http://example.com/book-cover.jpg';
    const bookId = 1;

    pipe.markAsFailed(bookId);
    const result = pipe.transform(coverImgUrl, bookId);

    expect(result).toBe(environment.bookCoverPlaceholder);
  });

  it('should return placeholder URL if the book ID is missing', () => {
    const coverImgUrl = 'http://example.com/book-cover.jpg';
    const result = pipe.transform(coverImgUrl, null as any);

    expect(result).toBe(environment.bookCoverPlaceholder);
  });

  it('should add the book ID to failedImages when markAsFailed is called', () => {
    const bookId = 1;

    expect(pipe['failedImages'].has(bookId)).toBeFalse();

    pipe.markAsFailed(bookId);

    expect(pipe['failedImages'].has(bookId)).toBeTrue();
  });

  it('should not add duplicate book IDs to failedImages', () => {
    const bookId = 1;

    pipe.markAsFailed(bookId);
    pipe.markAsFailed(bookId);

    expect(pipe['failedImages'].size).toBe(1);
  });
});