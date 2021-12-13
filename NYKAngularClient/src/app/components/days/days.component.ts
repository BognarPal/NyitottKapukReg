import { Component, OnInit } from '@angular/core';
import { DayModel } from 'src/app/models/day.model';

@Component({
  selector: 'app-days',
  templateUrl: './days.component.html',
  styleUrls: ['./days.component.css']
})
export class DaysComponent implements OnInit {

  days: DayModel[] = [];

  constructor() { }

  ngOnInit(): void {
    const d1 = new DayModel();
    d1.id = 1;
    d1.date = new Date('2022-10-20');
    d1.maxVisitors = 192
    this.days.push(d1);

    const d2 = new DayModel();
    d2.id = 1;
    d2.date = new Date('2022-10-22');
    d2.maxVisitors = 192
    this.days.push(d2);
  }

}
