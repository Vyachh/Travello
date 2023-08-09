import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IHotel } from '../models/Hotel';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HotelService {

  constructor(private httpClient: HttpClient) { }

  private baseURL = "https://localhost:7001/Hotel"

  hotels: IHotel[]

  isInfoLoaded: boolean

  getAll() {
    return this.httpClient.get(
      `${this.baseURL}/getall`
    )
  }

  getInfo(): Observable<IHotel[]> {
    if (!this.isInfoLoaded) {
      return this.loadInfo()
    }

    else {
      return new Observable<IHotel[]>(observer => {
        observer.next(this.hotels)
        observer.complete();
      })
    }
  }

  private loadInfo(): Observable<IHotel[]> {
    return this.httpClient.get<IHotel[]>
      (
        `${this.baseURL}/getall`
      )
  }
}
