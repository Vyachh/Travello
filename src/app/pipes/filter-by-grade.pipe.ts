import { Pipe, PipeTransform } from '@angular/core';
import { IHotel } from '../models/Hotel';

@Pipe({
  name: 'filterByGrade'
})
export class FilterByGradePipe implements PipeTransform {

  transform(hotels: IHotel[], selectedGrade: number | null): IHotel[] {
    if (!selectedGrade) {
      return hotels
    }

    return hotels.filter(h => h.grade == selectedGrade)
  }

}
