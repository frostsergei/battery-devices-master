import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FormGenDemoComponent } from './form-gen-demo.component';

describe('FormGenDemoComponent', () => {
  let component: FormGenDemoComponent;
  let fixture: ComponentFixture<FormGenDemoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [FormGenDemoComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(FormGenDemoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
