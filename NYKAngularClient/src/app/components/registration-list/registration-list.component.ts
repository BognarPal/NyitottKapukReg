import { Component, OnInit } from '@angular/core';
import { RegistrationListModel } from '../../models';
import { RegistrationService } from '../../services';

@Component({
  selector: 'app-registration-list',
  templateUrl: './registration-list.component.html',
  styleUrls: ['./registration-list.component.css']
})
export class RegistrationListComponent implements OnInit {

  registrationList: RegistrationListModel[] = [];
  errorText = '';

  constructor(private registrationService: RegistrationService) { }

  ngOnInit(): void {
    
  }

  list(date: string): void {
    date = date.split('T')[0]
    this.registrationService.listByDate(new Date(date)).subscribe(
      result => {
        console.log(result);
      },
      error => {
        console.log(error);
        this.errorText = error.message;
      }
    )
  }

}
