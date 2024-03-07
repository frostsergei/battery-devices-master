import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from '~/app-routing.module';
import { AppComponent } from '~/app.component';
import { API_BASE_URL, EchoClient } from '~/client';

@NgModule({
  declarations: [AppComponent],
  imports: [BrowserModule, HttpClientModule, AppRoutingModule, FormsModule],
  providers: [
    EchoClient,
    { provide: API_BASE_URL, useValue: 'https://localhost:7155' },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
