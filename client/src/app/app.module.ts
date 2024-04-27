import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { ToastrModule } from 'ngx-toastr';

import { SandboxModule } from './sandbox/sandbox.module';
import { YamlSenderModule } from './yaml-sender/yaml-sender.module';

import { AppRoutingModule } from '~/app-routing.module';
import { AppComponent } from '~/app.component';
import { API_BASE_URL, SchemaClient } from '~/client';
import * as env from '~/env';
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
    ToastrModule.forRoot(),
  ],
  providers: [
    SchemaClient,
    { provide: API_BASE_URL, useValue: env.API_BASE_URL },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
