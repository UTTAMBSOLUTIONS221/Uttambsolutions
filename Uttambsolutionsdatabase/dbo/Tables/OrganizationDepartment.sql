CREATE TABLE [dbo].[OrganizationDepartment] (
    [DepartmentId]    BIGINT        IDENTITY (1, 1) NOT NULL,
    [OrganizationId]  BIGINT        NOT NULL,
    [DepartmentName]  VARCHAR (100) NOT NULL,
    [DepartmentEmail] VARCHAR (200) NOT NULL,
    CONSTRAINT [PK__Organiza__B2079BED02DE3004] PRIMARY KEY CLUSTERED ([DepartmentId] ASC)
);

