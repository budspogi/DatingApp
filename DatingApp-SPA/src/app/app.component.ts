import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ValueComponent } from './value/value.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})


export class AppComponent {
  title = 'DatingApp-SPA';
}
