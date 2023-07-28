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
import { HeaderComponent } from './components/static/header/header.component';
import { CategoryComponent } from './components/home/category/category.component';
import { TopComponent } from './components/home/top/top.component';
import { HomeComponent } from './page/home/home.component';
import { BookingStepsComponent } from './components/home/booking-steps/booking-steps.component';
import { TestimonialsComponent } from './components/home/testimonials/testimonials.component';
import { NewsComponent } from './components/home/news/news.component';
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
import { TripFormComponent } from './page/profile/trip-form/trip-form.component';
import { DatePipe } from '@angular/common';
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
    TripFormComponent,
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
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    MatIconModule,
    MatCardModule,
    MatProgressBarModule

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
