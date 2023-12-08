import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {StudentDashboardComponent} from "./student-dashboard/student-dashboard.component";
import {GetAllCoursesComponent} from "./components/get-all-courses/get-all-courses.component";
import {CourseViewComponent} from "./components/course-view/course-view.component";
import {LessonViewComponent} from "./components/lesson-view/lesson-view.component";
import {StudentLayoutComponent} from "./student-layout/student-layout.component";
import {RoleGuard} from "../shared/guards/role.guard";
import {ShowUsersByRoleComponent} from "./components/show-users-by-role/show-users-by-role.component";
import {MyProfileComponent} from "./components/my-profile/my-profile.component";


const routes: Routes = [
  {
    path: '',
    component : StudentLayoutComponent,
    canActivate: [RoleGuard],
    data: { expectedRole: 'Student' },
    children:
      [
        { path: 'my-profile', component: MyProfileComponent},
        { path: 'users/role/:role', component: ShowUsersByRoleComponent},
        { path: 'dashboard',component : StudentDashboardComponent },
        { path: 'all-courses', component: GetAllCoursesComponent },
        { path: 'course-details/:id', component: CourseViewComponent },
        { path: 'courses/:courseId/lessons/:lessonId', component: LessonViewComponent },
      ]
  }

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StudentRoutingModule { }
