import { Injectable } from '@angular/core';
import { ITrip } from '../models/ITrip';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, filter, map, switchMap } from 'rxjs';
import { FileType } from '../enum/filetype.enum';
import { TripForm } from '../page/admin-panel/pages/admin-trip-form/TripForm';

@Injectable({
  providedIn: 'root',
})
export class TripService {
  tripList: ITrip[];

  isInfoLoaded: boolean = false;

  private headers: HttpHeaders;
  private token: string | null;

  selectedTrip: number;

  constructor(private httpClient: HttpClient) {
    this.token = localStorage.getItem('bearer');
    this.headers = new HttpHeaders({
      'Content-Type': 'application/json; charset=utf-8',
      Authorization: this.token ? 'bearer ' + this.token : '',
    });
  }
  private baseURL = 'https://localhost:7001/Trip';

  getById(id: number): Observable<ITrip> {
    return this.httpClient.get<ITrip>(`${this.baseURL}/GetTrip?id=${id}`);
  }

  getInfo(): Observable<ITrip[]> {
    if (!this.isInfoLoaded) {
      return this.loadInfo();
    } else {
      return new Observable<ITrip[]>((observer) => {
        observer.next(this.tripList);
        observer.complete();
      });
    }
  }

  private loadInfo(): Observable<ITrip[]> {
    return this.httpClient.get<ITrip[]>(`${this.baseURL}/GetTripList`, {
      headers: this.headers,
    });
  }

  addTrip(formData: FormData) {
    return this.httpClient.post(`${this.baseURL}/CreateTrip`, formData, {
      responseType: 'text',
    });
  }

  getNextTrip(): Observable<ITrip> {
    return this.httpClient.get<ITrip>(`${this.baseURL}/GetNextTrip`);
  }

  getOngoingTrip(): Observable<ITrip> {
    return this.httpClient.get<ITrip>(`${this.baseURL}/GetOngoingTrip`);
  }

  setNextTrip(id: number): Observable<ITrip> {
    return this.httpClient.get<ITrip>(`${this.baseURL}/SetNextTrip?id=${id}`);
  }

  undoNextTrip(id: number): Observable<ITrip> {
    return this.httpClient.get<ITrip>(`${this.baseURL}/UndoNextTrip?id=${id}`);
  }

  setOngoingTrip(id: number): Observable<ITrip> {
    return this.httpClient.get<ITrip>(
      `${this.baseURL}/SetOngoingTrip?id=${id}`
    );
  }

  undoOngoingTrip(id: number): Observable<ITrip> {
    return this.httpClient.get<ITrip>(
      `${this.baseURL}/UndoOngoingTrip?id=${id}`
    );
  }

  getTripList(): Observable<ITrip[]> {
    return this.httpClient.get<ITrip[]>(`${this.baseURL}/GetTripList`);
  }

  getApprovedTripList(): Observable<ITrip[]> {
   return this.httpClient.get<ITrip[]>(`${this.baseURL}/GetTripList`)
    .pipe(
      map(tripList => tripList.filter(t => t.isApproved))
    )
  }

  approve(id: number) {
    return this.httpClient.put(`${this.baseURL}/Approve?id=${id}`, {
      responseType: 'text',
    });
  }

  updateTrip(formData: FormData) {
    return this.httpClient.put(`${this.baseURL}/Update`, formData, {
      responseType: 'text',
    });
  }

  deleteTrip(id: number) {
    return this.httpClient.delete(`${this.baseURL}/Delete?id=${id}`);
  }

  uploadPhoto(photo: File, userId: string, fileType: FileType) {
    const formData = new FormData();
    formData.append('photo', photo);
    formData.append('userId', userId);
    formData.append('fileType', fileType);

    return this.httpClient.post(`${this.baseURL}/upload`, formData);
  }
}
