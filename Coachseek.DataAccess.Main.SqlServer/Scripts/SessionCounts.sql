/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP 1000 [Id]
      ,[BusinessId]
      ,[Guid]
      ,[ParentId]
      ,[LocationId]
      ,[CoachId]
      ,[ServiceId]
      ,[Name]
      ,[StartDate]
      ,[StartTime]
      ,[Duration]
      ,[StudentCapacity]
      ,[IsOnlineBookable]
      ,[SessionCount]
      ,[RepeatFrequency]
      ,[SessionPrice]
      ,[CoursePrice]
      ,[Colour]
  FROM [Coachseek-Prod].[dbo].[Session]

-- Session Counts for ALL Businesses
Select Count(*) As AllSessions
From [dbo].[Session]

Select Count(*) As Courses
From [dbo].[Session]
Where [SessionCount] > 1
  
Select Count(*) As CourseSessions
From [dbo].[Session]
Where [ParentId] Is Not Null And [RepeatFrequency] Is Null

Select Count(*) As StandaloneSessions
From [dbo].[Session]
Where [ParentId] Is Null And [RepeatFrequency] Is Null

-- Number Of Real Businesses
Select Count(*)
From [dbo].[Business]
Where [IsTesting] = 0

-- Session Counts for Real Businesses
Select Count(*) As AllSessionsByRealBusinesses
From [dbo].[Session] s
	Inner Join [dbo].[Business] b
		On s.[BusinessId] = b.[Id]
Where b.[IsTesting] = 0

Select Count(*) As CoursesByRealBusinesses
From [dbo].[Session] s
	Inner Join [dbo].[Business] b
		On s.[BusinessId] = b.[Id]
Where b.[IsTesting] = 0
AND s.[SessionCount] > 1
  
Select Count(*) As CourseSessionsByRealBusinesses
From [dbo].[Session] s
	Inner Join [dbo].[Business] b
		On s.[BusinessId] = b.[Id]
Where b.[IsTesting] = 0
And s.[ParentId] Is Not Null And s.[RepeatFrequency] Is Null

Select Count(*) As StandaloneSessionsByRealBusinesses
From [dbo].[Session] s
	Inner Join [dbo].[Business] b
		On s.[BusinessId] = b.[Id]
Where b.[IsTesting] = 0
AND s.[ParentId] Is Null And s.[RepeatFrequency] Is Null

