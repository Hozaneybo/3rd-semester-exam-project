import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {TeacherDashBoardComponent} from "./teacher-dashboard/TeacherDashBoard.component";
import {GetAllCoursesComponent} from "./components/course-management/get-all-courses/get-all-courses.component";
import {CourseDetailsComponent} from "./components/course-management/course-details/course-details.component";
import {CreateLessonComponent} from "./components/lesson-management/create-lesson/create-lesson.component";
import {LessonDetailsComponent} from "./components/lesson-management/lesson-details/lesson-details.component";
import {UpdateLessonComponent} from "./components/lesson-management/update-lesson/update-lesson.component";
import {TeacherLayoutComponent} from "./teacher-layout/teacher-layout.component";
import {RoleGuard} from "../shared/guards/role.guard";

const routes: Routes = [
  {
    path: '',
    component : TeacherLayoutComponent,
    canActivate: [RoleGuard],
    data: { expectedRole: 'Teacher' },
    children: [
      { path: 'dashboard', component: TeacherDashBoardComponent},
      { path: 'all-courses', component: GetAllCoursesComponent },
      { path: 'course-details/:id', component: CourseDetailsComponent },
      { path: 'create-lesson/:id', component: CreateLessonComponent},
      { path: 'courses/:courseId/lessons/:lessonId', component: LessonDetailsComponent },
      { path: 'courses/:courseId/update-lesson/:id', component: UpdateLessonComponent }
    ]
  },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TeacherRoutingModule { }
