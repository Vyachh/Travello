import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './page/home/home.component';
import { ProfileComponent } from './page/profile/profile.component';
import { DestinationsComponent } from './page/destinations/destinations.component';
import { HotelsComponent } from './page/hotels/hotels.component';
import { FlightsComponent } from './page/flights/flights.component';
import { BookingsComponent } from './page/bookings/bookings.component';
import { TravelloComponent } from './page/travello/travello.component';
import { AdminPanelComponent } from './page/admin-panel/admin-panel/admin-panel.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'profile', component: ProfileComponent },
  { path: 'destinations', component: DestinationsComponent },
  { path: 'hotels', component: HotelsComponent },
  { path: 'flights', component: FlightsComponent },
  { path: 'bookings', component: BookingsComponent },
  { path: 'travello', component: TravelloComponent },
  { path: 'adminpanel', component: AdminPanelComponent },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
