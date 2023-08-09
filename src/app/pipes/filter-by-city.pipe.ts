import { Pipe, PipeTransform } from '@angular/core';
import { IHotel } from '../models/Hotel';

@Pipe({
  name: 'filterByCity'
})
export class FilterByCityPipe implements PipeTransform {

  transform(hotels: IHotel[], selectedCity: string | null): IHotel[] {
    if (!selectedCity) {
      return hotels
    }

    return hotels.filter(h => h.city == selectedCity)
  }

}
