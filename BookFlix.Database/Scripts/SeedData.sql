/*
    Seed data for the BookFlix database.
    Run this script after publishing the SQL project to populate initial data.
    This script is idempotent - it uses MERGE to avoid duplicate inserts.
*/

-- =============================================
-- Roles
-- =============================================
MERGE INTO [dbo].[Roles] AS target
USING (VALUES
    ('32684285-5ff9-486d-a2a4-de00bdea2d20', N'Admin'),
    ('2abd05f3-fc73-4a5f-a3b5-01291030851f', N'User')
) AS source ([ID], [Name])
ON target.[ID] = source.[ID]
WHEN NOT MATCHED THEN
    INSERT ([ID], [Name]) VALUES (source.[ID], source.[Name]);
GO

-- =============================================
-- Users (admin user, password hash is BCrypt)
-- =============================================
MERGE INTO [dbo].[Users] AS target
USING (VALUES
    ('3dba3903-21a6-413d-a479-eb807eb5e6ed', N'admin', N'admin@example.com',
     N'$2a$11$IAzmX9gT.qkqm45lMnyh/uE0PZ793GOyIKEEn3dNdbAC1cfxcbFVa',
     '2025-08-05T10:00:00.000')
) AS source ([ID], [Username], [Email], [PasswordHash], [CreatedAt])
ON target.[ID] = source.[ID]
WHEN NOT MATCHED THEN
    INSERT ([ID], [Username], [Email], [PasswordHash], [CreatedAt])
    VALUES (source.[ID], source.[Username], source.[Email], source.[PasswordHash], source.[CreatedAt]);
GO

-- =============================================
-- UserRoles (assign admin role to admin user)
-- =============================================
MERGE INTO [dbo].[UserRoles] AS target
USING (VALUES
    ('3dba3903-21a6-413d-a479-eb807eb5e6ed', '32684285-5ff9-486d-a2a4-de00bdea2d20')
) AS source ([UserID], [RoleID])
ON target.[UserID] = source.[UserID] AND target.[RoleID] = source.[RoleID]
WHEN NOT MATCHED THEN
    INSERT ([UserID], [RoleID]) VALUES (source.[UserID], source.[RoleID]);
GO

-- =============================================
-- Authors
-- =============================================
MERGE INTO [dbo].[Authors] AS target
USING (VALUES
    ('a0000000-0000-0000-0000-000000000001', N'F. Scott Fitzgerald'),
    ('a0000000-0000-0000-0000-000000000002', N'Harper Lee'),
    ('a0000000-0000-0000-0000-000000000003', N'George Orwell'),
    ('a0000000-0000-0000-0000-000000000004', N'Jane Austen'),
    ('a0000000-0000-0000-0000-000000000005', N'J.R.R. Tolkien'),
    ('a0000000-0000-0000-0000-000000000006', N'Frank Herbert'),
    ('a0000000-0000-0000-0000-000000000007', N'Yuval Noah Harari'),
    ('a0000000-0000-0000-0000-000000000008', N'Dan Brown')
) AS source ([ID], [Name])
ON target.[ID] = source.[ID]
WHEN NOT MATCHED THEN
    INSERT ([ID], [Name]) VALUES (source.[ID], source.[Name]);
GO

-- =============================================
-- Genres
-- =============================================
MERGE INTO [dbo].[Genres] AS target
USING (VALUES
    (1,  N'Fiction'),
    (2,  N'Nonfiction'),
    (3,  N'Science Fiction'),
    (4,  N'Fantasy'),
    (5,  N'Mystery'),
    (6,  N'Thriller'),
    (7,  N'Romance'),
    (8,  N'Historical Fiction'),
    (9,  N'Biography'),
    (10, N'Autobiography'),
    (11, N'Self-Help'),
    (12, N'Business'),
    (13, N'Science'),
    (14, N'History'),
    (15, N'Young Adult'),
    (16, N'Children'),
    (17, N'Poetry'),
    (18, N'Horror'),
    (19, N'Adventure'),
    (20, N'Crime'),
    (21, N'Literary Criticism'),
    (22, N'Cooking'),
    (23, N'Travel'),
    (24, N'Philosophy'),
    (25, N'Religion')
) AS source ([ID], [Name])
ON target.[ID] = source.[ID]
WHEN NOT MATCHED THEN
    INSERT ([ID], [Name]) VALUES (source.[ID], source.[Name]);
GO

