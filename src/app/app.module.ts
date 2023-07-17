import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {FormsModule, ReactiveFormsModule} from '@angular/forms'
import {HttpClientModule} from '@angular/common/http'

import { MatIconModule } from '@angular/material/icon'
import { MatCardModule } from '@angular/material/card';
import { MatProgressBarModule } from '@angular/material/progress-bar';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavComponent } from './components/nav/nav.component';
import { HeaderComponent } from './components/header/header.component';
import { CategoryComponent } from './components/category/category.component';
import { TopComponent } from './components/top/top.component';
import { HomeComponent } from './page/home/home.component';
import { BookingStepsComponent } from './components/booking-steps/booking-steps.component';
import { TestimonialsComponent } from './components/testimonials/testimonials.component';
import { NewsComponent } from './components/news/news.component';
import { FooterComponent } from './components/footer/footer.component';
import { ModalComponent } from './components/modal/modal.component';
import { LoginComponent } from './components/auth/login/login.component';
import { SignUpComponent } from './components/auth/sign-up/sign-up.component';
import { ProfileComponent } from './page/profile/profile.component';
import { DestinationsComponent } from './page/destinations/destinations.component';
import { HotelsComponent } from './page/hotels/hotels.component';
import { FlightsComponent } from './page/flights/flights.component';
import { BookingsComponent } from './page/bookings/bookings.component';

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
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
