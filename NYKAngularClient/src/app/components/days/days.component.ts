import { Component, OnInit } from '@angular/core';
import { DayModel } from 'src/app/models/day.model';

import { DaysService, ConfirmService } from '../../services';

@Component({
  selector: 'app-days',
  templateUrl: './days.component.html',
  styleUrls: ['./days.component.css']
})
export class DaysComponent implements OnInit {

  days: DayModel[] = [];
  errorText = '';
  status = {
    isNew: false,
    new: {
      date: '',
      maxVisitors: 192
    },
    isModify: false,
    modify: new DayModel()
  }

  constructor(
    private daysService: DaysService,
    private confirmService: ConfirmService) { }

  ngOnInit(): void {
    this.daysService.getDays().subscribe(
      result => this.days = result,
      error => {
        console.log(error);
        this.errorText = error.message;
      }        
    );
  }

  new(): void {
    this.status.isNew = true;
    this.status.new.date = '';
    this.status.new.maxVisitors = 192;
  }

  saveNew(): void {
    this.errorText = '';
    if (isNaN(new Date(this.status.new.date).getTime())) {
      this.errorText = 'Hibás dátum'
      return;
    }
    if (isNaN(Number(this.status.new.maxVisitors)))
    {
      this.errorText = 'Hibás kapacitás adat'
      return;
    }

    const value: DayModel = {
      id: 0,
      date: new Date(this.status.new.date), 
      maxVisitors: Number(this.status.new.maxVisitors)
    };
    this.daysService.newDay(value).subscribe(
      result => {
        this.days.unshift(result);
        this.status.isNew = false;
      },
      error => {
        console.log(error);
        this.errorText = error.message;
      }        
    )
  }

  deleteDay(day: DayModel): void {
    this.confirmService.confirm('Biztos szeretné törölni a napot?').subscribe(
      result => {
        if( result ) {
          this.daysService.deleteDay(day).subscribe(
            response => {
              const index = this.days.indexOf(day);
              this.days.splice(index, 1);
            },
            error => {
              console.log(error)
              this.errorText = error.message;
            }
          );
        }
      },
      error => console.log(error)
    )
  }

  modify(day: DayModel): void {
    this.status.isModify = true;
    this.status.modify = JSON.parse( JSON.stringify(day) );
    this.status.modify.date = new Date(day.date).toISOString().split('T')[0]
    console.log(this.status.modify);
  }

  cancelModify(): void {
    this.status.isModify = false;
    this.status.modify = new DayModel();
  }

  saveModify(): void {
    console.log(this.status.modify.date);
    this.errorText = '';
    if (isNaN(new Date(this.status.modify.date).getTime())) {
      this.errorText = 'Hibás dátum'
      return;
    }
    if (isNaN(Number(this.status.modify.maxVisitors)))
    {
      this.errorText = 'Hibás kapacitás adat'
      return;
    }

    const value: DayModel = {
      id: this.status.modify.id,
      date: new Date(this.status.modify.date), 
      maxVisitors: Number(this.status.modify.maxVisitors)
    };
    this.daysService.modifyDay(value).subscribe(
      result => {
        const day = this.days.filter(d => d.id == result.id)[0];
        const index = this.days.indexOf(day);
        this.days.splice(index, 1);
        this.days.splice( index, 0, result );
        this.cancelModify();
      },
      error => {
        console.log(error);
        this.errorText = error.message;
      }        
    )
  }    

}
