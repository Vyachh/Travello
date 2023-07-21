import { Injectable } from '@angular/core';
import { ITrip } from '../models/trip';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TripService {

  constructor(private httpClient: HttpClient) { }
  baseURL = "https://localhost:7001/Trip"

  addTrip(trip: ITrip) {
    return this.httpClient.post(
      `${this.baseURL}/CreateTrip`, trip,
      { responseType: 'text' })
  }
  
  getNextTrip(): Observable<ITrip> {
    return this.httpClient.get<ITrip>(
      `${this.baseURL}/GetNextTrip`
    )
  }

  getTripList(): Observable<ITrip[]> {
    return this.httpClient.get<ITrip[]>(
      `${this.baseURL}/GetTripList`
    )
  }
}
