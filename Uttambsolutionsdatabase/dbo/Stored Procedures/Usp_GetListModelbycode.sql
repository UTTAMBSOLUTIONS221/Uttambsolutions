CREATE PROCEDURE [dbo].[Usp_GetListModelbycode]
@Type int,
@Code BIGINT
AS
BEGIN
SET NOCOUNT ON;
If(@Type = 91)
Select r.Subcountyname as Text, r.Subcountyid as Value From Systemsubcounty r WHERE  r.Countyid=@Code;
If(@Type = 911)
Select r.Subcountywardname as Text, r.Subcountywardid as Value From Systemsubcountyward r WHERE  r.Subcountyid=@Code;
If(@Type = 29)
Select r.Propertyhousename as Text, r.Propertyhouseid as Value From SystemPropertyHouses r WHERE r.Propertyhouseowner=@Code;
END
