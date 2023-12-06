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
import {CourseLessonsComponent} from "./components/lesson/course-lessons/course-lessons.component";
import {AdminPageComponent} from "./admin-page/admin-page.component";


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
    CourseLessonsComponent,
    AdminPageComponent

  ],
  imports: [
    CommonModule,
    AdminRoutingModule,
    ReactiveFormsModule,
    IonicModule,
    FormsModule
  ]
})
export class AdminModule { }
