import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { IFlight } from 'src/app/models/Flight';
import { FlightService } from 'src/app/services/flight.service';

@Component({
  selector: 'app-flights',
  templateUrl: './flights.component.html',
  styleUrls: ['./flights.component.css']
})
export class FlightsComponent implements OnInit {
  searchForm: FormGroup;
  flights: IFlight[]

  constructor(private formBuilder: FormBuilder, private flightService: FlightService) {
    this.searchForm = this.formBuilder.group({
      departure: 'New York',
      arrival: 'Los Angeles',
      departureTime: '',
      arrivalTime: '',
      passengers: 1
    });
  }

  ngOnInit(): void {
    this.getAll();
  }

  getAll() {
    this.flightService.getInfo().subscribe({
      next: flights => {
        this.flights = flights
      }
    })
  }

  onSubmit() {
    const searchParams = this.searchForm.value;

    
    this.flightService.searchFlights(searchParams).subscribe(
      {
        next: flights => {
          console.log(flights);
          
          this.flights = flights
        }
      })


  }
}
