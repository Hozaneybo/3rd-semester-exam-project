import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {TeacherDashBoardComponent} from "./teacher-dashboard/TeacherDashBoard.component";
import {GetAllCoursesComponent} from "./components/course-management/get-all-courses/get-all-courses.component";
import {CourseDetailsComponent} from "./components/course-management/course-details/course-details.component";
import {CreateLessonComponent} from "./components/lesson-management/create-lesson/create-lesson.component";

const routes: Routes = [
  {
    path: '',
    component : TeacherDashBoardComponent
  },
  { path: 'all-courses', component: GetAllCoursesComponent },
  { path: 'course-details/:id', component: CourseDetailsComponent },
  { path: 'create-lesson/:id', component: CreateLessonComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TeacherRoutingModule { }
