import { Component } from '@angular/core';

import { EchoClient, ErrorResponse, TextMessage } from '~/client';

@Component({
  selector: 'app-echo-sender',
  templateUrl: './echo-sender.component.html',
  styleUrl: './echo-sender.component.scss',
})
export class EchoSenderComponent {
  public requestMessage: string = '';
  public responseMessage: string = '';

  constructor(private readonly echoClient: EchoClient) {}

  onSubmit() {
    this.echo();
  }

  echo() {
    console.log(this.requestMessage);
    this.echoClient
      .echo(new TextMessage({ message: this.requestMessage }))
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
  }
}
