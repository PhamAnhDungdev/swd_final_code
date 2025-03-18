-- Create the database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'OnlineLearningIT')
BEGIN
    CREATE DATABASE OnlineLearningIT;
END
GO

USE OnlineLearningIT;
GO

-- Create User table first (needed as a foreign key for Course)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'User')
BEGIN
    CREATE TABLE [User] (
        user_id INT IDENTITY(1,1) PRIMARY KEY,
        username VARCHAR(50) NOT NULL,
        email VARCHAR(100) NOT NULL,
        password_hash VARCHAR(255) NOT NULL,
        full_name NVARCHAR(100) NOT NULL,
        role VARCHAR(20) NOT NULL, -- 'student', 'instructor', 'admin'
        created_at DATETIME DEFAULT GETDATE(),
        updated_at DATETIME DEFAULT GETDATE()
    );
END
GO

-- Create Course table with instructor_name instead of instructor_id
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Course')
BEGIN
    CREATE TABLE Course (
        course_id INT IDENTITY(1,1) PRIMARY KEY,
        title NVARCHAR(255) NOT NULL,
        description NTEXT,
        image_url VARCHAR(255),
        duration INT, -- in minutes or hours
        price DECIMAL(10, 2),
        level VARCHAR(50), -- beginner, intermediate, advanced
        category VARCHAR(100),
        instructor_name NVARCHAR(100), -- Changed from instructor_id INT to instructor_name NVARCHAR
        status VARCHAR(20) DEFAULT 'draft', -- 'active', 'draft', 'archived'
        created_at DATETIME DEFAULT GETDATE(),
        updated_at DATETIME DEFAULT GETDATE()
        -- Removed foreign key constraint to User table
    );
END
ELSE
BEGIN
    -- If the table already exists, alter it to change instructor_id to instructor_name
    IF EXISTS (SELECT * FROM sys.columns WHERE name = 'instructor_id' AND object_id = OBJECT_ID('Course'))
    BEGIN
        -- First drop the foreign key constraint if it exists
        DECLARE @constraintName NVARCHAR(100)
        SELECT @constraintName = name FROM sys.foreign_keys 
        WHERE parent_object_id = OBJECT_ID('Course') 
        AND referenced_object_id = OBJECT_ID('User')
        
        IF @constraintName IS NOT NULL
        BEGIN
            EXEC('ALTER TABLE Course DROP CONSTRAINT ' + @constraintName)
        END
        
        -- Then alter the column
        ALTER TABLE Course DROP COLUMN instructor_id
        ALTER TABLE Course ADD instructor_name NVARCHAR(100)
    END
END
GO

-- Create Lesson table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Lesson')
BEGIN
    CREATE TABLE Lesson (
        lesson_id INT IDENTITY(1,1) PRIMARY KEY,
        course_id INT NOT NULL,
        title NVARCHAR(255) NOT NULL,
        description NTEXT,
        content_type VARCHAR(20) NOT NULL, -- 'video', 'text', 'quiz', 'assignment'
        content_url VARCHAR(255),
        duration INT, -- in minutes
        order_number INT NOT NULL, -- for sorting lessons within a course
        is_free BIT DEFAULT 0, -- for preview lessons
        created_at DATETIME DEFAULT GETDATE(),
        updated_at DATETIME DEFAULT GETDATE(),
        
        FOREIGN KEY (course_id) REFERENCES Course(course_id) ON DELETE CASCADE
    );
END
GO

-- Create Certificate table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Certificate')
BEGIN
    CREATE TABLE Certificate (
        certificate_id INT IDENTITY(1,1) PRIMARY KEY,
        course_id INT NOT NULL,
        title NVARCHAR(255) NOT NULL,
        description NTEXT,
        template_url VARCHAR(255), -- URL to certificate template
        min_completion_percentage INT DEFAULT 100, -- minimum completion percentage required
        is_active BIT DEFAULT 1,
        created_at DATETIME DEFAULT GETDATE(),
        updated_at DATETIME DEFAULT GETDATE(),
        
        FOREIGN KEY (course_id) REFERENCES Course(course_id) ON DELETE CASCADE
    );
END
GO

-- Create UserCertificate table for tracking issued certificates
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserCertificate')
BEGIN
    CREATE TABLE UserCertificate (
        user_certificate_id INT IDENTITY(1,1) PRIMARY KEY,
        user_id INT NOT NULL,
        certificate_id INT NOT NULL,
        issue_date DATETIME DEFAULT GETDATE(),
        certificate_url VARCHAR(255), -- URL to generated certificate
        verification_code VARCHAR(50) UNIQUE,
        
        FOREIGN KEY (user_id) REFERENCES [User](user_id),
        FOREIGN KEY (certificate_id) REFERENCES Certificate(certificate_id)
    );
