import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { CarroComponent } from './carro/carro.component';
import { NavComponent } from './nav/nav.component';
import { AuthService } from './_services/auth.service';
import { HomeComponent } from './home/home.component';
import { RegistroComponent } from './registro/registro.component';
import { ErrorInterceptorProvider } from './_services/error.interceptor';
import { AlertifyService } from './_services/alertify.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
  declarations: [
    AppComponent,
    CarroComponent,
    NavComponent,
    HomeComponent,
    RegistroComponent,
  ],
  imports: [BrowserModule, HttpClientModule, FormsModule, BrowserAnimationsModule],
  providers: [AuthService, ErrorInterceptorProvider],
  bootstrap: [AppComponent],
})
export class AppModule {}
