import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { ITrip } from 'src/app/models/Trip';
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
  ongoingTrip:ITrip

  ngOnInit(): void {
    this.tripSubscription = this.tripService.getNextTrip().subscribe((trip: ITrip) => {
      this.trip = trip;
    });
    this.tripSubscription = this.tripService.getOngoingTrip().subscribe((trip: ITrip) => {
      this.ongoingTrip = trip;
    });
  }

  ngOnDestroy(): void {
    this.tripSubscription.unsubscribe();
  }
}

