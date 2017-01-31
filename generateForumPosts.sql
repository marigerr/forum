declare @dateVariable date
declare @Counter int

set @Counter = 20

While(@Counter <= 10000)
Begin

	select @dateVariable = DATEADD(DAY, -@Counter , GETDATE())
	

	Insert into ForumPosts([TimeStamp],Title, UserId, PostContent)
	values (@dateVariable, @Counter, 'd204599d-6cc3-4f07-b6d4-b5b211ed004b', @Counter)

	Set @Counter = @Counter + 1
End