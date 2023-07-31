import { Injectable } from '@angular/core';
import { ITrip } from '../models/Trip';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FileType } from '../enum/filetype.enum';

@Injectable({
  providedIn: 'root'
})
export class TripService {

  constructor(private httpClient: HttpClient) { }
  baseURL = "https://localhost:7001/Trip"

  getById(id: number): Observable<ITrip> {
    return this.httpClient.get<ITrip>(
      `${this.baseURL}/GetTrip?id=${id}`
    )
  }


  addTrip(formData: FormData) {
    return this.httpClient.post(
      `${this.baseURL}/CreateTrip`, formData,
      { responseType: 'text' })
  }


  getNextTrip(): Observable<ITrip> {
    return this.httpClient.get<ITrip>(
      `${this.baseURL}/GetNextTrip`
    )
  }

  getOngoingTrip(): Observable<ITrip> {
    return this.httpClient.get<ITrip>(
      `${this.baseURL}/GetOngoingTrip`
    )
  }

  setNextTrip(id: number): Observable<ITrip> {
    return this.httpClient.get<ITrip>(
      `${this.baseURL}/SetNextTrip?id=${id}`
    )
  }
  setOngoingTrip(id: number): Observable<ITrip> {
    return this.httpClient.get<ITrip>(
      `${this.baseURL}/SetOngoingTrip?id=${id}`
    )
  }

  getTripList(): Observable<ITrip[]> {
    return this.httpClient.get<ITrip[]>(
      `${this.baseURL}/GetTripList`
    )
  }

  deleteTrip(id: number) {
    return this.httpClient.delete(
      `${this.baseURL}/Delete?id=${id}`)
  }

  uploadPhoto(photo: File, userId: string, fileType: FileType) {
    const formData = new FormData();
    formData.append('photo', photo)
    formData.append('userId', userId)
    formData.append('fileType', fileType)

    return this.httpClient.post(
      `${this.baseURL}/upload`, formData
    )
  }
}
