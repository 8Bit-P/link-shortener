import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'shortenNumber'
})
export class ShortenNumberPipe implements PipeTransform {

  transform(value: number, ...args: unknown[]): string {
    if (value >= 10000) {
      return (value / 1000).toFixed(1) + 'k'; 
    }
    return value.toString();
  }

}
