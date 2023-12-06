export interface AllCoursesView{
  id: number,
  title: string,
  description: string,
  courseImgUrl: string,
}
export interface CreateCourse{


  title: string,
  description: string,
  courseImgUrl: string,
}

export interface CourseView{
  id: number,
  title: string,
  description: string,
  courseImgUrl: string,
  lessons: LessonView[]
}

export interface LessonView{
  id: number,
  courseId: number,
  title: string,
  content: string,
  imgUrls: LessonPictureView[],
  videoUrls: LessonVideoView[]
}
export interface LessonPictureView{
  id: number,
  pictureUrl: string,
  lessonId: number,
}

export interface LessonVideoView{
  id: number,
  videoUrl: string,
  lessonId: number,
}

export interface CreateLessonCommand {
  title: string;
  content: string;
  courseId: number;
  pictureUrls?: string[];
  videoUrls?: string[];
}

export interface UpdateLessonCommand {
  id: number;
  title: string;
  content: string;
  courseId: number;
  pictureUrls?: string[];
  videoUrls?: string[];

}

export interface UpdateCourseCommand {
  id: number;
  title: string;
  description: string;
  courseImgUrl: string;
}
