import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { HomeContentComponent } from './components/pages/home-content/home-content.component';
import { NgxNavbarModule } from 'ngx-bootstrap-navbar';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    FooterComponent,
    HomeContentComponent,
  ],
  imports: [BrowserModule, AppRoutingModule, NgbModule, NgxNavbarModule],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
