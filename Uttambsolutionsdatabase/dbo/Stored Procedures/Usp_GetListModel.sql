CREATE PROCEDURE [dbo].[Usp_GetListModel]
@Type int
AS
BEGIN
SET NOCOUNT ON;
If(@Type = 0)
Select r.Rolename as Text, r.Roleid as Value From Systemroles r;
If(@Type = 4)
Select r.Categoryname as Text, r.Categoryid as Value From Productcategories r;
If(@Type = 5)
Select r.Blogcategoryname as Text, r.Blogcategoryid as Value From Systemblogcategories r;
If(@Type = 6)
Select r.Brandname as Text, r.Brandid as Value From Productbrand r;
If(@Type = 8)
Select r.Permissionname as Text, r.PermissionId as Value From Systempermissions r;
If(@Type = 9)
Select r.Countyname as Text, r.Countyid as Value From Systemcounty r;
If(@Type = 91)
Select r.Subcountyname as Text, r.Subcountyid as Value From Systemsubcounty r;
If(@Type = 911)
Select r.Subcountywardname as Text, r.Subcountywardid as Value From Systemsubcountyward r;
If(@Type = 13)
Select r.Functionname as Text, r.Jobfunctionid as Value From Jobfunctions r;
If(@Type = 14)
Select r.Jobindustryname as Text, r.Jobindustryid as Value From Jobindustries r;
If(@Type = 15)
Select r.Locationname as Text, r.Joblocationid as Value From Joblocations r;
If(@Type = 16)
Select r.Experiencename as Text, r.Jobexperienceid as Value From Jobexperiences r;
If(@Type = 17)
Select r.Jobtypename as Text, r.Jobtypeid as Value From Jobtypes r;
If(@Type = 18)
Select r.Organizationname as Text, r.Organizationid as Value From Systemorganizations r;
If(@Type = 19)
Select r.Housebenefitname as Text, r.Housebenefitid as Value From Systemhousebenefits r;
If(@Type = 21)
Select r.Housedepositfeename as Text, r.Housedepositfeeid as Value From Systemhousedepositfees r;
If(@Type = 22)
Select r.Systemhousesizename as Text, r.Systemhousesizeid as Value From Systemhousesizes r;
If(@Type = 23)
Select r.Systemhousewatertypename as Text, r.Systemhousewatertypeid as Value From Systemhousewatertype r;
If(@Type = 24)
Select r.Kitchentypename as Text, r.Kitchentypeid as Value From Systemhousekitchentype r;
If(@Type = 25)
Select r.Paymentmode as Text, r.PaymentmodeId as Value From Paymentmodes r;
If(@Type = 215)
Select r.Paymentmode as Text, r.PaymentmodeId as Value From Paymentmodes r Where r.PaymentmodetypeId= (SELECT s.PaymentmodetypeId FROM Paymentmodetypes s WHERE s.Paymentmodetype='Cash');

If(@Type = 26)
Select r.Gendername as Text, r.Genderid as Value From Systemgender r;
If(@Type = 27)
Select r.Maritalstatusname as Text, r.Maritalstatusid as Value From Systemmaritalstatus r;
If(@Type = 28)
Select r.Kinrelationshipname as Text, r.Kinrelationshipid as Value From Systemkinrelationship r;

END
