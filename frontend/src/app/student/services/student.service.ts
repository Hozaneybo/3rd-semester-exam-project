import { Injectable } from '@angular/core';
import {environment} from "../../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {ResponseDto} from "../../shared/Models/LoginModels";
import {AllCoursesView, CourseView, LessonView} from "../../shared/Models/CourseModel";

@Injectable({
  providedIn: 'root'
})
export class StudentService {

  private baseUrl = environment.apiEndpoint + 'api/student';

  constructor(private http: HttpClient) {
  }


  getAllCourses(): Observable<ResponseDto<AllCoursesView[]>> {
    return this.http.get<ResponseDto<AllCoursesView[]>>(`${this.baseUrl}/courses`, {withCredentials: true});
  }

  getCourseById(courseId: string): Observable<ResponseDto<CourseView>> {
    return this.http.get<ResponseDto<CourseView>>(`${this.baseUrl}/courses/${courseId}`, {withCredentials: true});
  }

  getLessonById(courseId: number, lessonId: number): Observable<ResponseDto<LessonView>> {
    return this.http.get<ResponseDto<LessonView>>(`${this.baseUrl}/courses/${courseId}/lessons/${lessonId}`, { withCredentials: true });
  }
}
