import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ITrip } from 'src/app/models/ITrip';
import { AccountService } from 'src/app/services/account.service';
import { ModalService } from 'src/app/services/modal.service';
import { TripService } from 'src/app/services/trip.service';

@Component({
  selector: 'app-trip-list',
  templateUrl: './trip-list.component.html',
  styleUrls: ['./trip-list.component.css'],
})
export class TripListComponent {
  tripList: ITrip[];

  constructor(
    private tripService: TripService,
    public accountService: AccountService,
    public modalService: ModalService,
    private notifier: ToastrService
  ) {}

  ngOnInit(): void {
    this.getTripList();
  }

  getTripList() {
    this.tripService.getTripList().subscribe({
      next: (response) => {
        this.tripList = response.filter((t) => t.isApproved == true);
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  onSetNextTrip(trip: any) {
    if (trip.isOngoingTrip) {
      this.notifier.error('', 'The next trip has been already set.');
      return;
    }
    this.tripService.setNextTrip(trip.id).subscribe({
      next: (response) => {
        this.getTripList();
      },
      error: (error) => {
        this.notifier.error('', 'Next trip has not been setted.');
      },
    });
  }

  onSetOngoingTrip(trip: ITrip) {
    if (trip.isNextTrip) {
      this.notifier.error('', 'The ongoing trip has been already set.');
      return;
    }
    this.tripService.setOngoingTrip(trip.id).subscribe({
      next: (response) => {
        this.getTripList();
      },
    });
  }

  onUndoOngoingTrip(trip: ITrip) {
    if (!trip.isOngoingTrip) {
      this.notifier.error('', 'This is not ongoing trip.');
      return;
    }
    this.tripService.undoOngoingTrip(trip.id).subscribe({
      next: (response) => {
        this.getTripList();
      },
    });
  }

  onUndoNextTrip(trip: ITrip) {
    if (!trip.isNextTrip) {
      this.notifier.error('', 'This is not next trip.');
      return;
    }
    this.tripService.undoNextTrip(trip.id).subscribe({
      next: (response) => {
        this.getTripList();
      },
    });
  }

  onDeleteTrip(trip: any) {
    this.tripService.deleteTrip(trip.id).subscribe({
      next: (response) => {
        this.getTripList();
      },
    });
  }

  onEdit(id: number) {
    this.tripService.selectedTrip = id;
    this.modalService.onEditTripButtonClick();
  }
}
