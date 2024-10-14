CREATE TABLE [dbo].[Parcelcollectioncentercourier] (
    [Collectioncenterid] INT    NULL,
    [Courierid]          BIGINT NULL,
    FOREIGN KEY ([Collectioncenterid]) REFERENCES [dbo].[Parcelcollectioncenters] ([Collectioncenterid]),
    FOREIGN KEY ([Courierid]) REFERENCES [dbo].[Systemstaffs] ([Userid])
);

