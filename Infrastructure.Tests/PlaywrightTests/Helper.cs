using System;
using Dapper;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Npgsql;
using Service.PasswordService;

namespace Infrastructure.Tests.PlaywrightTests;

public static class Helper
{
    public static readonly Uri Uri;
    public static readonly string ProperlyFormattedConnectionString;
    public static readonly NpgsqlDataSource DataSource;
    
    public static int? AdminUserId { get; private set; }
    public static int? RegularUserId { get; private set; }

    static Helper()
    {
        string rawConnectionString;
        string envVarKeyName = "pgconn";

        rawConnectionString = Environment.GetEnvironmentVariable(envVarKeyName)!;
        if (rawConnectionString == null)
        {
            throw new Exception($"YOUR CONN STRING PGCONN IS EMPTY. {envVarKeyName}.");
        }

        try
        {
            Uri = new Uri(rawConnectionString);
            ProperlyFormattedConnectionString = string.Format(
                "Server={0};Database={1};User Id={2};Password={3};Port={4};Pooling=true;MaxPoolSize=3",
                Uri.Host,
                Uri.AbsolutePath.Trim('/'),
                Uri.UserInfo.Split(':')[0],
                Uri.UserInfo.Split(':')[1],
                Uri.Port > 0 ? Uri.Port : 5432);
            DataSource =
                new NpgsqlDataSourceBuilder(ProperlyFormattedConnectionString).Build();
            DataSource.OpenConnection().Close();
        }
        catch (Exception e)
        {
            throw new Exception($@"
Your connection string is found, but could not be used. Are you sure you correctly inserted
the connection-string to Postgres?", e);
        }
    }
    
    public static void TriggerBuild()
    {
        using (var conn = DataSource.OpenConnection())
        {
            try
            {
                conn.Execute(BuildScriptIfNotExists);
            }
            catch (Exception e)
            {
                throw new Exception("THERE WAS AN ERROR REBUILDING THE DATABASE.", e);
            }
        }
    }
    
    
    public static void CreateAndDeleteAdminUser(bool create, bool delete)
    {
        if (create)
        {
            try
            {
                // Create Admin User
                var adminUser = Create("Test Admin", "test@admin.com", "Admin Photo", "Admin");
                InsertAdminPasswordHash(adminUser.Id, "TTTTTTTT");
                AdminUserId = adminUser.Id;

                // Create Regular User
                var regularUser = Create("PlayWright", "playwright@example.com", "playwright_photo.jpg", "Student");
                InsertAdminPasswordHash(regularUser.Id, "pppppppp"); // Use a suitable password for the regular user
                RegularUserId = regularUser.Id;
            }
            catch (Exception e)
            {
                throw new Exception("There was an error creating the users.", e);
            }
        }
        else if (delete && AdminUserId.HasValue)
        {
            try
            {
                if (AdminUserId.HasValue)
                {
                    DeleteUser(AdminUserId.Value);
                    AdminUserId = null;
                }

                // Delete Regular User if it exists
                if (RegularUserId.HasValue)
                {
                    DeleteUser(RegularUserId.Value);
                    RegularUserId = null;
                }
            }
            catch (Exception e)
            {
                throw new Exception($"There was an error deleting the admin user with ID {AdminUserId}.", e);
            }
        }
    }
    
    
    public static User Create(string fullName, string email, string avatarUrl, string role)
    {
        const string sql = @"
INSERT INTO learning_platform.users (full_name, email, avatar_url, role)
VALUES (@FullName, @Email, @AvatarUrl, @Role) RETURNING
    id, full_name, email, avatar_url, role;
";
        using var connection = DataSource.OpenConnection();
        return connection.QueryFirst<User>(sql, new 
        { 
            FullName = fullName, 
            Email = email, 
            AvatarUrl = avatarUrl, 
            Role = role 
        });
    }


    
    public static void DeleteUser(int id)
    {
        const string sql = @"DELETE FROM learning_platform.users WHERE id = @Id;";
        using (var connection = DataSource.OpenConnection())
            try
            {
                connection.Execute(sql, new { Id = id });
            }
            catch (NpgsqlException ex)
            {
                throw new InvalidOperationException("An error occurred while deleting the user.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while deleting the user.", ex);
            }
    }


    public static void InsertAdminPasswordHash(int id, string password)
    {
        var hashAlgorithm = PasswordHashAlgorithm.Create();
        var salt = hashAlgorithm.GenerateSalt();
        var hash = hashAlgorithm.HashPassword(password, salt);
        var passwordHashRepository = new PasswordHashRepository(DataSource);
        try
        {
            passwordHashRepository.Create(id, hash, salt, hashAlgorithm.GetName());
        }
        catch (Exception e)
        {
            throw new Exception("There was an error creating the admin password hash.", e);
        }
    }
    
    public static void DeleteUserByEmail(string email)
    {
        const string sql = @"DELETE FROM learning_platform.users WHERE email = @Email;";
        using var connection = DataSource.OpenConnection();
        try
        {
            connection.Execute(sql, new { Email = email });
        }
        catch (NpgsqlException ex)
        {
            throw new InvalidOperationException($"An error occurred while deleting the user with email {email}.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"An unexpected error occurred while deleting the user with email {email}.", ex);
        }
    }
    
    public static void DeleteCourseByTitle(string title)
    {
        const string sql = @"DELETE FROM learning_platform.courses WHERE title = @Title;";
        using var connection = DataSource.OpenConnection();
        try
        {
            connection.Execute(sql, new { Title = title });
        }
        catch (NpgsqlException ex)
        {
            throw new InvalidOperationException($"An error occurred while deleting the course with title {title}.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"An unexpected error occurred while deleting the course with title {title}.", ex);
        }
    }



    public static string BuildScriptIfNotExists = @"
CREATE SCHEMA IF NOT EXISTS learning_platform;


CREATE TABLE IF NOT EXISTS learning_platform.users (
    id SERIAL PRIMARY KEY,
        full_name VARCHAR(50) NOT NULL,
        email VARCHAR(50) NOT NULL UNIQUE,
        avatar_url VARCHAR(100),
        role VARCHAR(12) DEFAULT 'Student'::character varying,
        email_verification_token VARCHAR(255),
        email_token_expires_at TIMESTAMP,
        password_reset_token VARCHAR(255),
        password_reset_token_expires_at TIMESTAMP,
        email_verified BOOLEAN DEFAULT false
);


CREATE TABLE IF NOT EXISTS learning_platform.password_hash (
    user_id INTEGER,
    hash VARCHAR(350) NOT NULL,
    salt VARCHAR(180) NOT NULL,
    algorithm VARCHAR(12) NOT NULL,
    FOREIGN KEY (user_id) REFERENCES learning_platform.users (id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS learning_platform.courses
(
    id          SERIAL PRIMARY KEY,
    title       VARCHAR(255) NOT NULL,
    description TEXT NOT NULL,
    course_img_url VARCHAR(255)
);

CREATE TABLE IF NOT EXISTS learning_platform.lessons
(
    id         SERIAL PRIMARY KEY,
    title      VARCHAR(255) NOT NULL,
    content    TEXT NOT NULL,
    course_id  INTEGER NOT NULL,
    FOREIGN KEY (course_id) REFERENCES learning_platform.courses (id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS learning_platform.lesson_pictures
(
    id         SERIAL PRIMARY KEY,
    img_url    VARCHAR(255) NOT NULL,
    lesson_id  INTEGER NOT NULL,
    FOREIGN KEY (lesson_id) REFERENCES learning_platform.lessons (id) ON DELETE CASCADE,
    UNIQUE (lesson_id, img_url)
);

CREATE TABLE IF NOT EXISTS learning_platform.lesson_videos
(
    id         SERIAL PRIMARY KEY,
    video_url  VARCHAR(255) NOT NULL,
    lesson_id  INTEGER NOT NULL,
    FOREIGN KEY (lesson_id) REFERENCES learning_platform.lessons (id) ON DELETE CASCADE,
    UNIQUE (lesson_id, video_url)
);

 ";
    
}