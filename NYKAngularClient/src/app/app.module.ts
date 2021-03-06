import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS} from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { 
  AppComponent,
  DaysComponent,
  MenuComponent,
  ConfirmComponent,
  RegistrationComponent,
  RegistrationListComponent
} from './components';
import { 
  AuthInterceptor 
} from './services';

@NgModule({
  declarations: [
    AppComponent,
    DaysComponent,
    MenuComponent,
    ConfirmComponent,
    RegistrationComponent,
    RegistrationListComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