-- =============================================
-- Books
-- =============================================
MERGE INTO [dbo].[Books] AS target
USING (VALUES
    ('b0000000-0000-0000-0000-000000000001', N'The Great Gatsby',                    N'A novel set in the Roaring Twenties.',              N'9780743273565', N'https://example.com/great-gatsby.jpg',           '1925-04-10', N'Scribner',              180, 4.20, 1, '2025-08-05T10:00:00.000'),
    ('b0000000-0000-0000-0000-000000000002', N'To Kill a Mockingbird',               N'A novel about racial injustice in the Deep South.',  N'9780061120084', N'https://example.com/to-kill-a-mockingbird.jpg',  '1960-07-11', N'J.B. Lippincott & Co.', 281, 4.30, 1, '2025-08-05T10:00:00.000'),
    ('b0000000-0000-0000-0000-000000000003', N'1984',                                N'A dystopian novel about totalitarianism.',           N'9780451524935', N'https://example.com/1984.jpg',                   '1949-06-08', N'Secker & Warburg',      328, 4.40, 1, '2025-08-05T10:00:00.000'),
    ('b0000000-0000-0000-0000-000000000004', N'Pride and Prejudice',                  N'A romantic novel about love and social class.',      N'9780141439518', N'https://example.com/pride-and-prejudice.jpg',    '1813-01-28', N'Penguin Classics',      432, 4.25, 1, '2025-08-05T10:00:00.000'),
    ('b0000000-0000-0000-0000-000000000005', N'The Hobbit',                           N'A fantasy adventure about Bilbo Baggins.',           N'9780547928227', N'https://example.com/the-hobbit.jpg',             '1937-09-21', N'Houghton Mifflin',      310, 4.27, 1, '2025-08-05T10:00:00.000'),
    ('b0000000-0000-0000-0000-000000000006', N'Dune',                                N'A science fiction epic about a desert planet.',      N'9780441172719', N'https://example.com/dune.jpg',                   '1965-08-01', N'Ace Books',             412, 4.21, 1, '2025-08-05T10:00:00.000'),
    ('b0000000-0000-0000-0000-000000000007', N'Sapiens: A Brief History of Humankind',N'A nonfiction exploration of human history.',         N'9780062316097', N'https://example.com/sapiens.jpg',                '2014-09-09', N'Harper',                443, 4.38, 1, '2025-08-05T10:00:00.000'),
    ('b0000000-0000-0000-0000-000000000008', N'The Da Vinci Code',                    N'A thriller involving a religious conspiracy.',       N'9780307277671', N'https://example.com/da-vinci-code.jpg',          '2003-03-18', N'Doubleday',             454, 3.85, 1, '2025-08-05T10:00:00.000')
) AS source ([ID], [Title], [Description], [ISBN], [CoverImageUrl], [PublicationDate], [Publisher], [PageCount], [AverageRating], [IsAvailable], [CreatedAt])
ON target.[ID] = source.[ID]
WHEN NOT MATCHED THEN
    INSERT ([ID], [Title], [Description], [ISBN], [CoverImageUrl], [PublicationDate], [Publisher], [PageCount], [AverageRating], [IsAvailable], [CreatedAt])
    VALUES (source.[ID], source.[Title], source.[Description], source.[ISBN], source.[CoverImageUrl], source.[PublicationDate], source.[Publisher], source.[PageCount], source.[AverageRating], source.[IsAvailable], source.[CreatedAt]);
GO

-- =============================================
-- BookAuthors
-- =============================================
MERGE INTO [dbo].[BookAuthors] AS target
USING (VALUES
    ('b0000000-0000-0000-0000-000000000001', 'a0000000-0000-0000-0000-000000000001'),
    ('b0000000-0000-0000-0000-000000000002', 'a0000000-0000-0000-0000-000000000002'),
    ('b0000000-0000-0000-0000-000000000003', 'a0000000-0000-0000-0000-000000000003'),
    ('b0000000-0000-0000-0000-000000000004', 'a0000000-0000-0000-0000-000000000004'),
    ('b0000000-0000-0000-0000-000000000005', 'a0000000-0000-0000-0000-000000000005'),
    ('b0000000-0000-0000-0000-000000000006', 'a0000000-0000-0000-0000-000000000006'),
    ('b0000000-0000-0000-0000-000000000007', 'a0000000-0000-0000-0000-000000000007'),
    ('b0000000-0000-0000-0000-000000000008', 'a0000000-0000-0000-0000-000000000008')
) AS source ([BookID], [AuthorID])
ON target.[BookID] = source.[BookID] AND target.[AuthorID] = source.[AuthorID]
WHEN NOT MATCHED THEN
    INSERT ([BookID], [AuthorID]) VALUES (source.[BookID], source.[AuthorID]);
GO

-- =============================================
-- BookGenres
-- =============================================
MERGE INTO [dbo].[BookGenres] AS target
USING (VALUES
    ('b0000000-0000-0000-0000-000000000001', 1),  -- The Great Gatsby -> Fiction
    ('b0000000-0000-0000-0000-000000000002', 1),  -- To Kill a Mockingbird -> Fiction
    ('b0000000-0000-0000-0000-000000000003', 3),  -- 1984 -> Science Fiction
    ('b0000000-0000-0000-0000-000000000004', 7),  -- Pride and Prejudice -> Romance
    ('b0000000-0000-0000-0000-000000000004', 8),  -- Pride and Prejudice -> Historical Fiction
    ('b0000000-0000-0000-0000-000000000005', 4),  -- The Hobbit -> Fantasy
    ('b0000000-0000-0000-0000-000000000005', 19), -- The Hobbit -> Adventure
    ('b0000000-0000-0000-0000-000000000006', 3),  -- Dune -> Science Fiction
    ('b0000000-0000-0000-0000-000000000007', 2),  -- Sapiens -> Nonfiction
    ('b0000000-0000-0000-0000-000000000007', 14), -- Sapiens -> History
    ('b0000000-0000-0000-0000-000000000008', 6),  -- The Da Vinci Code -> Thriller
    ('b0000000-0000-0000-0000-000000000008', 5)   -- The Da Vinci Code -> Mystery
) AS source ([BookID], [GenreID])
ON target.[BookID] = source.[BookID] AND target.[GenreID] = source.[GenreID]
WHEN NOT MATCHED THEN
    INSERT ([BookID], [GenreID]) VALUES (source.[BookID], source.[GenreID]);
GO
