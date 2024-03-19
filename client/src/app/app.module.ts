import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { SandboxModule } from './sandbox/sandbox.module';

import { AppRoutingModule } from '~/app-routing.module';
import { AppComponent } from '~/app.component';
import { API_BASE_URL, EchoClient } from '~/client';
import { SharedModule } from '~/shared/shared.module';
import { YamlEditorModule } from '~/yaml-editor/yaml-editor.module';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    FormsModule,
    YamlEditorModule,
    MatSlideToggleModule,
    SharedModule,
    SandboxModule,
    BrowserAnimationsModule,
  ],
  providers: [
    EchoClient,
    { provide: API_BASE_URL, useValue: 'https://localhost:7155' },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
