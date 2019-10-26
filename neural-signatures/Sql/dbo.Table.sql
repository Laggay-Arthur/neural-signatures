CREATE TABLE [dbo].[DocumentsAll]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [name_document] VARCHAR(100) NOT NULL, 
    [name_people] VARCHAR(15) NULL, 
    [text_document] VARCHAR(MAX) NULL, 
    [date_document] DATETIME2 NOT NULL, 
    [date_validity] DATETIME2 NULL
)
