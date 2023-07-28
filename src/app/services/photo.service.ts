import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { FileType } from '../enum/filetype.enum';

@Injectable({
  providedIn: 'root'
})
export class PhotoService {

  baseURL = "https://localhost:7001/Photo"

  constructor(private httpClient: HttpClient) {

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


  getTripPhoto(tripId: number) {
    return this.httpClient.get(
      `${this.baseURL}/GetTripPhoto?tripId=${tripId}`)
  }

  getPhoto(userId: string): Observable<any> {
    return this.httpClient.get(
      `${this.baseURL}/get?userId=${userId}`, { responseType: 'text' }
    )
  }
}
