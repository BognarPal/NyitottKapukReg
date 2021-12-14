import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { 
  DaysComponent,
  RegistrationComponent,
  MenuComponent,
  RegistrationListComponent 
} from './components';

const routes: Routes = [
  {path: 'days', component: DaysComponent},
  {path: 'registration', component: RegistrationComponent},
  {path: 'registration-list', component: RegistrationListComponent},
  {path: '', component: MenuComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
