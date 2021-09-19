
CREATE   PROCEDURE spGetSlots_by_filters
@pageNumber int=1,
@pageSize int=10,
@fromId int=NULL
AS 
BEGIN 

SELECT*FROM SlotsSummary
WHERE [Id] IS NULL OR [Id]>=@fromId
ORDER BY [Id]
OFFSET @pageSize * (@pageNumber - 1) ROWS FETCH NEXT @pageSize ROWS ONLY
END