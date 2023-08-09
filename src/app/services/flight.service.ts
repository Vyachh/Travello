import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IFlight } from '../models/Flight';

@Injectable({
  providedIn: 'root'
})
export class FlightService {

  private baseUrl = 'https://localhost:7001/Flight'; // Замените на URL вашего бэкенда

  hotels: IFlight[]

  isInfoLoaded: boolean
  
  constructor(private httpClient: HttpClient) { }

  searchFlights(params: any): Observable<any> {
    const url = `${this.baseUrl}/search`;
    return this.httpClient.post(url, params);
  }

  getInfo(): Observable<IFlight[]> {
    if (!this.isInfoLoaded) {
      return this.loadInfo()
    }

    else {
      return new Observable<IFlight[]>(observer => {
        observer.next(this.hotels)
        observer.complete();
      })
    }
  }

  private loadInfo(): Observable<IFlight[]> {
    return this.httpClient.get<IFlight[]>
      (
        `${this.baseUrl}/getall`
      )
  }
}
