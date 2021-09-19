CREATE     Proc [dbo].[spCreate_new_message]
@content nvarchar(20),
@type nvarchar(50),
@date datetime,
@elementId nvarchar(150)
AS
BEGIN
insert into [Messages]([Content],[Type],[Date],[ElementId]) Values (@content,@type,@date,@elementId)
END