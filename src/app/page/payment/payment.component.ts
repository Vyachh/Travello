import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ITrip } from 'src/app/models/ITrip';
import { TripService } from 'src/app/services/trip.service';
import { loadStripe } from '@stripe/stripe-js';

@Component({
  selector: 'app-payment',
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.css'],
})
export class PaymentComponent implements OnInit {
  @Input() id: number;

  trip: ITrip
  constructor(private tripService: TripService, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      const id = +params['id'];

      this.tripService.getById(id).subscribe({
        next: (response: ITrip) => {
          if (response.id == 0) {
            return;
          }
          this.trip = response;
        },
        error: (error) => {
          console.log(error);
        },
      });
    });
  }
}
