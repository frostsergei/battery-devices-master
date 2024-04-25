import { Component } from '@angular/core';

import { ToastrService } from 'ngx-toastr';

import { ErrorResponse, Schema, SchemaClient } from '~/client';

@Component({
  selector: 'app-yaml-sender',
  templateUrl: './yaml-sender.component.html',
  styleUrl: './yaml-sender.component.scss',
})
export class YamlSenderComponent {
  public yamlFormText: string = '';
  public yamlParametersText: string = '';
  public responseMessage: string = '';
  public selectedFile: string = 'form';

  constructor(
    private readonly schemaClient: SchemaClient,
    private toastr: ToastrService,
  ) {}

  onReceiveValidYamlText(newText: string): void {
    if (this.selectedFile === 'form') {
      this.sendYamlForm(newText);
    } else if (this.selectedFile === 'parameters') {
      this.sendYamlParameters(newText);
    }
  }

  sendYamlForm(newText: string) {
    this.yamlFormText = newText;
    this.schemaClient.postForm(new Schema({ content: newText })).subscribe({
      next: () => {
        console.log('Success: Form sent successfully');
        this.toastr.success('Форма успешно отправлена!');
        this.responseMessage = 'Form sent successfully';
      },
      error: (err: ErrorResponse) => {
        console.log(`error: ${err.message}`);
        this.toastr.error(`Ошибка: ${err.message}`);
        this.responseMessage = `Error: ${err.message}`;
      },
    });
    console.log(this.yamlFormText);
  }

  sendYamlParameters(newText: string) {
    this.yamlParametersText = newText;
    this.schemaClient
      .postParameters(new Schema({ content: newText }))
      .subscribe({
        next: () => {
          console.log('Success: Parameters sent successfully');
          this.toastr.success('Параметры успешно отправлены!');
          this.responseMessage = 'Parameters sent successfully';
        },
        error: (err: ErrorResponse) => {
          console.log(`error: ${err.message}`);
          this.toastr.error(`Ошибка: ${err.message}`);
          this.responseMessage = `Error: ${err.message}`;
        },
      });
    console.log(this.yamlParametersText);
  }
}
