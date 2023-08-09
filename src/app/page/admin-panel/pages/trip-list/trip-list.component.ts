import { Component } from '@angular/core';
import { ITrip } from 'src/app/models/Trip';
import { AccountService } from 'src/app/services/account.service';
import { ModalService } from 'src/app/services/modal.service';
import { TripService } from 'src/app/services/trip.service';

@Component({
  selector: 'app-trip-list',
  templateUrl: './trip-list.component.html',
  styleUrls: ['./trip-list.component.css']
})
export class TripListComponent {
  tripList: ITrip[]

  constructor(private tripService: TripService, public accountService: AccountService, public modalService: ModalService) {

  }

  ngOnInit(): void {
    this.getTripList();
  }


  getTripList() {
      this.tripService.getTripList().subscribe({
        next: response => {
          this.tripList = response.filter(t => t.isApproved == true)
        },
        error: error => {
          console.error(error);
        }
      })
  }

  onSetNextTrip(trip: any) {
    console.log(trip);
    
    if (trip.isOngoingTrip) {
      return console.error();
    }
    this.tripService.setNextTrip(trip.id).subscribe({
      next: response => {
        this.getTripList();
      }
    })
  }

  onSetOngoingTrip(trip: any) {
    if (trip.isNextTrip) {
      return console.error();
    }
    this.tripService.setOngoingTrip(trip.id).subscribe({
      next: response => {
        this.getTripList();
      }
    })
  }

  onDeleteTrip(trip: any) {
    this.tripService.deleteTrip(trip.id).subscribe({
      next: response => {
        this.getTripList();
      }
    })
  }
  onEdit(id: number) {
    this.tripService.selectedTrip = id
    this.modalService.onEditTripButtonClick()
  }
}
