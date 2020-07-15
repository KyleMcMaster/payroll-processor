import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'unslugify' })
export class UnslugifyPipe implements PipeTransform {
  transform(value?: string): string {
    return value?.replace('_', ' ') ?? '';
  }
}
