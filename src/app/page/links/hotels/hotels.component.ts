import { Component, OnInit } from '@angular/core';
import { IHotel } from 'src/app/models/Hotel';
import { HotelService } from 'src/app/services/hotel.service';

@Component({
  selector: 'app-hotels',
  templateUrl: './hotels.component.html',
  styleUrls: ['./hotels.component.css']
})
export class HotelsComponent implements OnInit {

  hotels: IHotel[]
  selectedGrade: number | null = null
  selectedCity: string | null = null

  constructor(private hotelService: HotelService) {

  }
  ngOnInit(): void {
    this.hotelService.getInfo().subscribe({
      next: response => {
        this.hotels = response
      }
    })
  }

  filterByGrade(grade: number | null): void {
    this.selectedGrade = grade
  }

  filterByCity(city: string | null) {
    this.selectedCity = city
  }


}
