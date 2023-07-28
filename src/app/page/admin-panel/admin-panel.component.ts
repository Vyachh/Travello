import { Component, OnInit } from '@angular/core';
import { ITrip } from 'src/app/models/Trip';
import { TripService } from 'src/app/services/trip.service';

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.css']
})
export class AdminPanelComponent implements OnInit {

  tripList: ITrip[]

  constructor(private tripService: TripService) {

  }

  ngOnInit(): void {
    this.tripService.getTripList().subscribe({
      next: response => {
        this.tripList = response
        console.log(response);
        
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
