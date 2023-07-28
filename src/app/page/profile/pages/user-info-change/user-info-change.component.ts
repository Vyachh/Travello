import { Component, OnInit } from '@angular/core';
import { ModalComponent } from 'src/app/components/modal/modal.component';
import { IUserInfo } from 'src/app/models/UserInfo';
import { AccountService } from 'src/app/services/account.service';
import { ModalService } from 'src/app/services/modal.service';

@Component({
  selector: 'app-user-info-change',
  templateUrl: './user-info-change.component.html',
  styleUrls: ['./user-info-change.component.css']
})
export class UserInfoChangeComponent implements OnInit {

  userInfo: IUserInfo

  constructor(private accountService: AccountService, public modalService: ModalService) {

  }

  ngOnInit(): void {
    this.accountService.getInfo().subscribe({
      next: userInfo => {
        this.userInfo = userInfo
        
      },
      error: error => {
        console.error(error);
      }
    })
  }

}
