/* Виконайте у базі AC після застосування EF Core migration. */
GO

CREATE OR ALTER VIEW dbo.vw_UserMovies
AS
SELECT
    u.Id AS UserId,
    u.Username,
    u.Email,
    m.Id AS MovieId,
    m.Title AS MovieTitle,
    m.ReleaseYear,
    m.AddedAt
FROM dbo.Users AS u
LEFT JOIN dbo.Movies AS m ON m.UserId = u.Id;
GO

CREATE OR ALTER PROCEDURE dbo.AddUser
    @Username nvarchar(50),
    @Email nvarchar(254),
    @Password nvarchar(max)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Users (Username, Email, Password)
    VALUES (@Username, @Email, @Password);

    SELECT CONVERT(int, SCOPE_IDENTITY()) AS Id;
END;
GO
