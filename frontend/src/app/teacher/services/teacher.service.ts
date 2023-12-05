import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {ResponseDto} from "../../shared/Models/LoginModels";
import {AllCoursesView, CourseView, CreateLessonCommand, LessonView} from "../../shared/Models/CourseModel";
import {environment} from "../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class TeacherService {
  private baseUrl = environment.apiEndpoint + 'api/teacher';

  constructor(private http: HttpClient) {}


  getAllCourses(): Observable<ResponseDto<AllCoursesView[]>> {
    return this.http.get<ResponseDto<AllCoursesView[]>>(`${this.baseUrl}/courses`, { withCredentials: true });
  }

  getCourseById(courseId: string): Observable<ResponseDto<CourseView>> {
    return this.http.get<ResponseDto<CourseView>>(`${this.baseUrl}/courses/${courseId}`, { withCredentials: true });
  }

  createLesson(courseId: string, lesson: CreateLessonCommand): Observable<ResponseDto<any>> {
    return this.http.post<ResponseDto<any>>(`${this.baseUrl}/courses/lesson/create`, lesson, { withCredentials: true });
  }

  getLessonById(courseId: number, lessonId: number): Observable<ResponseDto<LessonView>> {
    return this.http.get<ResponseDto<LessonView>>(`${this.baseUrl}/courses/${courseId}/lessons/${lessonId}`, { withCredentials: true });
  }


}
