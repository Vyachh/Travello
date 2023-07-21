import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { ITrip } from 'src/app/models/trip';
import { TripService } from 'src/app/services/trip.service';

@Component({
  selector: 'app-travello',
  templateUrl: './travello.component.html',
  styleUrls: ['./travello.component.css']
})
export class TravelloComponent implements OnInit {
  private tripSubscription: Subscription;

  constructor(private tripService: TripService) {

  }
  trip: ITrip

  ngOnInit(): void {
    this.tripSubscription = this.tripService.getNextTrip().subscribe((trip: ITrip) => {
      this.trip = trip;
      console.log(this.trip);
    });
  }

  ngOnDestroy(): void {
    this.tripSubscription.unsubscribe();
  }
}

