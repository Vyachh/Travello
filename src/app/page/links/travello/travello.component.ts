import { Component, OnInit, OnDestroy } from '@angular/core';
import { Route, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { ITrip } from 'src/app/models/Trip';
import { TripService } from 'src/app/services/trip.service';

@Component({
  selector: 'app-travello',
  templateUrl: './travello.component.html',
  styleUrls: ['./travello.component.css'],
})
export class TravelloComponent implements OnInit {
  private tripSubscription: Subscription;

  constructor(public tripService: TripService, private router: Router) {}

  tripList: ITrip[];

  nextTrip: ITrip;
  ongoingTrip: ITrip;

  hasNextTripLoaded: boolean;
  hasOngoingTripLoaded: boolean;

  ngOnInit(): void {
    this.tripSubscription = this.tripService.getNextTrip().subscribe({
      next: (nextTrip) => {
        if (nextTrip.id == 0) {
          return;
        }
        this.nextTrip = nextTrip;
      },
    });

    this.tripSubscription = this.tripService
      .getOngoingTrip()
      .subscribe({
        next: (ongoingTrip) => {
          if (ongoingTrip.id == 0) {
            return;
          }
          this.ongoingTrip = ongoingTrip;
        },
        error: error =>{
          console.error(error)
        }
      });

    this.tripService.getTripList().subscribe({
      next: (response) => {
        this.tripList = response;
      },
    });
  }

  navigateToDetails(id: number) {
    this.router.navigate(['trip', id]);
  }

  ngOnDestroy(): void {
    this.tripSubscription.unsubscribe();
  }
}
