import { Component, OnInit, OnDestroy } from '@angular/core';
import { Route, Router } from '@angular/router';
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

  constructor(public tripService: TripService, private router: Router) {

  }

  tripList: ITrip[]

  trip: ITrip
  ongoingTrip: ITrip

  ngOnInit(): void {
    this.tripSubscription = this.tripService.getNextTrip().subscribe((trip: ITrip) => {
      this.trip = trip;
    });
    this.tripSubscription = this.tripService.getOngoingTrip().subscribe((trip: ITrip) => {
      this.ongoingTrip = trip;
    });

    this.tripService.getTripList().subscribe({
      next: response => {
        this.tripList = response
      }
    })
  }

  navigateToDetails(id: number) {
    this.router.navigate(['trip', id])
  }

  ngOnDestroy(): void {
    this.tripSubscription.unsubscribe();
  }
}

