import { HttpClient } from '@angular/common/http';
import { HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs'
import { IUser } from '../models/user';
import { LocalStorageService } from './local-storage.service';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  constructor(private httpClient: HttpClient, public localStorage: LocalStorageService) {
    if (localStorage.getItem('bearer') != null) {
      this.isLoggedIn = true;
    }
  }

  baseURL = "https://localhost:7001"
  user: IUser;
  isLoggedIn: boolean;


  login(user: IUser): Observable<any> {
    return this.httpClient.post
      (`${this.baseURL}/Account/Login`, user, { responseType: 'text' })
  }

  signUp(user: IUser): Observable<any> {
    return this.httpClient.post
      (
        `${this.baseURL}/Account/SignUp`, user, { responseType: 'text' }
      )
  }

}
