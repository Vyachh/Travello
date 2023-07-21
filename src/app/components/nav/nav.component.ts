import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { IUserInfo } from 'src/app/models/userInfo';
import { AccountService } from 'src/app/services/account.service';
import { ModalService } from 'src/app/services/modal.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent {

  isLoggedIn: boolean
  isAdmin: boolean
  isMenuOpen: boolean = false;

  userInfo: IUserInfo

  constructor(public modalService: ModalService,
    public accountService: AccountService,
    private router: Router) {
    this.isLoggedIn = accountService.isLoggedIn;
    this.isAdmin = true
    
    accountService.getInfo().subscribe({
      next: userInfo => {
        this.userInfo = userInfo
      },
      error: error => {
        console.error(error);
      }
    })
  }

  onSignOut(){
    this.accountService.signOut()
  }

  onRedirect(page: string) {
    this.router.navigate([`/${page}`])
  }
}
