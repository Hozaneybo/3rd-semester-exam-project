import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {StudentDashboardComponent} from "./student-dashboard/student-dashboard.component";
import {GetAllCoursesComponent} from "./components/get-all-courses/get-all-courses.component";
import {CourseViewComponent} from "./components/course-view/course-view.component";
import {LessonViewComponent} from "./components/lesson-view/lesson-view.component";


const routes: Routes = [
  {
    path: 'dashboard',
    component : StudentDashboardComponent
  },
  { path: 'all-courses', component: GetAllCoursesComponent },
  { path: 'course-details/:id', component: CourseViewComponent },
  { path: 'courses/:courseId/lessons/:lessonId', component: LessonViewComponent },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StudentRoutingModule { }
