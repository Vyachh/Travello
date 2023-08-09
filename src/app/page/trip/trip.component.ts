import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ITrip } from 'src/app/models/Trip';
import { AccountService } from 'src/app/services/account.service';
import { ModalService } from 'src/app/services/modal.service';
import { TripService } from 'src/app/services/trip.service';
import { TripEditComponent } from './trip-edit/trip-edit.component';

@Component({
  selector: 'app-trip',
  templateUrl: './trip.component.html',
  styleUrls: ['./trip.component.css']
})
export class TripComponent implements OnInit {
  @Input() id: number

  trip: ITrip

  constructor(private route: ActivatedRoute, public tripService: TripService,
    public modalService: ModalService
  ) {

  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const id = +params['id']

      this.tripService.getById(id).subscribe({
        next: (response: any) => {
          this.trip = response.result
        }
      })
    })
  }

  onEdit() {
    console.log('aa');
    
    // this.tripService.selectedTrip = trip.id
    console.log(this.tripService.selectedTrip);
    
    this.modalService.onEditTripButtonClick()

  }
}