END
GO

-- Clear existing data to avoid conflicts when re-running the script
DELETE FROM UserCertificate;
DELETE FROM Certificate;
DELETE FROM Lesson;
DELETE FROM Course;
DELETE FROM [User];
GO

-- Reset identity columns
DBCC CHECKIDENT ('UserCertificate', RESEED, 0);
DBCC CHECKIDENT ('Certificate', RESEED, 0);
DBCC CHECKIDENT ('Lesson', RESEED, 0);
DBCC CHECKIDENT ('Course', RESEED, 0);
DBCC CHECKIDENT ('User', RESEED, 0);
GO

-- Insert sample data
-- Sample Users
INSERT INTO [User] (username, email, password_hash, full_name, role)
VALUES 
    ('john_instructor', 'john@example.com', 'hashed_password_123', N'John Smith', 'instructor'),
    ('mary_instructor', 'mary@example.com', 'hashed_password_456', N'Mary Johnson', 'instructor'),
    ('sam_student', 'sam@example.com', 'hashed_password_789', N'Sam Wilson', 'student'),
    ('lisa_student', 'lisa@example.com', 'hashed_password_abc', N'Lisa Brown', 'student'),
    ('admin_user', 'admin@example.com', 'hashed_password_xyz', N'Admin User', 'admin');
GO

-- Sample Courses - Using instructor_name instead of instructor_id
INSERT INTO Course (title, description, image_url, duration, price, level, category, instructor_name, status)
VALUES 
    (N'Introduction to SQL Server', N'Learn the basics of SQL Server database management and querying.', '/images/sql-basics.jpg', 600, 49.99, 'beginner', 'Databases', N'John Smith', 'active'),
    (N'Advanced JavaScript Programming', N'Master advanced JavaScript concepts and modern frameworks.', '/images/js-advanced.jpg', 1200, 79.99, 'advanced', 'Web Development', N'Mary Johnson', 'active'),
    (N'Python for Data Science', N'Use Python to analyze and visualize complex datasets.', '/images/python-data.jpg', 900, 59.99, 'intermediate', 'Data Science', N'John Smith', 'active'),
    (N'Mobile App Development with React Native', N'Build cross-platform mobile apps with React Native.', '/images/react-native.jpg', 1500, 89.99, 'intermediate', 'Mobile Development', N'Mary Johnson', 'draft');
GO

-- Sample Lessons
INSERT INTO Lesson (course_id, title, description, content_type, content_url, duration, order_number, is_free)
VALUES 
    -- SQL Server course lessons
    (1, N'Introduction to Databases', N'Understanding database concepts and terminology.', 'video', '/content/sql/intro.mp4', 15, 1, 1),
    (1, N'SQL Server Installation', N'Step-by-step guide to installing SQL Server.', 'text', '/content/sql/installation.html', 10, 2, 1),
    (1, N'Creating Your First Database', N'Learn to create and configure a new database.', 'video', '/content/sql/first-db.mp4', 20, 3, 0),
    (1, N'Basic SELECT Queries', N'Writing your first SELECT statements.', 'video', '/content/sql/select.mp4', 25, 4, 0),
    (1, N'SQL Server Quiz', N'Test your SQL Server knowledge.', 'quiz', '/content/sql/quiz1.json', 15, 5, 0),
    
    -- JavaScript course lessons
    (2, N'Modern JavaScript Features', N'Overview of ES6+ features.', 'video', '/content/js/modern.mp4', 30, 1, 1),
    (2, N'Advanced Functions and Closures', N'Mastering JavaScript functions.', 'video', '/content/js/functions.mp4', 45, 2, 0),
    (2, N'Asynchronous JavaScript', N'Working with Promises and async/await.', 'video', '/content/js/async.mp4', 40, 3, 0),
    (2, N'JavaScript Framework Comparison', N'React vs. Angular vs. Vue.', 'text', '/content/js/frameworks.html', 20, 4, 0),
    (2, N'Building a JavaScript Project', N'Hands-on coding assignment.', 'assignment', '/content/js/project.json', 120, 5, 0),
    
    -- Python course lessons
    (3, N'Python Environment Setup', N'Setting up your Python data science environment.', 'video', '/content/python/setup.mp4', 20, 1, 1),
    (3, N'Data Manipulation with Pandas', N'Working with DataFrames in Pandas.', 'video', '/content/python/pandas.mp4', 35, 2, 0),
    (3, N'Data Visualization with Matplotlib', N'Creating informative visualizations.', 'video', '/content/python/matplotlib.mp4', 30, 3, 0),
    
    -- React Native course lessons
    (4, N'React Native Fundamentals', N'Core concepts of React Native.', 'video', '/content/react-native/fundamentals.mp4', 25, 1, 1),
    (4, N'Setting Up Your Development Environment', N'Tools and configurations for React Native.', 'text', '/content/react-native/setup.html', 15, 2, 0);
