using Dapper;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Npgsql;

namespace Infrastructure.Repositories;

public class LessonRepository : ILessonRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public LessonRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public IEnumerable<Lesson> GetAllLessons()
    { 
        try
        {
            const string sql = @"
SELECT id, title, content, course_id
FROM learning_platform.lessons;
";
            using var connection = _dataSource.OpenConnection();
            return connection.Query<Lesson>(sql);
        }
        catch (NpgsqlException ex)
        {
            throw new InvalidOperationException("An error occurred while retrieving lessons.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred while retrieving lessons.", ex);
        }
    }

    public Lesson GetLessonById(int courseId, int id)
    {
        try
        {
            const string lessonSql = $@"
SELECT 
    id as {nameof(Lesson.Id)},
    title as {nameof(Lesson.Title)},
    content as {nameof(Lesson.Content)},
    course_id as {nameof(Lesson.CourseId)}
    From learning_platform.lessons
WHERE id = @Id AND course_id = @CourseId;
";

            const string picturesSql = $@"
SELECT
    id as {nameof(LessonPicture.Id)},
    img_url as {nameof(LessonPicture.ImgUrl)},
    lesson_id as {nameof(LessonPicture.LessonId)}
    FROM learning_platform.lesson_pictures
WHERE lesson_id = @Id;
";

            const string videosSql = $@"
SELECT 
    id as {nameof(LessonVideo.Id)},
    video_url as {nameof(LessonVideo.VideoUrl)},
    lesson_id as {nameof(LessonVideo.LessonId)}
    FROM learning_platform.lesson_videos
WHERE lesson_id = @Id;
";

            using var connection = _dataSource.OpenConnection();
            var lesson = connection.QuerySingleOrDefault<Lesson>(lessonSql, new { Id = id, CourseId = courseId });

            if (lesson != null)
            {
                var pictures = connection.Query<LessonPicture>(picturesSql, new { Id = id });
                var videos = connection.Query<LessonVideo>(videosSql, new { Id = id });

                lesson.ImgUrls = pictures.ToList();
                lesson.VideoUrls = videos.ToList();
            }

            return lesson;

        }
        catch (NpgsqlException ex)
        {
            throw new InvalidOperationException("An error occurred while retrieving the lesson by ID.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred while retrieving the lesson by ID.", ex);
        }
        
    }


    public Lesson AddLesson(string title, string content, int courseId, IEnumerable<string> pictureUrls, IEnumerable<string> videoUrls)
    {
        const string insertLessonSql = $@"
INSERT INTO learning_platform.lessons (title, content, course_id)
VALUES (@Title, @Content, @CourseId)
RETURNING 
    id as {nameof(Lesson.Id)},
    title as {nameof(Lesson.Title)},
    content as {nameof(Lesson.Content)},
    course_id as {nameof(Lesson.CourseId)};";

        const string insertPictureSql = $@"
INSERT INTO learning_platform.lesson_pictures (img_url, lesson_id)
VALUES (@ImgUrl, @LessonId) 
RETURNING
    id as {nameof(LessonPicture.Id)},
    img_url as {nameof(LessonPicture.ImgUrl)},
    lesson_id as {nameof(LessonPicture.LessonId)};
";

        const string insertVideoSql = $@"
INSERT INTO learning_platform.lesson_videos (video_url, lesson_id)
VALUES (@VideoUrl, @LessonId) 
RETURNING 
    id as {nameof(LessonVideo.Id)},
    video_url as {nameof(LessonVideo.VideoUrl)},
    lesson_id as {nameof(LessonVideo.LessonId)};
";

        using var connection = _dataSource.OpenConnection();
        using var transaction = connection.BeginTransaction();

        try
        {
            var lesson = connection.QueryFirst<Lesson>(insertLessonSql, new { Title = title, Content = content, CourseId = courseId }, transaction);

            foreach (var imgUrl in pictureUrls)
            {
                connection.Execute(insertPictureSql, new { ImgUrl = imgUrl, LessonId = lesson.Id }, transaction);
            }

            foreach (var videoUrl in videoUrls)
            {
                connection.Execute(insertVideoSql, new { VideoUrl = videoUrl, LessonId = lesson.Id }, transaction);
            }

            transaction.Commit();
            return lesson;
        }
        catch (NpgsqlException ex)
        {
            transaction.Rollback();
            throw new InvalidOperationException("An error occurred while adding a new lesson.", ex);
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw new Exception("An unexpected error occurred while adding a new lesson.", ex);
        }
    }
    
    
    public Lesson UpdateLesson(int id, string title, string content, int courseId, IEnumerable<string> pictureUrls, IEnumerable<string> videoUrls)
{
    using var connection = _dataSource.OpenConnection();
    using var transaction = connection.BeginTransaction();

    try
    {
        var updateLessonSql = @"
UPDATE learning_platform.lessons
SET title = COALESCE(@Title, title), 
    content = COALESCE(@Content, content), 
    course_id = COALESCE(@CourseId, course_id)
WHERE id = @Id
RETURNING *;";

        var lesson = connection.QueryFirstOrDefault<Lesson>(updateLessonSql, new { Id = id, Title = title, Content = content, CourseId = courseId }, transaction);

        if (lesson == null)
        {
            transaction.Rollback();
            return null;
        }
        
        if (pictureUrls != null)
        {
            var pictureUrlList = pictureUrls.ToList();
            var deletePicturesSql = @"
DELETE FROM learning_platform.lesson_pictures
WHERE lesson_id = @LessonId AND img_url != ALL(@ImgUrls);";

            var insertPictureSql = @"
INSERT INTO learning_platform.lesson_pictures (img_url, lesson_id)
VALUES (@ImgUrl, @LessonId)
ON CONFLICT (img_url, lesson_id) DO NOTHING;";

            connection.Execute(deletePicturesSql, new { LessonId = id, ImgUrls = pictureUrlList }, transaction);
            
            foreach (var imgUrl in pictureUrlList)
            { 
                connection.Execute(insertPictureSql, new { ImgUrl = imgUrl, LessonId = id }, transaction);
            }
        }
        
        if (videoUrls != null)
        {
            var videoUrlList = videoUrls.ToList();
            var deleteVideosSql = @"
DELETE FROM learning_platform.lesson_videos
WHERE lesson_id = @LessonId AND video_url != ALL(@VideoUrls);";

            var insertVideoSql = @"
INSERT INTO learning_platform.lesson_videos (video_url, lesson_id)
VALUES (@VideoUrl, @LessonId)
ON CONFLICT (video_url, lesson_id) DO NOTHING;";

            connection.Execute(deleteVideosSql, new { LessonId = id, VideoUrls = videoUrlList }, transaction);
            
            foreach (var videoUrl in videoUrlList)
            { 
                connection.Execute(insertVideoSql, new { VideoUrl = videoUrl, LessonId = id }, transaction);
            }
        }

        transaction.Commit();
        return lesson;
    }
    catch (NpgsqlException ex)
    {
        transaction.Rollback();
        throw new InvalidOperationException("An error occurred while updating the lesson.", ex);
    }
    catch (Exception ex)
    {
        transaction.Rollback();
        throw new Exception("An unexpected error occurred while updating the lesson.", ex);
    }
}


    
    public void DeleteLesson(int id)
    {
        const string sql = @"
DELETE FROM learning_platform.lessons
WHERE id = @Id;
";
        using var connection = _dataSource.OpenConnection(); 
        try
        {
            connection.Execute(sql, new { Id = id });
        }
        catch (NpgsqlException ex)
        {
            throw new InvalidOperationException("An error occurred while deleting the lesson.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred while deleting the lesson.", ex);
        }
    }
}