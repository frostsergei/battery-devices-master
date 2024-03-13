import { Component } from '@angular/core';

import { EchoBody, EchoClient, ErrorResponse } from '~/client';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {
  public requestMessage: string = '';
  public responseMessage: string = '';
  public yamlText: string = '';

  constructor(private readonly echoClient: EchoClient) {}

  onSubmit() {
    this.echo();
  }

  echo() {
    console.log(this.requestMessage);
    this.echoClient
      .echo(new EchoBody({ message: this.requestMessage }))
      .subscribe({
        next: (value: EchoBody) => {
          console.log(`response: ${value.message}`);
          this.responseMessage = value.message;
        },
        error: (err: ErrorResponse) => {
          console.log(`error: ${err.message}`);
          this.responseMessage = `Error: ${err.message}`;
        },
      });
  }

  onReceiveValidYamlText(newText: string): void {
    this.yamlText = newText;
    console.log(this.yamlText);
  }
}
