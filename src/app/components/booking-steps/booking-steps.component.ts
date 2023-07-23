import { Component, OnInit } from '@angular/core';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { Subscription } from 'rxjs';
import { ITrip } from 'src/app/models/trip';
import { TripService } from 'src/app/services/trip.service';

@Component({
  selector: 'app-booking-steps',
  templateUrl: './booking-steps.component.html',
  styleUrls: ['./booking-steps.component.css'],

})
export class BookingStepsComponent implements OnInit {
  private tripSubscription: Subscription;

  constructor(private tripService: TripService) {
  }
  trip: ITrip
  ngOnInit(): void {
    this.tripService.getNextTrip().subscribe((trip: ITrip) => {
      this.trip = trip;
    });
  }

  // ngOnDestroy(): void {
  //   this.tripSubscription.unsubscribe();
  // }
}
