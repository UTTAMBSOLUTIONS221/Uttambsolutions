CREATE TABLE [dbo].[Systemroleperms] (
    [RoleId]       BIGINT NOT NULL,
    [PermissionId] BIGINT NOT NULL,
    CONSTRAINT [FK__Systemrol__Permi__32E0915F] FOREIGN KEY ([PermissionId]) REFERENCES [dbo].[Systempermissions] ([PermissionId]),
    CONSTRAINT [FK__Systemrol__RoleI__33D4B598] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Systemroles] ([Roleid])
);

