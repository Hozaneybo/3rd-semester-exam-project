import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { StudentRoutingModule } from './student-routing.module';
import {StudentDashboardComponent} from "./student-dashboard/student-dashboard.component";
import {GetAllCoursesComponent} from "./components/get-all-courses/get-all-courses.component";
import {CourseViewComponent} from "./components/course-view/course-view.component";
import {LessonViewComponent} from "./components/lesson-view/lesson-view.component";
import {ShowUsersByRoleComponent} from "./components/show-users-by-role/show-users-by-role.component";
import {IonicModule} from "@ionic/angular";


@NgModule({
  declarations: [
    StudentDashboardComponent,
    GetAllCoursesComponent,
    CourseViewComponent,
    LessonViewComponent,
    ShowUsersByRoleComponent

  ],
  imports: [
    CommonModule,
    StudentRoutingModule,
    IonicModule
  ]
})
export class StudentModule { }
