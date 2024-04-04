import { Component } from '@angular/core';

import {
  TextMessage,
  ErrorResponse,
  YamlConfigClient,
  YamlConfigurationBody,
} from '~/client';

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

  constructor(private readonly yamlConfigClient: YamlConfigClient) {}

  onReceiveValidYamlText(newText: string): void {
    if (this.selectedFile === 'form') {
      this.sendYamlForm(newText);
    } else if (this.selectedFile === 'parameters') {
      this.sendYamlParameters(newText);
    }
  }

  sendYamlForm(newText: string) {
    this.yamlFormText = newText;
    this.yamlConfigClient
      .postForm(new YamlConfigurationBody({ yamlConfiguration: newText }))
      .subscribe({
        next: (value: TextMessage) => {
          console.log(`response: ${value.message}`);
          this.responseMessage = value.message;
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
    this.yamlConfigClient
      .postParameters(new YamlConfigurationBody({ yamlConfiguration: newText }))
      .subscribe({
        next: (value: TextMessage) => {
          console.log(`response: ${value.message}`);
          this.responseMessage = value.message;
        },
        error: (err: ErrorResponse) => {
          console.log(`error: ${err.message}`);
          this.responseMessage = `Error: ${err.message}`;
        },
      });
    console.log(this.yamlParametersText);
  }
}
