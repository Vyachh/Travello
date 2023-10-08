import { Component, OnInit } from '@angular/core';
import { ITrip } from 'src/app/models/ITrip';
import { TripService } from 'src/app/services/trip.service';
import { faCheck } from '@fortawesome/free-solid-svg-icons';
import { faX } from '@fortawesome/free-solid-svg-icons';
import { AccountService } from 'src/app/services/account.service';
import { IUserInfo } from 'src/app/models/UserInfo';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.css'],
})
export class AdminPanelComponent implements OnInit {
  faCheck = faCheck;
  faX = faX;

  searchTrip: string;

  tripList: ITrip[];

  userInfo: IUserInfo;

  constructor(
    private tripService: TripService,
    public accountService: AccountService,
    private notifier: ToastrService
  ) {}

  ngOnInit(): void {
    this.getUserInfo();
    this.getTripList();
  }

  onSetNextTrip(trip: ITrip) {
    this.tripService.setNextTrip(trip.id).subscribe();
  }

  onSetOngoingTrip(trip: ITrip) {
    this.tripService.setOngoingTrip(trip.id).subscribe();
  }

  onSearchTripChange(request: string) {
    this.filterTripList(request);
  }

  onDeleteTrip(id: number) {
    this.tripService.deleteTrip(id).subscribe({
      next: () => {
        this.getTripList();
      },
      error: (error) => {
        console.log(error);
        this.notifier.error("Trip didn't deleted");
      },
    });
  }

  onApprove(id: number) {
    this.tripService.approve(id).subscribe({
      next: () => {
        this.getTripList();
      },
      error: (error) => {
        console.log(error);
        this.notifier.error("Trip didn't approved");
      },
    });
  }

  private filterTripList(request: string) {
    if (this.searchTrip !== '') {
      this.tripService.getTripList().subscribe({
        next: (response: ITrip[]) => {
          this.tripList = response.filter((t) => t.title.includes(request));
        },
        error: (error) => {
          console.log(error);
          this.notifier.error("Trips didn't filtered");
        },
      });
    } else {
      this.getTripList();
    }
  }

  private getTripList() {
    this.tripService.getTripList().subscribe({
      next: (response) => {
        this.tripList = response.filter((t) => !t.isApproved);
      },
      error: (error) => {
        console.error(error);
        this.notifier.error("Trip list didn't get");
      },
    });
  }

  private getUserInfo() {
    this.accountService.getInfo().subscribe({
      next: (response) => {
        this.userInfo = response;
      },
      error: (error) => {
        console.error(error);
        this.notifier.error("User info list didn't get");
      },
    });
  }
}
