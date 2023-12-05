export interface AllCoursesView{
  id: number,
  title: string,
  description: string,
  courseImgUrl: string
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
  courseId: number;
  pictureUrls?: string[];
  videoUrls?: string[];
}
