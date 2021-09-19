
CREATE   PROCEDURE spCreate_new_Slot_Info
@numberMessages int,
@firstAndLastCharacters nvarchar(250),
@ChangedCharacters nvarchar(450),
@maxTimeBetweenContentChanges nvarchar(100)
AS
BEGIN 
INSERT INTO SlotsSummary([NumberMessages],[FirstAndLastCharacters],[ChangedCharacters],[MaxTimeBetweenContentChanges])
VALUES(@numberMessages,@firstAndLastCharacters,@ChangedCharacters,@maxTimeBetweenContentChanges)
END