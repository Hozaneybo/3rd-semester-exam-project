import { Injectable } from '@angular/core';
import {environment} from "../../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {ResponseDto, User} from "../../shared/Models/LoginModels";
import {Observable} from "rxjs";
import {
  AllCoursesView,
  CourseView, CreateCourse,
  CreateLessonCommand,
  LessonView, UpdateCourseCommand,
  UpdateLessonCommand
} from "../../shared/Models/CourseModel";

@Injectable({
  providedIn: 'root'
})
export class AdminService {


  private readonly url = environment.apiEndpoint + 'api/admin/' ;


  constructor(private http: HttpClient) { }

  getAllUsers(): Observable<ResponseDto<User[]>> {
    return this.http.get<ResponseDto<User[]>>(this.url + 'users',  { withCredentials: true });
  }

  deleteUser(userId: number): Observable<any> {
    return this.http.delete(this.url + `users/delete/${userId}`, { withCredentials: true });
  }

  getUsersByRole(role: string): Observable<ResponseDto<User[]>> {
    return this.http.get<ResponseDto<User[]>>(`${this.url}users/role`, {
      params: { role }, // Make sure the backend expects this param
      withCredentials: true
    });
  }

  getUserById(userId: number): Observable<ResponseDto<User>> {
    return this.http.get<ResponseDto<User>>(`${this.url}users/${userId}`, { withCredentials: true });
  }

  updateUser(userId: number, userData: any): Observable<any> {
    return this.http.put(`${this.url}users/update/${userId}`, userData, { withCredentials: true });
  }
  getAllCourses():Observable<ResponseDto<AllCoursesView[]>>{
    return this.http.get<ResponseDto<AllCoursesView[]>>(this.url + 'courses', {withCredentials: true})
  }

  createCourse(courseData: CreateCourse): Observable<ResponseDto<CreateCourse>> {
    return this.http.post<ResponseDto<CreateCourse>>(this.url + 'courses/create', courseData, { withCredentials: true });
  }

  updateCourse(courseId: number, courseData: any): Observable<any> {
    return this.http.put<any>(`${this.url}courses/update/${courseId}`, courseData, { withCredentials: true });
  }


  getCourseById(courseId: number): Observable<ResponseDto<CourseView>> {
    return this.http.get<ResponseDto<CourseView>>(this.url + `courses/${courseId}`, { withCredentials: true });
  }

  deleteCourse(courseId: number): Observable<any> {
    return this.http.delete(this.url + `courses/delete/${courseId}`, { withCredentials: true });
  }

  updateLesson(courseId: number, lessonId: number, lessonData: UpdateLessonCommand): Observable<ResponseDto<LessonView>> {
    return this.http.put<ResponseDto<LessonView>>(this.url + `courses/${courseId}/lessons/update/${lessonId}`, lessonData, { withCredentials: true });
  }

  getLessonById(courseId: number, lessonId: number): Observable<ResponseDto<LessonView>> {
    return this.http.get<ResponseDto<LessonView>>(this.url + `courses/${courseId}/lessons/${lessonId}`, { withCredentials: true });
  }

  deleteLesson(courseId: number, lessonId: number): Observable<any> {
    return this.http.delete(this.url + `courses/${courseId}/lessons/delete/${lessonId}`, { withCredentials: true });
  }

}
