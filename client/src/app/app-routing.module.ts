import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { FormGenDemoComponent } from '~/sandbox/form-gen-demo/form-gen-demo.component';
import { YamlSenderComponent } from '~/yaml-sender/yaml-sender.component';

const routes: Routes = [
  { path: '', component: FormGenDemoComponent },
  { path: 'settings', component: YamlSenderComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
