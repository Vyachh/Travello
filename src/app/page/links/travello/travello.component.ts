import { Component, OnInit, OnDestroy, AfterViewInit } from '@angular/core';
import { Route, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { ITrip } from 'src/app/models/ITrip';
import { TripService } from 'src/app/services/trip.service';

@Component({
  selector: 'app-travello',
  templateUrl: './travello.component.html',
  styleUrls: ['./travello.component.css'],
})
export class TravelloComponent implements AfterViewInit {
  private tripSubscription: Subscription;

  constructor(public tripService: TripService, private router: Router) {}

  ngAfterViewInit(): void {
    this.getNextTrip();
    this.getOngoingTrip();
    this.getApprovedTripList();
  }

  tripList: ITrip[];
  nextTrip: ITrip;
  ongoingTrip: ITrip;

  private getApprovedTripList() {
    this.tripService.getApprovedTripList().subscribe({
      next: (response) => {
        this.tripList = response;
      },
    });
  }

  private getOngoingTrip() {
    this.tripSubscription = this.tripService.getOngoingTrip().subscribe({
      next: (ongoingTrip) => {
        if (ongoingTrip.id == 0) {
          return;
        }
        this.ongoingTrip = ongoingTrip;
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  private getNextTrip() {
    this.tripSubscription = this.tripService.getNextTrip().subscribe({
      next: (nextTrip) => {
        if (nextTrip.id == 0) {
          return;
        }
        this.nextTrip = nextTrip;
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
