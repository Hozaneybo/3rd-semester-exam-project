export interface AllCoursesView{
  id: number,
  title: string,
  description: string,
  courseImgUrl: string
}

export interface CourseView{

  title: string,
  description: string,
  courseImgUrl: string,
}

export interface LessonView{
  id: number,
  courseId: number,
  title: string,
  content: string,
  images: LessonPictureView[],
  videos: LessonVideoView[]
}
export interface LessonPictureView{
  id: number,
  picUrl: string,
  lessonId: number,
}

export interface LessonVideoView{
  id: number,
  vidUrl: string,
  lessonId: number,
}

export interface CreateLessonCommand {
  title: string;
  content: string;
  imgUrls: string[];
  videoUrls: string[];
  courseId: number;
}

export interface UpdateLessonCommand {
  id: number;
  title: string;
  content: string;
  imgUrls: string[];
  videoUrls: string[];
  courseId: number;
}

export interface UpdateCourseCommand {
  id: number;
  title: string;
  description: string;
  courseImgUrl: string;
}
