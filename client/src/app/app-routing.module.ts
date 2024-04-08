import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { FormComponent } from '~/sandbox/form/form.component';
import { YamlSenderComponent } from '~/yaml-sender/yaml-sender.component';

const routes: Routes = [
  { path: '', component: FormComponent },
  { path: 'settings', component: YamlSenderComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
