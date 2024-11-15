import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'placeholder',
  standalone: true
})
export class PlaceholderPipe implements PipeTransform {

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  transform(value: any, placeholder = '—'): string {
    return value === null || value === undefined || value === '' ? placeholder : value.toString();
  }
}
