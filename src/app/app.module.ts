import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms'
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http'

import { MatIconModule } from '@angular/material/icon'
import { MatCardModule } from '@angular/material/card';
import { MatProgressBarModule } from '@angular/material/progress-bar';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavComponent } from './components/static/nav/nav.component';
import { HeaderComponent } from './page/home/header/header.component';
import { CategoryComponent } from './page/home/category/category.component';
import { TopComponent } from './page/home/top/top.component';
import { HomeComponent } from './page/home/home/home.component';
import { BookingStepsComponent } from './page/home/booking-steps/booking-steps.component';
import { TestimonialsComponent } from './page/home/testimonials/testimonials.component';
import { NewsComponent } from './page/home/news/news.component';
import { FooterComponent } from './components/static/footer/footer.component';
import { ModalComponent } from './components/modal/modal.component';
import { LoginComponent } from './components/auth/login/login.component';
import { SignUpComponent } from './components/auth/sign-up/sign-up.component';
import { ProfileComponent } from './page/profile/profile.component';
import { DestinationsComponent } from './page/links/destinations/destinations.component';
import { HotelsComponent } from './page/links/hotels/hotels.component';
import { FlightsComponent } from './page/links/flights/flights.component';
import { BookingsComponent } from './page/links/bookings/bookings.component';
import { AuthIntreceptor } from './services/auth.interceptor';
import { TravelloComponent } from './page/links/travello/travello.component';
import { AdminPanelComponent } from './page/admin-panel/admin-panel.component';
import { CommonModule, DatePipe } from '@angular/common';
import { ChangePasswordComponent } from './page/profile/pages/change-password/change-password.component';

import '@angular/common/locales/global/ru';
import { DayPipe } from './pipes/day.pipe';
import { MonthPipe } from './pipes/month.pipe';
import { TripListComponent } from './page/admin-panel/pages/trip-list/trip-list.component';
import { SidebarComponent } from './page/profile/sidebar/sidebar.component';
import { AdminSidebarComponent } from './page/admin-panel/admin-sidebar/admin-sidebar.component';
import { AdminHomeComponent } from './page/admin-panel/pages/admin-home/admin-home.component';
import { UserInfoChangeComponent } from './page/profile/pages/user-info-change/user-info-change.component';
import { BirthDateChangeComponent } from './page/profile/components/birth-date-change/birth-date-change.component';
import { OngoingTripComponent } from './page/links/travello/ongoing-trip/ongoing-trip.component';
import { NextTripComponent } from './page/links/travello/next-trip/next-trip.component';
import { AddParticipantsComponent } from './page/admin-panel/pages/add-participants/add-participants.component';
import { TruncatePipe } from './pipes/truncate.pipe';
import { SubscribeTripComponent } from './page/links/travello/next-trip/subscribe-trip/subscribe-trip.component';
import { AdminChangeUserInformationComponent } from './page/admin-panel/pages/admin-change-user-information/admin-change-user-information.component';
import { RolePipe } from './pipes/role.pipe';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { TripComponent } from './page/trip/trip.component';
import { TripEditComponent } from './page/trip/trip-edit/trip-edit.component';
import { FilterByGradePipe } from './pipes/filter-by-grade.pipe';
import { FilterByCityPipe } from './pipes/filter-by-city.pipe';
import { AdminTripFormComponent } from './page/admin-panel/pages/admin-trip-form/admin-trip-form.component';
import { ToastrModule } from 'ngx-toastr';
import { NotFoundComponent } from './page/not-found/not-found.component';
import { PaymentComponent } from './page/payment/payment.component';

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HeaderComponent,
    CategoryComponent,
    TopComponent,
    HomeComponent,
    BookingStepsComponent,
    TestimonialsComponent,
    NewsComponent,
    FooterComponent,
    ModalComponent,
    LoginComponent,
    SignUpComponent,
    ProfileComponent,
    DestinationsComponent,
    HotelsComponent,
    FlightsComponent,
    BookingsComponent,
    TravelloComponent,
    AdminPanelComponent,
    SidebarComponent,
    ChangePasswordComponent,
    DayPipe,
    MonthPipe,
    TripListComponent,
    AdminSidebarComponent,
    AdminHomeComponent,
    UserInfoChangeComponent,
    BirthDateChangeComponent,
    OngoingTripComponent,
    NextTripComponent,
    AddParticipantsComponent,
    TruncatePipe,
    SubscribeTripComponent,
    AdminChangeUserInformationComponent,
    RolePipe,
    TripComponent,
    TripEditComponent,
    FilterByGradePipe,
    FilterByCityPipe,
    AdminTripFormComponent,
    NotFoundComponent,
    PaymentComponent,
  ],
  imports: [
    CommonModule,
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    MatIconModule,
    MatCardModule,
    MatProgressBarModule,
    FontAwesomeModule,
    ToastrModule.forRoot(),
  ],
  providers: [{
    provide: HTTP_INTERCEPTORS,
    useClass: AuthIntreceptor,
    multi: true,
  },
    DatePipe

  ],
  bootstrap: [AppComponent]
})

export class AppModule { }
