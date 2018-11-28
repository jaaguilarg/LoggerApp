USE [Logsdb]
GO

/****** Object:  StoredProcedure [dbo].[spInsertLog]    Script Date: 11/27/2018 9:55:13 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spInsertLog] 
	-- Add the parameters for the stored procedure here
	@LogId uniqueIdentifier,
	@EventDate datetime,
	@Description varchar(max),
	@Type varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO Log (LogId,EventDate,Description,Type) VALUES (@LogId,@EventDate,@Description,@Type)
END
GO


