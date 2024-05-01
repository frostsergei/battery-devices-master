import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalInstructionsComponent } from './modal-instructions.component';

describe('ModalInstructionsComponent', () => {
  let component: ModalInstructionsComponent;
  let fixture: ComponentFixture<ModalInstructionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ModalInstructionsComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(ModalInstructionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
