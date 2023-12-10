import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminRoutingModule } from './admin-routing.module';
import {AdminDashboardComponent} from "./admin-dashboard/admin-dashboard.component";
import {AllUsersComponent} from "./components/user/all-users/all-users.component";
import {UpdateUserComponent} from "./components/user/update-user/update-user.component";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {UserDetailsComponent} from "./components/user/user-details/user-details.component";
import {UsersByRoleComponent} from "./components/user/user-by-role/users-by-role.component";
import {IonicModule} from "@ionic/angular";
import {AllCoursesComponent} from "./components/course/all-courses/all-courses.component";
import {CreateCourseComponent} from "./components/course/create-course/create-course.component";
import {UpdateCourseComponent} from "./components/course/update-course/update-course.component";
import {AdminPageComponent} from "./admin-page/admin-page.component";
import {UpdateLessonComponent} from "./components/lesson/update-lesson/update-lesson.component";
import {QuillEditorComponent} from "ngx-quill";
import {MyProfileComponent} from "./components/my-profile/my-profile.component";
import {EditProfileComponent} from "./edit-profile/edit-profile.component";
import {CourseForLessonComponent} from "./components/lesson/course-for-lesson/course-for-lesson.component";
import {LessonDetailsComponent} from "./components/lesson/lesson-details/lesson-details.component";
import {LessonsDetailsComponent} from "./components/lesson/lessons-details/lessons-details.component";
import {SharedModule} from "../shared/shared.module";


@NgModule({
  declarations: [
    AdminDashboardComponent,
    AllUsersComponent,
    UpdateUserComponent,
    UserDetailsComponent,
    UsersByRoleComponent,
    AllCoursesComponent,
    CreateCourseComponent,
    UpdateCourseComponent,
    AdminPageComponent,
    UpdateLessonComponent,
    MyProfileComponent,
    EditProfileComponent,
    CourseForLessonComponent,
    LessonDetailsComponent,
    LessonsDetailsComponent

  ],
  imports: [
    CommonModule,
    AdminRoutingModule,
    ReactiveFormsModule,
    IonicModule,
    FormsModule,
    QuillEditorComponent,
    SharedModule
  ]
})
export class AdminModule { }
