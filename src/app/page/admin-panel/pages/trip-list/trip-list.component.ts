import { Component } from '@angular/core';
import { ITrip } from 'src/app/models/Trip';
import { TripService } from 'src/app/services/trip.service';

@Component({
  selector: 'app-trip-list',
  templateUrl: './trip-list.component.html',
  styleUrls: ['./trip-list.component.css']
})
export class TripListComponent {
  tripList: ITrip[]

  constructor(private tripService: TripService) {

  }

  ngOnInit(): void {
    this.tripService.getTripList().subscribe({
      next: response => {
        this.tripList = response
      },
      error: error => {
        console.error(error);
      }
    })
  }

  onSetNextTrip(trip: any) {
    this.tripService.setNextTrip(trip.id).subscribe()
  }

  onSetOngoingTrip(trip: any) {
    this.tripService.setOngoingTrip(trip.id).subscribe()
  }

  onDeleteTrip(trip: any) {
    this.tripService.deleteTrip(trip.id).subscribe()
    location.reload();
  }
}
