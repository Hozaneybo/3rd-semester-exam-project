import {ChangeDetectorRef, Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {TeacherService} from "../../../services/teacher.service";
import {CourseView, LessonView} from "../../../../shared/Models/CourseModel";
import {ResponseDto} from "../../../../shared/Models/LoginModels";

@Component({
  selector: 'app-course-details',
  templateUrl: './course-details.component.html',
  styleUrls: ['./course-details.component.scss'],
})
export class CourseDetailsComponent implements OnInit {
  course: CourseView | undefined;

  constructor(
      private teacherService: TeacherService,
      private route: ActivatedRoute,
      private router: Router,
      private changeDetectorRef: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      const courseId = params['id'];
      this.teacherService.getCourseById(courseId).subscribe({
        next: (response: ResponseDto<CourseView>) => {
          if (response.responseData) {
            this.course = response.responseData;
          } else {

          }
        },
        error: (error) => {
        }
      });
    });

  }

  getVideoThumbnail(videoUrl: string): string {
    return 'path_to_video_thumbnail';
  }

  navigateToCreateLesson() {
    if (this.course && this.course.id) {
      this.router.navigate(['/teacher/create-lesson', this.course.id]);
    } else {
      // Handle error: course ID is not available
    }
  }

  onLessonSelected(event: CustomEvent) {
    console.log("Accordion changed, event detail:", event.detail);
    const lessonId = event.detail.value; // The value property contains the lesson ID
    if (this.course?.id) {
      this.teacherService.getLessonById(this.course.id, lessonId).subscribe({
        next: (response: ResponseDto<LessonView>) => {
          // Safely assert that this.course is defined, since we checked it above
          const lessonIndex = this.course!.lessons.findIndex(l => l.id === lessonId);
          if (lessonIndex >= 0 && response.responseData) {
            // We can also safely assert that lessons is non-null/non-undefined
            this.course!.lessons[lessonIndex] = response.responseData;
            this.changeDetectorRef.detectChanges();
          }
        },
        error: (error) => {
          // Handle error
        }
      });
    }
  }



}
