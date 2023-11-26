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

    public async Task<IEnumerable<Lesson>> GetAllLessons()
    {
        const string sql = @"
SELECT id, title, content, course_id
FROM learning_platform.lessons;
";
        using var connection = _dataSource.OpenConnection();
        return await connection.QueryAsync<Lesson>(sql);
    }

    public async Task<Lesson> GetLessonById(int id)
    {
        const string lessonSql = @"
SELECT * FROM learning_platform.lessons
WHERE id = @Id;
";

        const string picturesSql = @"
SELECT * FROM learning_platform.lesson_pictures
WHERE lesson_id = @Id;
";

        const string videosSql = @"
SELECT * FROM learning_platform.lesson_videos
WHERE lesson_id = @Id;
";

        using var connection = _dataSource.OpenConnection();
        var lesson = await connection.QuerySingleOrDefaultAsync<Lesson>(lessonSql, new { Id = id });

        if (lesson != null)
        {
            var pictures = await connection.QueryAsync<LessonPicture>(picturesSql, new { Id = id });
            var videos = await connection.QueryAsync<LessonVideo>(videosSql, new { Id = id });

            lesson.ImgUrls = pictures.ToList();
            lesson.VideoUrls = videos.ToList();
        }

        return lesson;
    }


    public async Task<Lesson> AddLesson(string title, string content, int courseId, IEnumerable<string> pictureUrls, IEnumerable<string> videoUrls)
    {
        const string insertLessonSql = @"
INSERT INTO learning_platform.lessons (title, content, course_id)
VALUES (@Title, @Content, @CourseId)
RETURNING *;
";

        const string insertPictureSql = @"
INSERT INTO learning_platform.lesson_pictures (img_url, lesson_id)
VALUES (@ImgUrl, @LessonId);
";

        const string insertVideoSql = @"
INSERT INTO learning_platform.lesson_videos (video_url, lesson_id)
VALUES (@VideoUrl, @LessonId);
";

        using var connection = _dataSource.OpenConnection();
        using var transaction = connection.BeginTransaction();

        try
        {
            var lesson = await connection.QueryFirstAsync<Lesson>(insertLessonSql, new { Title = title, Content = content, CourseId = courseId }, transaction);

            foreach (var imgUrl in pictureUrls)
            {
                await connection.ExecuteAsync(insertPictureSql, new { ImgUrl = imgUrl, LessonId = lesson.Id }, transaction);
            }

            foreach (var videoUrl in videoUrls)
            {
                await connection.ExecuteAsync(insertVideoSql, new { VideoUrl = videoUrl, LessonId = lesson.Id }, transaction);
            }

            transaction.Commit();
            return lesson;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
    
    
    public async Task<Lesson> UpdateLesson(int id, string title, string content, int courseId, IEnumerable<string> pictureUrls, IEnumerable<string> videoUrls)
{
    using var connection = _dataSource.OpenConnection();
    using var transaction = connection.BeginTransaction();

    try
    {
        
        var updateLessonSql = @"
UPDATE learning_platform.lessons
SET title = @Title, content = @Content, course_id = @CourseId
WHERE id = @Id
RETURNING *;";

        var lesson = await connection.QueryFirstOrDefaultAsync<Lesson>(updateLessonSql, new { Id = id, Title = title, Content = content, CourseId = courseId }, transaction);

        if (lesson == null)
        {
            transaction.Rollback();
            return null;
        }
        
        var pictureUrlList = pictureUrls.ToList();
        var videoUrlList = videoUrls.ToList();
        
        var deletePicturesSql = @"
DELETE FROM learning_platform.lesson_pictures
WHERE lesson_id = @LessonId AND img_url != ALL(@ImgUrls);";

        var deleteVideosSql = @"
DELETE FROM learning_platform.lesson_videos
WHERE lesson_id = @LessonId AND video_url != ALL(@VideoUrls);";

        await connection.ExecuteAsync(deletePicturesSql, new { LessonId = id, ImgUrls = pictureUrlList }, transaction);
        await connection.ExecuteAsync(deleteVideosSql, new { LessonId = id, VideoUrls = videoUrlList }, transaction);
        
        var insertPictureSql = @"
INSERT INTO learning_platform.lesson_pictures (img_url, lesson_id)
VALUES (@ImgUrl, @LessonId)
ON CONFLICT (img_url, lesson_id) DO NOTHING;";

        var insertVideoSql = @"
INSERT INTO learning_platform.lesson_videos (video_url, lesson_id)
VALUES (@VideoUrl, @LessonId)
ON CONFLICT (video_url, lesson_id) DO NOTHING;";

        foreach (var imgUrl in pictureUrlList)
        {
            await connection.ExecuteAsync(insertPictureSql, new { ImgUrl = imgUrl, LessonId = id }, transaction);
        }

        foreach (var videoUrl in videoUrlList)
        {
            await connection.ExecuteAsync(insertVideoSql, new { VideoUrl = videoUrl, LessonId = id }, transaction);
        }

        transaction.Commit();
        return lesson;
    }
    catch
    {
        transaction.Rollback();
        throw;
    }
}



    public async Task DeleteLesson(int id)
    {
        const string sql = @"
DELETE FROM learning_platform.lessons
WHERE id = @Id;
";
        using var connection = _dataSource.OpenConnection();
        await connection.ExecuteAsync(sql, new { Id = id });
    }
}