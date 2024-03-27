import { ComponentFixture, TestBed } from '@angular/core/testing';

import { YamlSenderComponent } from './yaml-sender.component';

describe('YamlSenderComponent', () => {
  let component: YamlSenderComponent;
  let fixture: ComponentFixture<YamlSenderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [YamlSenderComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(YamlSenderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
