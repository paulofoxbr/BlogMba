select * from aspnetusers


/*
delete from aspnetusers
delete from AspNetUserRoles
delete from AspNetRoles
*/
SELECT *  FROM AspNetUserRoles;

SELECT Id, Name, NormalizedName, ConcurrencyStamp FROM AspNetRoles;

CREATE TABLE Posts (Id INT NOT NULL, Title NVARCHAR NOT NULL, Content NVARCHAR NOT NULL, Created DATETIME2 NOT NULL, Updated DATETIME2, AuthorId INT NOT NULL, CONSTRAINT PK_Posts PRIMARY KEY (Id));

SELECT * FROM Authors;

SELECT * FROM Posts;

