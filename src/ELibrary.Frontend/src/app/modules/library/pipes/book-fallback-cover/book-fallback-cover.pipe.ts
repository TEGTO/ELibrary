import { Injectable, Pipe, PipeTransform } from '@angular/core';
import { environment } from '../../../../../environment/environment';

@Pipe({
  name: 'bookFallbackCover'
})
@Injectable({
  providedIn: 'root',
})
export class BookFallbackCoverPipe implements PipeTransform {
  private readonly failedImages = new Set<number>();

  transform(coverImgUrl: string, bookId: number): string {
    if (this.failedImages.has(bookId) || !bookId) {
      return environment.bookCoverPlaceholder;
    }
    return coverImgUrl;
  }

  markAsFailed(bookId: number): void {
    this.failedImages.add(bookId);
  }
}
