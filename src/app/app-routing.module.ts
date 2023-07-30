import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './page/home/home.component';
import { ProfileComponent } from './page/profile/profile.component';
import { DestinationsComponent } from './page/links/destinations/destinations.component';
import { HotelsComponent } from './page/links/hotels/hotels.component';
import { FlightsComponent } from './page/links/flights/flights.component';
import { BookingsComponent } from './page/links/bookings/bookings.component';
import { TravelloComponent } from './page/links/travello/travello.component';
import { AdminPanelComponent } from './page/admin-panel/admin-panel.component';
import { ChangePasswordComponent } from './page/profile/pages/change-password/change-password.component';
import { TripListComponent } from './page/admin-panel/pages/trip-list/trip-list.component';
import { UserInfoChangeComponent } from './page/profile/pages/user-info-change/user-info-change.component';
import { AddParticipantsComponent } from './page/admin-panel/pages/add-participants/add-participants.component';
import { TripFormComponent } from './page/profile/trip-form/trip-form.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'profile', component: ProfileComponent },
  { path: 'profile/changepassword', component: ChangePasswordComponent },
  { path: 'profile/changeinfo', component: UserInfoChangeComponent },
  { path: 'profile/createtrip', component: TripFormComponent },
  { path: 'destinations', component: DestinationsComponent },
  { path: 'hotels', component: HotelsComponent },
  { path: 'flights', component: FlightsComponent },
  { path: 'bookings', component: BookingsComponent },
  { path: 'travello', component: TravelloComponent },
  { path: 'adminpanel', component: AdminPanelComponent },
  { path: 'adminpanel/triplist', component: TripListComponent },
  { path: 'adminpanel/addparticipants', component: AddParticipantsComponent },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
