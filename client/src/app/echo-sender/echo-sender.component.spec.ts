import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EchoSenderComponent } from './echo-sender.component';

describe('EchoSenderComponent', () => {
  let component: EchoSenderComponent;
  let fixture: ComponentFixture<EchoSenderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [EchoSenderComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(EchoSenderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
