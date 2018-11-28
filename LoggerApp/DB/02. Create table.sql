USE [Logsdb]
GO

/****** Object:  Table [dbo].[Log]    Script Date: 11/27/2018 9:53:32 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Log](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LogId] [uniqueidentifier] NOT NULL,
	[EventDate] [datetime] NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[Type] [varchar](50) NOT NULL,
 CONSTRAINT [PK__Log__3214EC07743B3ABC] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

