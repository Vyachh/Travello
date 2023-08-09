import { HttpClient } from '@angular/common/http';
import { HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, of, switchMap } from 'rxjs'
import { IUser } from '../models/User';
import { LocalStorageService } from './local-storage.service';
import { IUserInfo } from '../models/UserInfo';
import { PhotoService } from './photo.service';


@Injectable({
  providedIn: 'root'
})
export class AccountService {
  constructor(private httpClient: HttpClient,
    public localStorage: LocalStorageService,
    public photoService: PhotoService
  ) {
    this.token = localStorage.getItem('bearer');
    if (this.token != null) {
      this.isLoggedIn = true;
    }
    this.headers = new HttpHeaders({
      'Content-Type': 'application/json; charset=utf-8',
      'Authorization': this.token ? "bearer " + this.token : ""
    })

  }

  private headers: HttpHeaders;
  private token: string | null;
  private baseURL = "https://localhost:7001/Account"
  user: IUser;
  userInfo: IUserInfo;

  isLoggedIn: boolean = false;
  isAdmin: boolean = false;
  isInfoLoaded: boolean = false;


  getInfo(): Observable<IUserInfo> {
    if (!this.isInfoLoaded) {
      return this.loadInfo().pipe(
        switchMap((userInfo: IUserInfo) => {
          return this.photoService.getPhoto(userInfo.id).pipe(
            catchError(error => {
              console.log("Фото не найдено");
              return of('')
            }),
            switchMap((photoData: any) => {
              userInfo.image = photoData;
              this.userInfo = userInfo;
              this.isInfoLoaded = true;
              return of(userInfo)
            })
          )
        })
      );
    }
    else {
      return new Observable<IUserInfo>(observer => {
        observer.next(this.userInfo)
        observer.complete();
      })
    }
  }

  private loadInfo(): Observable<IUserInfo> {
    return this.httpClient.get<IUserInfo>
      (
        `${this.baseURL}/GetInfo`,
        { headers: this.headers },
      )
  }

  getPhoto() {
    return this.httpClient.get
      (
        `${this.baseURL}/GetInfo`,
        { headers: this.headers },
      )
  }

  getCurrentTripId(id: string):Observable<number> {
    return this.httpClient.get<number>(
      `${this.baseURL}/GetCurrentTrip?id=${id}`
    )
  }

  getAll(): Observable<any> {
    return this.httpClient.get
      (
        `${this.baseURL}/GetAll`
      )
  }

  getOngoingCount() {
    return this.httpClient.get(`${this.baseURL}/GetOngoingCount`)
  }

  login(user: IUser): Observable<any> {
    return this.httpClient.post
      (`${this.baseURL}/Login`, user, { responseType: 'text' })
  }

  signUp(user: IUser): Observable<any> {
    return this.httpClient.post
      (
        `${this.baseURL}/SignUp`, user, { responseType: 'text' }
      )
  }

  signOut() {
    localStorage.clear();
    location.reload();
  }

  changeInfo(user: IUserInfo): Observable<any> {
    return this.httpClient.put
      (`${this.baseURL}/ChangeInfo`, user, { responseType: 'text' })
  }

  сhangePassword(user: IUser) {
    return this.httpClient.put
      (
        `${this.baseURL}/ChangePassword`, user, { responseType: 'text' }
      )
  }

  refreshToken() {
    return this.httpClient.post
      (
        `${this.baseURL}/RefreshToken`, { headers: this.headers }
      )
  }

  isLoggedInCall() {
    return this.isLoggedIn
  }

  addUsersToTrip(userList: IUserInfo[]) {
    return this.httpClient.put
      (
        `${this.baseURL}/SetCurrentTrip`, userList,
        { headers: this.headers },
      )
  }


}
