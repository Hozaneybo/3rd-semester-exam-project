import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {ResponseDto, User} from "../../shared/Models/LoginModels";
import {
  AllCoursesView,
  CourseView,
  CreateLessonCommand,
  LessonView,
  UpdateLessonCommand
} from "../../shared/Models/CourseModel";
import {SearchResultDto} from "../../shared/Models/SearchTerm";
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TeacherService {
  private baseUrl = environment.apiUrl + '/api/teacher';

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

  updateLesson(lessonId: number, lesson: UpdateLessonCommand): Observable<ResponseDto<any>> {
    return this.http.put<ResponseDto<any>>(`${this.baseUrl}/lessons/update/${lessonId}`, lesson, { withCredentials: true });
  }

  deleteLesson(lessonId: number): Observable<ResponseDto<any>> {
    return this.http.delete<ResponseDto<any>>(`${this.baseUrl}/lessons/delete/${lessonId}`, { withCredentials: true });
  }

  getUsersByRole(role: string): Observable<ResponseDto<User[]>> {
    return this.http.get<ResponseDto<User[]>>(`${this.baseUrl}/users/role`, {
      params: { role },
      withCredentials: true
    });
  }

  search(searchTerm: string): Observable<ResponseDto<SearchResultDto[]>> {
    return this.http.get<ResponseDto<SearchResultDto[]>>(`${this.baseUrl}/search`, {
      params: { searchTerm },
      withCredentials: true
    });
  }
}
