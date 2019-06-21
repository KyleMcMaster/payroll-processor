import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'noValue' })
export class NoValuePipe implements PipeTransform {
  transform(value: string): string {
    return value === null ||
      value === undefined ||
      value === '' ||
      value === '00000000-0000-0000-0000-000000000000'
      ? 'N/A'
      : value;
  }
}