GO

-- Sample Certificates
INSERT INTO Certificate (course_id, title, description, template_url, min_completion_percentage, is_active)
VALUES 
    (1, N'SQL Server Fundamentals', N'This certificate verifies completion of the SQL Server Fundamentals course.', '/templates/sql-cert.html', 85, 1),
    (2, N'Advanced JavaScript Mastery', N'This certificate verifies proficiency in advanced JavaScript concepts.', '/templates/js-cert.html', 90, 1),
    (3, N'Python Data Science Certification', N'This certificate confirms skills in Python for data analysis.', '/templates/python-cert.html', 80, 1),
    (4, N'React Native Developer Certificate', N'This certificate is currently in draft mode.', '/templates/react-native-cert.html', 85, 0);
GO

-- Sample UserCertificates with unique verification codes
INSERT INTO UserCertificate (user_id, certificate_id, certificate_url, verification_code)
VALUES 
    (3, 1, '/certificates/user3-sql.pdf', 'SQLCERT-3851-XYZ'),
    (4, 2, '/certificates/user4-js.pdf', 'JSCERT-4962-DEF'),
    (3, 3, '/certificates/user3-python.pdf', 'PYCERT-3275-GHI');
GO

-- Create stored procedure for updating a course
CREATE OR ALTER PROCEDURE UpdateCourse
    @CourseId INT,
    @Title NVARCHAR(255) = NULL,
    @Description NTEXT = NULL,
    @ImageUrl VARCHAR(255) = NULL,
    @Duration INT = NULL,
    @Price DECIMAL(10, 2) = NULL,
    @Level VARCHAR(50) = NULL,
    @Category VARCHAR(100) = NULL,
    @InstructorName NVARCHAR(100) = NULL, -- Changed from @InstructorId
    @Status VARCHAR(20) = NULL
AS
BEGIN
    UPDATE Course
    SET 
        title = ISNULL(@Title, title),
        description = ISNULL(@Description, description),
        image_url = ISNULL(@ImageUrl, image_url),
        duration = ISNULL(@Duration, duration),
        price = ISNULL(@Price, price),
        level = ISNULL(@Level, level),
        category = ISNULL(@Category, category),
        instructor_name = ISNULL(@InstructorName, instructor_name), -- Changed from instructor_id
        status = ISNULL(@Status, status),
        updated_at = GETDATE()
    WHERE course_id = @CourseId;
    
    SELECT 'Course updated successfully' AS Result;
END
GO

-- Create stored procedure for adding a certificate
CREATE OR ALTER PROCEDURE AddCertificate
    @CourseId INT,
    @Title NVARCHAR(255),
    @Description NTEXT = NULL,
    @TemplateUrl VARCHAR(255) = NULL,
    @MinCompletionPercentage INT = 100,
    @IsActive BIT = 1
AS
BEGIN
    INSERT INTO Certificate (
        course_id, 
        title, 
        description, 
        template_url, 
        min_completion_percentage, 
        is_active
    )
    VALUES (
        @CourseId,
        @Title,
        @Description,
        @TemplateUrl,
        @MinCompletionPercentage,
        @IsActive
    );
    
    SELECT 
        'Certificate added successfully' AS Result,
        SCOPE_IDENTITY() AS CertificateId;
END
GO

-- Create stored procedure for issuing a certificate to a user
CREATE OR ALTER PROCEDURE IssueCertificateToUser
    @UserId INT,
    @CertificateId INT,
    @CertificateUrl VARCHAR(255) = NULL
AS
BEGIN
    -- Generate a unique verification code
    DECLARE @VerificationCode VARCHAR(50);
    SET @VerificationCode = 'CERT-' + CAST(@UserId AS VARCHAR) + '-' + 
                           CAST(@CertificateId AS VARCHAR) + '-' + 
                           LEFT(REPLACE(NEWID(), '-', ''), 8);
    
    INSERT INTO UserCertificate (
        user_id,
        certificate_id,
        certificate_url,
        verification_code
    )
    VALUES (
        @UserId,
        @CertificateId,
        @CertificateUrl,
        @VerificationCode
    );
    
    SELECT 
        'Certificate issued successfully' AS Result,
        @VerificationCode AS VerificationCode;
END
GO