import { DayModel } from "./day.model";

export class RegistrationModel {
    id: number | null = null;
    day: DayModel | null = null;
    email: string = '';
    password: string = '';
    parentName1: string = '';
    parentName2: string = '';
    studentName1: string = '';
    studentName2: string = '';
    studentName3: string = '';
    studentName4: string = '';

    parent1Checked = false;
    numberOfStudents = 1;
}