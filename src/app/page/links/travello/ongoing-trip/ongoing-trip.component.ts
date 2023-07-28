import { Component, Input } from '@angular/core';
import { ITrip } from 'src/app/models/Trip';

@Component({
  selector: 'app-ongoing-trip',
  templateUrl: './ongoing-trip.component.html',
  styleUrls: ['./ongoing-trip.component.css']
})
export class OngoingTripComponent {

  @Input() ongoingTrip: ITrip

}
