import { Component } from '@angular/core';

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
  public selectedFile: string = 'parameters';

  constructor(private readonly schemaClient: SchemaClient) {}

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
        this.responseMessage = 'Form sent successfully';
      },
      error: (err: ErrorResponse) => {
        console.log(`error: ${err.message}`);
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
          this.responseMessage = 'Parameters sent successfully';
        },
        error: (err: ErrorResponse) => {
          console.log(`error: ${err.message}`);
          this.responseMessage = `Error: ${err.message}`;
        },
      });
    console.log(this.yamlParametersText);
  }
}
