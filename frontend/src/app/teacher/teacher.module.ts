import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TeacherRoutingModule } from './teacher-routing.module';
import {TeacherDashBoardComponent} from "./teacher-dashboard/TeacherDashBoard.component";
import {GetAllCoursesComponent} from "./components/course-management/get-all-courses/get-all-courses.component";
import {CourseDetailsComponent} from "./components/course-management/course-details/course-details.component";
import {IonicModule} from "@ionic/angular";
import {CreateLessonComponent} from "./components/lesson-management/create-lesson/create-lesson.component";
import {QuillModule} from "ngx-quill";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";


@NgModule({
  declarations: [
    TeacherDashBoardComponent,
    GetAllCoursesComponent,
    CourseDetailsComponent,
    CreateLessonComponent,
  ],
  imports: [
    CommonModule,
    TeacherRoutingModule,
    IonicModule,
    QuillModule.forRoot(),
    FormsModule,
    ReactiveFormsModule
  ]
})
export class TeacherModule { }
