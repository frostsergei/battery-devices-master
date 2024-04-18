import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { SandboxModule } from './sandbox/sandbox.module';
import { YamlSenderModule } from './yaml-sender/yaml-sender.module';

import { AppRoutingModule } from '~/app-routing.module';
import { AppComponent } from '~/app.component';
import { API_BASE_URL, YamlConfigClient } from '~/client';
import { SharedModule } from '~/shared/shared.module';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    FormsModule,
    YamlSenderModule,
    SharedModule,
    SandboxModule,
    BrowserAnimationsModule,
  ],
  providers: [
    YamlConfigClient,
    { provide: API_BASE_URL, useValue: 'https://localhost:7155' },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
