
CREATE   PROCEDURE spGet_messages_by_filters
@fromDate DATETIME,
@toDate DATETIME,
@pageNumber int=1,
@pageSize int=10,
@contentLength int=NULL,
@content nvarchar(20)=Null
AS 
BEGIN
SELECT*FROM [Messages] WHERE ([Date] BETWEEN @fromDate AND @toDate)
AND (@contentLength IS NULL OR LEN([Content]) = @contentLength )
AND (@content IS NULL OR [Content] LIKE '%'+@content+'%')
ORDER BY [Id]
OFFSET @pageSize * (@pageNumber - 1) ROWS FETCH NEXT @pageSize ROWS ONLY
END
