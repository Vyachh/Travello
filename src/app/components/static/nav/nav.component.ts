import { Component, ElementRef, HostListener, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { IUserInfo } from 'src/app/models/UserInfo';
import { AccountService } from 'src/app/services/account.service';
import { ModalService } from 'src/app/services/modal.service';
import { PhotoService } from 'src/app/services/photo.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  [x: string]: any;

  isLoggedIn: boolean
  isAdmin: boolean = false;
  @ViewChild('menu') menu: ElementRef;
  isMenuOpen: boolean = false;
  hasPhoto = false;

  userInfo: IUserInfo

  constructor(
    public modalService: ModalService,
    public accountService: AccountService,
    private router: Router,
    private notifier:ToastrService
  ) {
    this.isLoggedIn = accountService.isLoggedIn;
  }

  ngOnInit(): void {
    if (this.isLoggedIn) {
      this.accountService.getInfo().subscribe({
        next: userInfo => {
          this.userInfo = userInfo
          if (userInfo.image) {
            this.hasPhoto = true
          }
          if (userInfo.role == "Admin") {
            this.isAdmin = true
          }
        },
        error: error => {
          console.error(error);
          this.notifier.error('','The User information has not been loaded.', {
            timeOut: 3000,
            positionClass: 'toast-bottom-right',
          });
        }
      })
    }
  }


  
  // @HostListener('document:click', ['$event.target'])
  // if (isLoggedIn) {

  // }
  // onClickOutside(target: any) {
  //   const menuElement = this.menu.nativeElement;
  //   if (!menuElement.contains(target)) {
  //     this.isMenuOpen = false;
  //   }
  // }

  onSignOut() {
    this.accountService.signOut()
  }

  onRedirect(page: string) {
    this.router.navigate([`/${page}`])
  }
}
