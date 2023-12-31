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
import {LessonDetailsComponent} from "./components/lesson-management/lesson-details/lesson-details.component";
import {UpdateLessonComponent} from "./components/lesson-management/update-lesson/update-lesson.component";
import {TeacherLayoutComponent} from "./teacher-layout/teacher-layout.component";
import {ShowUsersByRoleComponent} from "./components/show-users-by-role/show-users-by-role.component";
import {MyProfileComponent} from "./components/my-profile/my-profile.component";
import {SharedModule} from "../shared/shared.module";
import {EditProfileComponent} from "./components/edit-profile/edit-profile.component";


@NgModule({
  declarations: [
    TeacherDashBoardComponent,
    GetAllCoursesComponent,
    CourseDetailsComponent,
    CreateLessonComponent,
    LessonDetailsComponent,
    UpdateLessonComponent,
    ShowUsersByRoleComponent,
    TeacherLayoutComponent,
    MyProfileComponent,
    EditProfileComponent

  ],
    imports: [
        CommonModule,
        TeacherRoutingModule,
        IonicModule,
        QuillModule.forRoot(),
        FormsModule,
        ReactiveFormsModule,
        SharedModule,

    ]
})
export class TeacherModule { }
