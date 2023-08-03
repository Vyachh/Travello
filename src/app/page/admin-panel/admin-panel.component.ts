import { Component, OnInit } from '@angular/core';
import { ITrip } from 'src/app/models/Trip';
import { TripService } from 'src/app/services/trip.service';
import { faCheck } from '@fortawesome/free-solid-svg-icons';
import { faX } from '@fortawesome/free-solid-svg-icons';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.css']
})
export class AdminPanelComponent implements OnInit {

  faCheck = faCheck
  faX = faX

  searchTrip: string

  tripList: ITrip[]

  constructor(private tripService: TripService, public accountService: AccountService) {

  }

  ngOnInit(): void {
    this.tripService.getTripList().subscribe({
      next: response => {
        this.tripList = response.filter(t => t.isApproved == false)
        console.log(response);

      },
      error: error => {
        console.error(error);
      }
    })
  }

  onSetNextTrip(trip: any) {
    this.tripService.setNextTrip(trip.id).subscribe()
  }

  onSetOngoingTrip(trip: any) {
    this.tripService.setOngoingTrip(trip.id).subscribe()
  }

  onDeleteTrip(id: number) {
    this.tripService.deleteTrip(id).subscribe({
      next: response => {
        location.reload();
      }
    })
  }

  onSearchTripChange() {
    if (this.searchTrip == "") {
      this.tripList = []
      this.getTripList()
    }
    this.tripList = this.tripList.filter(t => t.title.includes(this.searchTrip))
  }

  onApprove(id: number) {
    this.tripService.approve(id).subscribe({
      next: response => {
        location.reload();
      }
    }
    );
  }

  private getTripList() {
    this.tripService.getTripList().subscribe({
      next: (response: ITrip[]) => {
        this.tripList = response
        // this.accountService.getOngoingCount().subscribe({
        //   next: (response: any) => {
        //     this.onGoingCount = response
        //     this.onGoingCount.forEach((element: any) => {
        //       let trip = tripArray.find(t => t.id == element.tripId)
        //       if (trip) {
        //         if (trip.id == element.tripId) {
        //           trip.onGoingCount = element.count
        //           this.trips.push(trip);
        //         }
        //       }
        //     })
      }
    })
  }
}

