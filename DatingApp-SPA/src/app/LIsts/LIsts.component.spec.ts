/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { LIstsComponent } from './LIsts.component';

describe('LIstsComponent', () => {
  let component: LIstsComponent;
  let fixture: ComponentFixture<LIstsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LIstsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LIstsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
