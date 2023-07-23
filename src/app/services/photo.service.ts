import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PhotoService {

  baseURL = "https://localhost:7001/Photo"

  constructor(private httpClient: HttpClient) {

  }

  uploadPhoto(photo: File, userId: string) {
    const formData = new FormData();
    formData.append('photo', photo)
    formData.append('userId', userId)

    return this.httpClient.post(
      `${this.baseURL}/upload`, formData
    )
  }
  getPhoto(userId: string): Observable<any> {
    return this.httpClient.get(
      `${this.baseURL}/get?userId=${userId}`, {responseType: 'text'}
    )
  }
}
