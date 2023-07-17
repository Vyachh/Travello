import { HttpClient } from '@angular/common/http';
import { HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs'
import { IUser } from '../models/user';
import { LocalStorageService } from './local-storage.service';
import { IUserInfo } from '../models/userInfo';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  constructor(private httpClient: HttpClient,
    public localStorage: LocalStorageService,) {
    this.token = localStorage.getItem('bearer');
    if (this.token != null) {
      this.isLoggedIn = true;
    }
    this.headers = new HttpHeaders({
      'Content-Type': 'application/json; charset=utf-8',
      'Authorization': this.token ? "bearer " + this.token : ""
    })
  }

  headers: HttpHeaders;
  token: string | null;
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
  getInfo(): Observable<IUserInfo> {
    return this.httpClient.get<IUserInfo>
      (
        `${this.baseURL}/Account/GetInfo`, { headers: this.headers },
      )
  }

  сhangePassword(user: IUser) {
    return this.httpClient.put
      (
        `${this.baseURL}/Account/ChangePassword`, user, { responseType: 'text' }
      )
  }
}
