import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { text } from '@fortawesome/fontawesome-svg-core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EmailService {

  constructor(private httpClient: HttpClient) {
    this.headers = new HttpHeaders({
      'Content-Type': 'application/json; charset=utf-8'
    })
  }

  private headers: HttpHeaders;

  private baseURL = "https://localhost:7001/Email"

  subscribeToNews(data: FormData): Observable<any> {
    return this.httpClient.post(
      `${this.baseURL}/sendemail`, data
    )
  }
}
