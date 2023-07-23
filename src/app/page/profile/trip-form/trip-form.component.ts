import { Component, OnInit, Input } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ITrip } from 'src/app/models/trip';
import { IUserInfo } from 'src/app/models/userInfo';
import { TripService } from 'src/app/services/trip.service';
import { DatePipe } from '@angular/common';
//
@Component({
  selector: 'app-trip-form',
  templateUrl: './trip-form.component.html',
  styleUrls: ['./trip-form.component.css']
})
export class TripFormComponent implements OnInit {

  @Input() userInfo: IUserInfo
  @Input() trip: ITrip

  /**
   *
   */
  constructor(private tripService: TripService, private datePipe: DatePipe) {

  }

  tripForm = new FormGroup({
    userId: new FormControl<string>('', [
      Validators.required
    ]),
    title: new FormControl<string>('', [
      Validators.required,
      Validators.minLength(6)
    ]),
    description: new FormControl<string>('', [
      Validators.required,
    ]),
    dateStart: new FormControl<string>('', [
      Validators.required,
    ]),
    dateEnd: new FormControl<string>('', [
      Validators.required,
    ]),
    image: new FormControl<string>('', [
      Validators.required
    ])
  })

  ngOnInit(): void {

  }

  onSubmit() {
    this.trip.userId = this.userInfo.id
    this.trip.description = this.tripForm.value.description || ""
    this.trip.title = this.tripForm.value.title || ""
    this.trip.dateFrom = this.tripForm.value.dateStart || ""
    this.trip.dateTo = this.tripForm.value.dateEnd || ""
    this.trip.author = this.userInfo.userName
    this.trip.image = this.tripForm.value.image || ""
    console.log(this.tripForm.value);

    this.tripService.addTrip(this.trip).subscribe({
      next: response => {
        console.log(response);
      },
      error: e => {
        console.error(e);
      }
    });

  }
}
