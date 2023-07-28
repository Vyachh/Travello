import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { Subscription, pipe } from 'rxjs';
import { ITrip } from 'src/app/models/Trip';
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
  nextTrip: ITrip
  ongoingTrip: ITrip

  percentCompleted: number

  ngOnInit(): void {
    this.tripService.getNextTrip().subscribe((trip: ITrip) => {
      this.nextTrip = trip;
    });
    this.tripService.getOngoingTrip().subscribe((trip: ITrip) => {
      this.ongoingTrip = trip;

      this.percentCompleted = this.getPercentCompleted(this.ongoingTrip);
    });

  }

  getPercentCompleted(ongoingTrip: ITrip) {
    const today = this.getTodayDate();
    const dateFrom = ongoingTrip.dateFrom
    const dateTo = ongoingTrip.dateTo

    const daysLeft = this.daysLeftCalculate(today, dateTo) / (1000 * 60 * 60 * 24);
    const tripLength = this.daysLeftCalculate(dateFrom, dateTo) / (1000 * 60 * 60 * 24)
    const invertedPercentage = Math.round(100 - (daysLeft / tripLength * 100))

    return invertedPercentage
  }

  getTodayDate() {
    const today = new Date();
    const pipe = new DatePipe('en-US')
    return pipe.transform(today, 'MM/dd/YYYY')
  }

  changeToDay(date: any) {
    const pipe = new DatePipe('en-US')
    return pipe.transform(date, 'dd')
  }

  daysLeftCalculate(date1: any, date2: any) {
    const day1 = Number(this.changeToDay(date1))
    const day2 = Number(this.changeToDay(date2))
    return day1 - day2
  }


  // ngOnDestroy(): void {
  //   this.tripSubscription.unsubscribe();
  // }
}
