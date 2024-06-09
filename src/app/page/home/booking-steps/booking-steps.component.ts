import { DatePipe } from '@angular/common';
import { AfterViewInit, Component, OnInit } from '@angular/core';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { NotifierService } from 'angular-notifier';
import { NotifierNotification } from 'angular-notifier/lib/models/notifier-notification.model';
import { ToastrService } from 'ngx-toastr';
import { Subscription, pipe } from 'rxjs';
import { ITrip } from 'src/app/models/ITrip';
import { TripService } from 'src/app/services/trip.service';

@Component({
  selector: 'app-booking-steps',
  templateUrl: './booking-steps.component.html',
  styleUrls: ['./booking-steps.component.css'],
})
export class BookingStepsComponent implements AfterViewInit {

  constructor(
    public tripService: TripService,
    private notifier: ToastrService
  ) {}

  ngAfterViewInit(): void {
    this.getNextTrip();
    this.getOngoingTrip();
  }


  nextTrip: ITrip;
  ongoingTrip: ITrip;

  percentCompleted: number;

  private getOngoingTrip() {
    this.tripService.getOngoingTrip().subscribe({
      next: (ongoingTrip) => {
        
        if (ongoingTrip.id === 0) {
          return;
        }
        if (this.getTodayDate() > ongoingTrip.dateFrom) {
          return;
        }

        this.ongoingTrip = ongoingTrip;

        this.percentCompleted = this.getPercentCompleted(this.ongoingTrip);
      },
      error: (e) => {
        console.error(e);
        this.notifier.error(
          '',
          'The information about the Ongoing Trip has not been loaded.',
          {
            timeOut: 3000,
            positionClass: 'toast-bottom-right',
          }
        );
      },
    });
  }

  private getNextTrip() {
    this.tripService.getNextTrip().subscribe({
      next: (nextTrip) => {
        if (nextTrip.id == 0) {
          return;
        }
        this.nextTrip = nextTrip;
      },
      error: (e) => {
        console.error(e);
        this.notifier.error(
          '',
          'The information about the Next Trip has not been loaded.',
          {
            timeOut: 3000,
            positionClass: 'toast-bottom-right',
          }
        );
      },
    });
  }

  getPercentCompleted(ongoingTrip: ITrip) {

    const today = this.getTodayDate();
    
    const dateFrom = ongoingTrip.dateFrom;
    const dateTo = ongoingTrip.dateTo;

    const daysLeft =
      this.daysLeftCalculate(today, dateTo) / (1000 * 60 * 60 * 24);
    const tripLength =
      this.daysLeftCalculate(dateFrom, dateTo) / (1000 * 60 * 60 * 24);
    const invertedPercentage = Math.round(100 - (tripLength / daysLeft) * 100);

    return invertedPercentage;
  }

  getTodayDate():string {
    const today = new Date();
    const pipe = new DatePipe('en-US');
    return pipe.transform(today, 'MM/dd/YYYY') || "";
  }

  changeToDay(date: any) {
    const pipe = new DatePipe('en-US');
    return pipe.transform(date, 'dd');
  }

  daysLeftCalculate(date1: any, date2: any) {
    const day1 = Number(this.changeToDay(date1));
    const day2 = Number(this.changeToDay(date2));
    return day1 - day2;
  }

  // ngOnDestroy(): void {
  //   this.tripSubscription.unsubscribe();
  // }
}
