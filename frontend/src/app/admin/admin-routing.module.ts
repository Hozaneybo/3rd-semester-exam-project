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
import {AdminPageComponent} from "./admin-page/admin-page.component";
import {UpdateLessonComponent} from "./components/lesson/update-lesson/update-lesson.component";
import {RoleGuard} from "../shared/guards/role.guard";
import {MyProfileComponent} from "./components/my-profile/my-profile.component";
import {EditProfileComponent} from "./components/edit-profile/edit-profile.component";
import {CourseForLessonComponent} from "./components/lesson/course-for-lesson/course-for-lesson.component";
import {LessonDetailsComponent} from "./components/lesson/lesson-details/lesson-details.component";
import {LessonsDetailsComponent} from "./components/lesson/lessons-details/lessons-details.component";

const routes: Routes = [

  {
    path:'',
    component: AdminPageComponent,
    canActivate: [RoleGuard],
    data: { expectedRole: 'Admin' },

    children: [
      {path: 'edit-profile',
      component: EditProfileComponent},

      { path: 'my-profile',
        component: MyProfileComponent },

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
        path:'users/role/:role',
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

      { path: 'courses/:courseId/lessons/:lessonId',
        component: LessonsDetailsComponent
      },
      {
        path: 'courses/:courseId/update-lesson/:id',
        component: UpdateLessonComponent
      },
      {
        path: 'course-lessons',
      component: CourseForLessonComponent
      },
      {path:'course-details/:id',
      component: LessonDetailsComponent}
    ]
  },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
