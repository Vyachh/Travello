import { DatePipe } from '@angular/common';
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'month'
})
export class MonthPipe implements PipeTransform {

  transform(value: any, ...args: any[]): any {
    const datePipe = new DatePipe('en-US');
    return datePipe.transform(value, 'MMMM');
  }

}
