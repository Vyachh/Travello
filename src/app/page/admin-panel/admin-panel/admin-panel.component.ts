import { Component, OnInit } from '@angular/core';
import { ITrip } from 'src/app/models/trip';
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
        this.tripList = response,
        console.log(response);
        
      },
      error: error => {
        console.error(error);

      }
    })
  }
}
