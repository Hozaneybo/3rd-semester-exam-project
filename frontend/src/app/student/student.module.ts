import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { StudentRoutingModule } from './student-routing.module';
import {StudentDashboardComponent} from "./student-dashboard/student-dashboard.component";
import {GetAllCoursesComponent} from "./components/get-all-courses/get-all-courses.component";
import {CourseViewComponent} from "./components/course-view/course-view.component";
import {LessonViewComponent} from "./components/lesson-view/lesson-view.component";
import {ShowUsersByRoleComponent} from "./components/show-users-by-role/show-users-by-role.component";
import {IonicModule} from "@ionic/angular";
import {StudentLayoutComponent} from "./student-layout/student-layout.component";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {MyProfileComponent} from "./components/my-profile/my-profile.component";
import {SharedModule} from "../shared/shared.module";
import {EditProfileComponent} from "./components/edit-profile/edit-profile.component";


@NgModule({
  declarations: [
    StudentDashboardComponent,
    GetAllCoursesComponent,
    CourseViewComponent,
    LessonViewComponent,
    ShowUsersByRoleComponent,
    StudentLayoutComponent,
    MyProfileComponent,
    EditProfileComponent

  ],
  imports: [
    CommonModule,
    StudentRoutingModule,
    IonicModule,
    FormsModule,
    SharedModule,
    ReactiveFormsModule
  ]
})
export class StudentModule { }
