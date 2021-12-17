import { Component, OnInit } from '@angular/core';
import { DaysService, RegistrationService } from '../../services';
import { DayModel } from '../../models';
import { RegistrationModel } from 'src/app/models/registration.model';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {

  status = {
    selectedDate: '',     //üres: még nem vásztott dátumot
    registrationSucceeded: false
  }

  days: DayModel[] = [];
  errorMessage = '';
  registration = new RegistrationModel();

  constructor(
    private dayService: DaysService,
    private registrationService: RegistrationService) { }

  ngOnInit(): void {
    this.dayService.getDays().subscribe(
      result => this.days = result,
      error => console.log(error)
    )
  }

  regisztracio(day: DayModel) {    
    this.status.selectedDate = day.date.toLocaleString().split('T')[0];

    this.registration = new RegistrationModel();
    this.registration.day = day;
    this.registration.password = '-';
    this.registration.id = 0;
  }

  parentNameChecked(event: any) {
    this.registration.parent1Checked = event.target.checked;
    this.registration.parentName1 = '';
  }

  studentsCountChanged() {
    switch (Number(this.registration.numberOfStudents)) {
      case 0:
        this.registration.studentName1 = '';
        this.registration.studentName2 = '';
        this.registration.studentName3 = '';
        this.registration.studentName4 = '';
        break;
      case 1:
        this.registration.studentName2 = '';
        this.registration.studentName3 = '';
        this.registration.studentName4 = '';
        break;
      case 2:
        this.registration.studentName3 = '';
        this.registration.studentName4 = '';
        break;
      case 3:
        this.registration.studentName4 = '';
        break;    
      default:
        break;
    }
  }

  send() {
    if (this.checkData()) {
      this.registrationService.send(this.registration).subscribe(
        result => {
          this.status.registrationSucceeded = true;
        },
        err => {
          console.log(err);
          this.errorMessage = err.message;
        }

      )
    }
  }

  checkData(): boolean {
    this.errorMessage = '';
    if (!this.registration.email) {
      this.errorMessage = 'Kérem adja meg az e-mail címét';
      return false;
    }
    if (this.registration.parent1Checked && !this.registration.parentName1){
      this.errorMessage = 'Kérem adja meg a kísérő nevét';
      return false;
    }
    if (this.registration.numberOfStudents > 0 && !this.registration.studentName1) {
      this.errorMessage = 'Kérem adja meg minden tanuló nevét';
      return false;
    }
    if (this.registration.numberOfStudents > 1 && !this.registration.studentName2) {
      this.errorMessage = 'Kérem adja meg minden tanuló nevét';
      return false;
    }
    if (this.registration.numberOfStudents > 2 && !this.registration.studentName3) {
      this.errorMessage = 'Kérem adja meg minden tanuló nevét';
      return false;
    }
    if (this.registration.numberOfStudents > 4 && !this.registration.studentName4) {
      this.errorMessage = 'Kérem adja meg minden tanuló nevét';
      return false;
    }
    return true;
  }
}
