export interface LessonCreate {
  title: string;
  content: string;
  courseId: number;
  images: File[] | null;
  videos: File[] | null;
}


