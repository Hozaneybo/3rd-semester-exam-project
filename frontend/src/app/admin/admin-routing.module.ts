import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {AdminDashboardComponent} from "./admin-dashboard/admin-dashboard.component";
import {AllUsersComponent} from "./components/user/all-users/all-users.component";
import {UpdateUserComponent} from "./components/user/update-user/update-user.component";
import {UserDetailsComponent} from "./components/user/user-details/user-details.component";
import {UsersByRoleComponent} from "./components/user/user-by-role/users-by-role.component";
import {AllCoursesComponent} from "./components/course/all-courses/all-courses.component";
import {CreateCourseComponent} from "./components/course/create-course/create-course.component";
import {UpdateCourseComponent} from "./components/course/update-course/update-course.component";
import {CourseLessonsComponent} from "./components/lesson/course-lessons/course-lessons.component";
import {AdminPageComponent} from "./admin-page/admin-page.component";
import {LessonDetailsComponent} from "./components/lesson/lessons-details/lesson-details.component";
import {UpdateLessonComponent} from "./components/lesson/update-lesson/update-lesson.component";

const routes: Routes = [

  {
    path:'',
    component: AdminPageComponent,

    children: [
      { path: 'dashboard',
        component: AdminDashboardComponent },
      {
        path: 'users',
        component: AllUsersComponent
      },
      {
        path: 'update-user/:id',
        component:UpdateUserComponent
      },
      {
        path:'user-details/:id',
        component: UserDetailsComponent
      },
      {
        path:'users-by-role/:role',
        component: UsersByRoleComponent
      },
      {
        path: 'courses',
        component : AllCoursesComponent
      },
      {
        path: 'create-course',
        component: CreateCourseComponent
      },
      {
        path:'course/update/:id',
        component: UpdateCourseComponent
      },
      {
        path: 'courses/:courseId',
        component: CourseLessonsComponent
      },
      { path: 'courses/:courseId/lessons/:lessonId',
        component: LessonDetailsComponent
      },
      {
        path: 'courses/:courseId/update-lesson/:id',
        component: UpdateLessonComponent
      }
    ]
  },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
