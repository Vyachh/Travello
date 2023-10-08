import { AfterViewInit, Component, Input, OnInit } from '@angular/core';
import { ITrip } from 'src/app/models/ITrip';
import { IUserInfo } from 'src/app/models/UserInfo';
import { AccountService } from 'src/app/services/account.service';
import { ModalService } from 'src/app/services/modal.service';

@Component({
  selector: 'app-next-trip',
  templateUrl: './next-trip.component.html',
  styleUrls: ['./next-trip.component.css']
})
export class NextTripComponent implements AfterViewInit {
  @Input() nextTrip: ITrip

  constructor(public accountService: AccountService, public modalService: ModalService) {

  }
  
  ngAfterViewInit(): void {
    this.accountService.getInfo().subscribe({
      next: response => {
        this.userInfo = response
      }
    })
  }

  userInfo: IUserInfo

}
