USE [QuizManagementSystem]
GO
/****** Object:  StoredProcedure [dbo].[PR_MST_Question_Delete]    Script Date: 28-01-2025 20:08:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--4)Delete
CREATE PROCEDURE [dbo].[PR_MST_Question_Delete]
    @QuestionID INT
AS
BEGIN
    DELETE FROM [dbo].[MST_Question]
    WHERE [QuestionID] = @QuestionID
END;
GO
/****** Object:  StoredProcedure [dbo].[PR_MST_Question_Insert]    Script Date: 28-01-2025 20:08:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PR_MST_Question_Insert]
    @QuestionText NVARCHAR(MAX),
    @QuestionLevelID INT,
    @OptionA NVARCHAR(100),
    @OptionB NVARCHAR(100),
    @OptionC NVARCHAR(100),
    @OptionD NVARCHAR(100),
    @CorrectOption NVARCHAR(100),
    @QuestionMarks INT,
    @UserID INT
AS
BEGIN
    INSERT INTO [dbo].[MST_Question] 
        ([QuestionText], [QuestionLevelID], [OptionA], [OptionB], [OptionC], [OptionD], [CorrectOption], [QuestionMarks], [UserID])
    VALUES 
        (@QuestionText, @QuestionLevelID, @OptionA, @OptionB, @OptionC, @OptionD, @CorrectOption, @QuestionMarks, @UserID);
END;
GO
/****** Object:  StoredProcedure [dbo].[PR_MST_Question_SelectAll]    Script Date: 28-01-2025 20:08:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[PR_MST_Question_SelectAll]
@UserID int
AS
BEGIN
    SELECT 
        Q.[QuestionID],
        Q.[QuestionText],
		Q.[QuestionLevelID],
        QL.[QuestionLevel],
        Q.[OptionA],
        Q.[OptionB],
        Q.[OptionC],
        Q.[OptionD],
        Q.[CorrectOption],
        Q.[QuestionMarks],
        Q.[IsActive],
		Q.[UserID],
        U.[UserName],
        Q.[Created],
        Q.[Modified]
    FROM 
        [dbo].[MST_Question] Q
    INNER JOIN [dbo].[MST_QuestionLevel] QL
        ON QL.[QuestionLevelID] = Q.[QuestionLevelID]
    INNER JOIN [dbo].[MST_User] U
        ON U.[UserID] = Q.[UserID]
	Where U.[UserID] = @UserID
end
GO
/****** Object:  StoredProcedure [dbo].[PR_MST_Question_SelectByPK]    Script Date: 28-01-2025 20:08:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--5)SelectByPK
CREATE PROCEDURE [dbo].[PR_MST_Question_SelectByPK]
    @QuestionID INT
AS
BEGIN
    SELECT 
        Q.[QuestionID],
        Q.[QuestionText],
        Q.[QuestionLevelID],
        Q.[OptionA],
        Q.[OptionB],
        Q.[OptionC],
        Q.[OptionD],
        Q.[CorrectOption],
        Q.[QuestionMarks],
        Q.[IsActive],
        Q.[UserID],
        Q.[Created],
        Q.[Modified]
    FROM 
        [dbo].[MST_Question] Q
    WHERE 
        Q.[QuestionID] = @QuestionID;
END;
GO
/****** Object:  StoredProcedure [dbo].[PR_MST_Question_Update]    Script Date: 28-01-2025 20:08:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PR_MST_Question_Update]
    @QuestionID INT,
    @QuestionText NVARCHAR(MAX),
    @QuestionLevelID INT,
    @OptionA NVARCHAR(100),
    @OptionB NVARCHAR(100),
    @OptionC NVARCHAR(100),
    @OptionD NVARCHAR(100),
    @CorrectOption NVARCHAR(100),
    @QuestionMarks INT,
    @IsActive BIT,
    @UserID INT
AS
BEGIN
    UPDATE [dbo].[MST_Question]
    SET 
        [QuestionText] = @QuestionText,
        [QuestionLevelID] = @QuestionLevelID,
        [OptionA] = @OptionA,
        [OptionB] = @OptionB,
        [OptionC] = @OptionC,
        [OptionD] = @OptionD,
        [CorrectOption] = @CorrectOption,
        [QuestionMarks] = @QuestionMarks,
        [IsActive] = @IsActive,
        [UserID] = @UserID,
        [Modified] = GETDATE()
    WHERE 
        [QuestionID] = @QuestionID;
END;
GO
/****** Object:  StoredProcedure [dbo].[PR_MST_QuestionLevel_Delete]    Script Date: 28-01-2025 20:08:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--4)Delete
CREATE PROCEDURE [dbo].[PR_MST_QuestionLevel_Delete]
    @QuestionLevelID INT
AS
BEGIN
    DELETE FROM [dbo].[MST_QuestionLevel]
    WHERE [dbo].[MST_QuestionLevel].[QuestionLevelID] = @QuestionLevelID;
END;
GO
/****** Object:  StoredProcedure [dbo].[PR_MST_QuestionLevel_DropDown]    Script Date: 28-01-2025 20:08:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PR_MST_QuestionLevel_DropDown]
@UserID int
AS
BEGIN
    -- Select the required columns for the dropdown
    SELECT 
        QuestionLevelID,       -- Primary Key for the level
        QuestionLevel      -- Name/Description of the level
    FROM 
        MST_QuestionLevel  -- Replace with the name of your table
	where UserID = @UserID
    ORDER BY 
        QuestionLevel ASC; -- Optional: Order by LevelName alphabetically
END;
GO
/****** Object:  StoredProcedure [dbo].[PR_MST_QuestionLevel_Insert]    Script Date: 28-01-2025 20:08:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PR_MST_QuestionLevel_Insert]
    @QuestionLevel NVARCHAR(100),
    @UserID INT
AS
BEGIN
    INSERT INTO [dbo].[MST_QuestionLevel] 
        ([QuestionLevel], [UserID])
    VALUES 
        (@QuestionLevel, @UserID);
END;
GO
/****** Object:  StoredProcedure [dbo].[PR_MST_QuestionLevel_SelectAll]    Script Date: 28-01-2025 20:08:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[PR_MST_QuestionLevel_SelectAll]
@UserID int
AS
BEGIN
    SELECT 
        [dbo].[MST_QuestionLevel].[QuestionLevelID],
        [dbo].[MST_QuestionLevel].[QuestionLevel],
		[dbo].[MST_QuestionLevel].[UserID],
        u.[UserName],
        [dbo].[MST_QuestionLevel].[Created],
        [dbo].[MST_QuestionLevel].[Modified]
    FROM 
        [dbo].[MST_QuestionLevel]
    INNER JOIN [dbo].[MST_User] u
        ON u.[UserID] = [dbo].[MST_QuestionLevel].[UserID]
	Where u.[UserID] = @UserID
    ORDER BY 
        [dbo].[MST_QuestionLevel].[QuestionLevel] ASC;

END;
GO
/****** Object:  StoredProcedure [dbo].[PR_MST_QuestionLevel_SelectByPK]    Script Date: 28-01-2025 20:08:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--5)SelectByPK
CREATE PROCEDURE [dbo].[PR_MST_QuestionLevel_SelectByPK]
    @QuestionLevelID INT
AS
BEGIN
    SELECT 
        [dbo].[MST_QuestionLevel].[QuestionLevelID],
        [dbo].[MST_QuestionLevel].[QuestionLevel],
        [dbo].[MST_QuestionLevel].[UserID],
        [dbo].[MST_QuestionLevel].[Created],
        [dbo].[MST_QuestionLevel].[Modified]
    FROM 
        [dbo].[MST_QuestionLevel]
    WHERE 
        [dbo].[MST_QuestionLevel].[QuestionLevelID] = @QuestionLevelID;
END;
GO
/****** Object:  StoredProcedure [dbo].[PR_MST_QuestionLevel_Update]    Script Date: 28-01-2025 20:08:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--3)Update
CREATE PROCEDURE [dbo].[PR_MST_QuestionLevel_Update]
    @QuestionLevelID INT,
    @QuestionLevel NVARCHAR(100),
    @UserID INT
AS
BEGIN
    UPDATE [dbo].[MST_QuestionLevel]
    SET 
        [dbo].[MST_QuestionLevel].[QuestionLevel] = @QuestionLevel,
        [dbo].[MST_QuestionLevel].[UserID] = @UserID,
        [dbo].[MST_QuestionLevel].[Modified] = GETDATE()
    WHERE 
        [dbo].[MST_QuestionLevel].[QuestionLevelID] = @QuestionLevelID;
END;
GO
/****** Object:  StoredProcedure [dbo].[PR_MST_Quiz_Delete]    Script Date: 28-01-2025 20:08:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[PR_MST_Quiz_Delete]
    @QuizID INT
AS
BEGIN
    Delete FROM [dbo].[MST_Quiz]
    WHERE [QuizID] = @QuizID;
END;
GO
/****** Object:  StoredProcedure [dbo].[PR_MST_Quiz_Insert]    Script Date: 28-01-2025 20:08:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PR_MST_Quiz_Insert]
    @QuizName NVARCHAR(100),
    @TotalQuestions INT,
    @QuizDate DATETIME,
    @UserID INT
AS
BEGIN
    INSERT INTO [dbo].[MST_Quiz] 
        ([QuizName], [TotalQuestions], [QuizDate], [UserID])
    VALUES
        (@QuizName, @TotalQuestions, @QuizDate, @UserID)
END;
GO
/****** Object:  StoredProcedure [dbo].[PR_MST_Quiz_SelectAll]    Script Date: 28-01-2025 20:08:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[PR_MST_Quiz_SelectAll]
@UserID int
AS
BEGIN
    SELECT 
        m.[QuizID],
        m.[QuizName],
        m.[TotalQuestions],
        m.[QuizDate],
        u.[UserID],
		u.[UserName],
        m.[Created],
        m.[Modified]
    FROM 
        [dbo].[MST_Quiz] m
	Inner join MST_User u
	on u.[UserID] = m.[UserID]
	where u.[UserID] = @UserID
    ORDER BY 
        m.[QuizName] asc,
		m.[QuizDate] desc,
		u.UserName asc;
END;
GO
/****** Object:  StoredProcedure [dbo].[PR_MST_Quiz_SelectByPK]    Script Date: 28-01-2025 20:08:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[PR_MST_Quiz_SelectByPK]
    @QuizID INT
AS
BEGIN
    SELECT 
        [QuizID],
        [QuizName],
        [TotalQuestions],
        [QuizDate],
        [UserID],
        [Created],
        [Modified]
    FROM 
        [dbo].[MST_Quiz]
    WHERE 
        [QuizID] = @QuizID;
END;
GO
/****** Object:  StoredProcedure [dbo].[PR_MST_Quiz_Update]    Script Date: 28-01-2025 20:08:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PR_MST_Quiz_Update]
    @QuizID INT,
    @QuizName NVARCHAR(100),
    @TotalQuestions INT,
    @QuizDate DATETIME,
    @UserID INT
AS
BEGIN
    UPDATE [dbo].[MST_Quiz]
    SET 
        [QuizName] = @QuizName,
        [TotalQuestions] = @TotalQuestions,
        [QuizDate] = @QuizDate,
        [UserID] = @UserID,
        [Modified] = GETDATE()
    WHERE 
        [QuizID] = @QuizID;
END;
GO
/****** Object:  StoredProcedure [dbo].[PR_MST_QuizWiseQuestions_Delete]    Script Date: 28-01-2025 20:08:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[PR_MST_QuizWiseQuestions_Delete]
    @QuizWiseQuestionsID INT
AS
BEGIN
    
    delete FROM [dbo].[MST_QuizWiseQuestions] 
    WHERE [QuizWiseQuestionsID] = @QuizWiseQuestionsID;
END;
GO
/****** Object:  StoredProcedure [dbo].[PR_MST_QuizWiseQuestions_Insert]    Script Date: 28-01-2025 20:08:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PR_MST_QuizWiseQuestions_Insert]
    @QuizID INT,
    @QuestionID INT,
    @UserID INT
AS
BEGIN
    INSERT INTO [dbo].[MST_QuizWiseQuestions] 
        ([QuizID], [QuestionID], [UserID])
    VALUES
        (@QuizID, @QuestionID, @UserID);
END;
GO
/****** Object:  StoredProcedure [dbo].[PR_MST_QuizWiseQuestions_SelectAll]    Script Date: 28-01-2025 20:08:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[PR_MST_QuizWiseQuestions_SelectAll]
@UserID int
AS
BEGIN
    SELECT 
        QWQ.[QuizWiseQuestionsID],
        QWQ.[QuizID],
        Q.[QuizName],
        QWQ.[QuestionID],
        QT.[QuestionText],
        QWQ.[UserID],
        U.[UserName],
        QWQ.[Created],
        QWQ.[Modified]
    FROM 
        [dbo].[MST_QuizWiseQuestions] QWQ
    INNER JOIN 
        [dbo].[MST_Quiz] Q ON Q.[QuizID] = QWQ.[QuizID]
    INNER JOIN 
        [dbo].[MST_Question] QT ON QT.[QuestionID] = QWQ.[QuestionID]
    INNER JOIN 
        [dbo].[MST_User] U ON U.[UserID] = QWQ.[UserID]
	where U.[UserID] = @UserID
    ORDER BY 
        Q.[QuizName], QT.[QuestionText];
END;
GO
/****** Object:  StoredProcedure [dbo].[PR_MST_QuizWiseQuestions_SelectByPK]    Script Date: 28-01-2025 20:08:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PR_MST_QuizWiseQuestions_SelectByPK]
    @QuizWiseQuestionsID INT
AS
BEGIN
    SELECT 
        QWQ.[QuizWiseQuestionsID],
        QWQ.[QuizID],
        QWQ.[QuestionID],
        QWQ.[UserID],
        QWQ.[Created],
        QWQ.[Modified]
    FROM 
        [dbo].[MST_QuizWiseQuestions] QWQ
    WHERE 
        QWQ.[QuizWiseQuestionsID] = @QuizWiseQuestionsID;
END;
GO
/****** Object:  StoredProcedure [dbo].[PR_MST_QuizWiseQuestions_Update]    Script Date: 28-01-2025 20:08:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[PR_MST_QuizWiseQuestions_Update]
    @QuizWiseQuestionsID INT,
    @QuizID INT,
    @QuestionID INT,
    @UserID INT
AS
BEGIN
    UPDATE [dbo].[MST_QuizWiseQuestions]
    SET 
        [QuizID] = @QuizID,
        [QuestionID] = @QuestionID,
        [UserID] = @UserID,
        [Modified] = GETDATE()
    WHERE 
        [QuizWiseQuestionsID] = @QuizWiseQuestionsID;
END;
GO
/****** Object:  StoredProcedure [dbo].[PR_MST_User_Delete]    Script Date: 28-01-2025 20:08:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PR_MST_User_Delete]
    @UserID INT
AS
BEGIN
    DELETE FROM [dbo].[MST_User]
    WHERE [dbo].[MST_User].[UserID] = @UserID;
END;
GO
/****** Object:  StoredProcedure [dbo].[PR_MST_User_DropDown]    Script Date: 28-01-2025 20:08:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[PR_MST_User_DropDown]
AS
BEGIN
    -- Select the required columns for the dropdown
    SELECT 
        UserID,      -- Unique identifier for the user
        Username     -- Display name for the dropdown
    FROM 
        MST_User        -- Replace 'Users' with the actual table name
    ORDER BY 
        Username ASC; -- Optional: Order alphabetically by username
END;
GO
/****** Object:  StoredProcedure [dbo].[PR_MST_User_Insert]    Script Date: 28-01-2025 20:08:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PR_MST_User_Insert]
    @UserName NVARCHAR(100),
    @Password NVARCHAR(100),
    @Email NVARCHAR(100),
    @Mobile NVARCHAR(100)
AS
BEGIN
    INSERT INTO [dbo].[MST_User] 
        ([UserName], [Password], [Email], [Mobile])
    VALUES 
        (@UserName, @Password, @Email, @Mobile)

    -- Optionally return the inserted UserID
    SELECT SCOPE_IDENTITY() AS NewUserID;
END;
GO
/****** Object:  StoredProcedure [dbo].[PR_MST_User_SelectAll]    Script Date: 28-01-2025 20:08:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--============================= MST_User ==============================
--1)SelectAll
CREATE PROCEDURE [dbo].[PR_MST_User_SelectAll]
AS
BEGIN
    SELECT 
        [dbo].[MST_User].[UserID],
        [dbo].[MST_User].[UserName],
        [dbo].[MST_User].[Email],
        [dbo].[MST_User].[Mobile],
        [dbo].[MST_User].[Password],
        [dbo].[MST_User].[IsActive],
        [dbo].[MST_User].[IsAdmin],
        [dbo].[MST_User].[Created],
        [dbo].[MST_User].[Modified]
    FROM 
        [dbo].[MST_User]
    ORDER BY 
        [dbo].[MST_User].[UserName], 
        [dbo].[MST_User].[Email];
END;
GO
/****** Object:  StoredProcedure [dbo].[PR_MST_User_SelectByPK]    Script Date: 28-01-2025 20:08:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PR_MST_User_SelectByPK]
    @UserID INT
AS
BEGIN
    SELECT 
        [dbo].[MST_User].[UserID],
        [dbo].[MST_User].[UserName],
        [dbo].[MST_User].[Email],
        [dbo].[MST_User].[Mobile],
        [dbo].[MST_User].[Password],
        [dbo].[MST_User].[IsActive],
        [dbo].[MST_User].[IsAdmin],
        [dbo].[MST_User].[Created],
        [dbo].[MST_User].[Modified]
    FROM 
        [dbo].[MST_User]
    WHERE 
        [dbo].[MST_User].[UserID] = @UserID;
END;
GO
/****** Object:  StoredProcedure [dbo].[PR_MST_User_Update]    Script Date: 28-01-2025 20:08:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PR_MST_User_Update]
    @UserID INT,
    @UserName NVARCHAR(100),
    @Password NVARCHAR(100),
    @Email NVARCHAR(100),
    @Mobile NVARCHAR(100),
    @IsActive BIT,
    @IsAdmin BIT
AS
BEGIN
    UPDATE [dbo].[MST_User]
    SET 
        [dbo].[MST_User].[UserName] = @UserName,
        [dbo].[MST_User].[Password] = @Password,
        [dbo].[MST_User].[Email] = @Email,
        [dbo].[MST_User].[Mobile] = @Mobile,
        [dbo].[MST_User].[IsActive] = @IsActive,
        [dbo].[MST_User].[IsAdmin] = @IsAdmin,
        [dbo].[MST_User].[Modified] = GETDATE()
    WHERE 
        [dbo].[MST_User].[UserID] = @UserID
END
GO
/****** Object:  StoredProcedure [dbo].[PR_Question_DropDown]    Script Date: 28-01-2025 20:08:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[PR_Question_DropDown]
@UserID int
AS
BEGIN
    SELECT QuestionID, QuestionText
    FROM MST_Question
	Where MST_Question.UserID = @UserID
    ORDER BY QuestionText;
END
GO
/****** Object:  StoredProcedure [dbo].[PR_Quiz_DropDown]    Script Date: 28-01-2025 20:08:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PR_Quiz_DropDown]
@UserID int
AS
BEGIN
    SELECT QuizID, QuizName
    FROM MST_Quiz
	where MST_Quiz.UserID = @UserID
    ORDER BY QuizName;
END
GO
/****** Object:  StoredProcedure [dbo].[PR_User_DropDown]    Script Date: 28-01-2025 20:08:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[PR_User_DropDown]
AS
BEGIN
    SELECT UserID, UserName
    FROM MST_User
    ORDER BY UserName;
END


GO
/****** Object:  StoredProcedure [dbo].[PR_User_Login]    Script Date: 28-01-2025 20:08:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[PR_User_Login]
    @UserName NVARCHAR(100),
    @Password NVARCHAR(100)
AS
BEGIN
    SELECT 
        [dbo].[MST_User].[UserID], 
        [dbo].[MST_User].[UserName],
        [dbo].[MST_User].[Email],
		[dbo].[MST_User].[Password],
        [dbo].[MST_User].[Mobile],
		[dbo].[MST_User].[IsActive],
        [dbo].[MST_User].[IsAdmin]
    FROM 
        [dbo].[MST_User] 
    WHERE 
        [dbo].[MST_User].[UserName] = @UserName 
        AND [dbo].[MST_User].[Password] = @Password;
END
GO
/****** Object:  StoredProcedure [dbo].[usp_GetDashboardData]    Script Date: 28-01-2025 20:08:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[usp_GetDashboardData]
@UserId int
AS
BEGIN
    -- Enable NOCOUNT for better performance
    SET NOCOUNT ON;
-- SET NOCOUNT ON: Suppresses the message from being returned. This prevents the sending of DONEINPROC messages to the client for each
-- statement in a stored procedure.
-- SET NOCOUNT OFF: Includes the message in the result set. 
    -- Temporary tables for organized data fetching
	CREATE TABLE #Counts (
        Metric NVARCHAR(255),
        Value INT
		);

    CREATE TABLE #RecentQuizes (
        QuizID INT,
        QuizName NVARCHAR(100),
		TotalQuestions INT,
        QuizDate DATETIME
    );

    CREATE TABLE #RecentQuestions (
        QuestionID INT,
        QuestionText NVARCHAR(max),
        QuestionMarks INT,
        CorrectOption NVARCHAR(100)
    );

	CREATE TABLE #RecentUsers (
        UserID INT,
        UserName NVARCHAR(100),
        Email NVARCHAR(100),
        Mobile NVARCHAR(100)
    );

    CREATE TABLE #TopActiveUsers (
        UserName NVARCHAR(100),
        TotalQuizes INT,
		TotalQuestions INT
    );

    ---- Step 1: Get Counts
    --
	INSERT INTO #Counts
        SELECT 'TotalQuizes', COUNT(*) FROM [MST_Quiz] Where [MST_Quiz].UserID = @UserId
    INSERT INTO #Counts
	    SELECT 'TotalQuestions', COUNT(*) FROM [MST_Question] Where [MST_Question].UserID = @UserId
	INSERT INTO #Counts
		SELECT 'TotalUsers',COUNT(*) FROM [MST_User]
	INSERT INTO #Counts
		SELECT 'TotalQuestionLevels',COUNT(*) FROM [MST_QuestionLevel] Where [MST_QuestionLevel].UserID = @UserId
	

    -- Step 2: Get Recent 10 Quizes
    INSERT INTO #RecentQuizes
    SELECT TOP 10
        Q.QuizID,
        Q.QuizName,
        Q.TotalQuestions,
		Q.QuizDate
    FROM [MST_Quiz] Q
	Where Q.UserID = @UserId 
    ORDER BY Q.QuizDate DESC;

    -- Step 3: Get Recent 10 Newly Added Questions
    INSERT INTO #RecentQuestions 
    SELECT TOP 10
        QuestionID,
        QuestionText,
        QuestionMarks,
        CorrectOption
    FROM [MST_Question]
	Where [MST_Question].UserID = @UserId
    ORDER BY QuestionText DESC;

	-- Step 4: Get Recent 10 Newly Added Users
    INSERT INTO #RecentUsers
    SELECT TOP 10
        UserID,
        UserName,
        Email,
		Mobile
    FROM [MST_User]
    ORDER BY UserName DESC;

    -- Step 5: Get Top 10 Active Users
    INSERT INTO #TopActiveUsers
    SELECT TOP 10
        U.UserName,
        COUNT(DISTINCT Q.QuizID) AS TotalQuizes,
		COUNT(DISTINCT [MST_Question].[QuestionID]) AS TotalQuestions
    FROM MST_User U
    LEFT OUTER JOIN [MST_Quiz] Q ON Q.UserID = U.UserID
	LEFT OUTER JOIN [MST_Question] ON [MST_Question].UserID = U.UserID
    GROUP BY U.UserName
    ORDER BY COUNT(DISTINCT Q.QuizID),COUNT(DISTINCT [MST_Question].[QuestionID]) DESC;

    -- Output Results
    -- Output Counts
    SELECT * FROM #Counts;

    -- Output Recent Quizes
    SELECT * FROM #RecentQuizes;

    -- Output Recent Questions
    SELECT * FROM #RecentQuestions;

    -- Output Recent Users
    SELECT * FROM #RecentUsers;

    -- Output Top Active Users
    SELECT * FROM #TopActiveUsers;

    -- Cleanup Temporary Tables
    DROP TABLE #RecentQuizes;
    DROP TABLE #RecentQuestions;
    DROP TABLE #RecentUsers;
    DROP TABLE #TopActiveUsers;
END;

GO
