USE [CanYouCodeDb]
GO

CREATE TABLE [dbo].[Account](
	[Id] BigInt IDENTITY(1,1) NOT NULL,
	[DateAdded] DateTime NOT NULL,
	[Email] NVarChar(50) NOT NULL,
	[LastLoginDate] DateTime NOT NULL,
	[Password] NVarChar(50) NOT NULL,
	[Phone] NVarChar(50),
	[Rating] Int NOT NULL,
	[Status] NVarChar(50) NOT NULL,
	[Type] NVarChar(50) NOT NULL,
	[Username] NVarChar(50) NOT NULL,
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED (
	[Id] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[AccountAlert](
	[Id] BigInt IDENTITY(1,1) NOT NULL,
	[Status] NVarChar(50) NOT NULL,
	[Message] NVarChar(4000) NOT NULL,
	[Type] NVarChar(50) NOT NULL,
	[Account] BigInt,
 CONSTRAINT [PK_AccountAlert] PRIMARY KEY CLUSTERED (
	[Id] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[Attachment](
	[Id] BigInt IDENTITY(1,1) NOT NULL,
	[DateAdded] DateTime NOT NULL,
	[OriginalFileName] NVarChar(500) NOT NULL,
	[Token] UniqueIdentifier NOT NULL,
	[Url] NVarChar(500) NOT NULL,
 CONSTRAINT [PK_Attachment] PRIMARY KEY CLUSTERED (
	[Id] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[Bid](
	[Id] BigInt IDENTITY(1,1) NOT NULL,
	[DateCreated] DateTime NOT NULL,
	[HoursOfEffort] Int NOT NULL,
	[Invited] Bit NOT NULL,
	[MaxQuote] Int,
	[Message] NText,
	[MinQuote] Int,
	[Status] NVarChar(50) NOT NULL,
	[Timeframe] NVarChar(50) NOT NULL,
	[Company] BigInt NOT NULL,
	[Project] BigInt NOT NULL,
 CONSTRAINT [PK_Bid] PRIMARY KEY CLUSTERED (
	[Id] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[Company](
	[Id] BigInt IDENTITY(1,1) NOT NULL,
	[City] NVarChar(50),
	[Country] NVarChar(100) NOT NULL,
	[Currency] NVarChar(50),
	[Description] NText,
	[IsVerified] Bit NOT NULL,
	[Logo] NVarChar(250),
	[MinimumRate] Int,
	[Name] NVarChar(200) NOT NULL,
	[Style] NVarChar(50) NOT NULL,
	[Type] NVarChar(50) NOT NULL,
	[Website] NVarChar(200),
	[Account] BigInt NOT NULL,
 CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED (
	[Id] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[Consultant](
	[Id] BigInt IDENTITY(1,1) NOT NULL,
	[Designation] NVarChar(100),
	[LinkedinProfile] NVarChar(500) NOT NULL,
	[Name] NVarChar(50) NOT NULL,
	[Picture] NVarChar(250) NOT NULL,
	[Company] BigInt NOT NULL,
 CONSTRAINT [PK_Consultant] PRIMARY KEY CLUSTERED (
	[Id] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[Credential](
	[Id] BigInt IDENTITY(1,1) NOT NULL,
	[Link] NVarChar(500) NOT NULL,
	[Type] NVarChar(50) NOT NULL,
	[Consultant] BigInt NOT NULL,
 CONSTRAINT [PK_Credential] PRIMARY KEY CLUSTERED (
	[Id] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[Employer](
	[Id] BigInt IDENTITY(1,1) NOT NULL,
	[City] NVarChar(50) NOT NULL,
	[Country] NVarChar(100) NOT NULL,
	[IsVerified] Bit NOT NULL,
	[Name] NVarChar(50) NOT NULL,
	[Picture] NVarChar(50),
	[Account] BigInt NOT NULL,
 CONSTRAINT [PK_Employer] PRIMARY KEY CLUSTERED (
	[Id] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[EmployerFeedback](
	[Id] BigInt IDENTITY(1,1) NOT NULL,
	[Rating] NVarChar(50) NOT NULL,
	[Text] NVarChar(50) NOT NULL,
	[Company] BigInt NOT NULL,
	[Employer] BigInt NOT NULL,
	[Project] BigInt NOT NULL,
 CONSTRAINT [PK_EmployerFeedback] PRIMARY KEY CLUSTERED (
	[Id] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[PortfolioEntry](
	[Id] BigInt IDENTITY(1,1) NOT NULL,
	[Content] NText,
	[Description] NText NOT NULL,
	[Image] NVarChar(500) NOT NULL,
	[Title] NVarChar(50) NOT NULL,
	[Type] NVarChar(50) NOT NULL,
	[Company] BigInt NOT NULL,
	[Project] BigInt,
 CONSTRAINT [PK_PortfolioEntry] PRIMARY KEY CLUSTERED (
	[Id] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[ProfileFeedback](
	[Id] BigInt IDENTITY(1,1) NOT NULL,
	[Rating] NVarChar(50) NOT NULL,
	[Text] NVarChar(50) NOT NULL,
	[Company] BigInt NOT NULL,
	[Employer] BigInt NOT NULL,
	[Project] BigInt NOT NULL,
 CONSTRAINT [PK_ProfileFeedback] PRIMARY KEY CLUSTERED (
	[Id] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[Project](
	[Id] BigInt IDENTITY(1,1) NOT NULL,
	[Budget] Int NOT NULL,
	[ClosingDate] DateTime NOT NULL,
	[Currency] NVarChar(50) NOT NULL,
	[DateAdded] DateTime NOT NULL,
	[Description] NText NOT NULL,
	[DescriptionText] NText NOT NULL,
	[Status] NVarChar(50) NOT NULL,
	[Title] NVarChar(100) NOT NULL,
	[Employer] BigInt NOT NULL,
 CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED (
	[Id] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[ProjectInvite](
	[Id] BigInt IDENTITY(1,1) NOT NULL,
	[Status] NVarChar(50) NOT NULL,
	[Company] BigInt NOT NULL,
	[Project] BigInt NOT NULL,
 CONSTRAINT [PK_ProjectInvite] PRIMARY KEY CLUSTERED (
	[Id] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[Tag](
	[Id] BigInt IDENTITY(1,1) NOT NULL,
	[Name] NVarChar(50) NOT NULL,
	[Slug] NVarChar(50) NOT NULL,
 CONSTRAINT [PK_Tag] PRIMARY KEY CLUSTERED (
	[Id] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[Token](
	[Id] BigInt IDENTITY(1,1) NOT NULL,
	[CreatedDate] DateTime NOT NULL,
	[Data] NVarChar(1000) NOT NULL,
	[Key] NVarChar(50) NOT NULL,
	[Type] NVarChar(50) NOT NULL,
 CONSTRAINT [PK_Token] PRIMARY KEY CLUSTERED (
	[Id] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[ProjectAttachmentMap](
	[Attachment] BigInt NOT NULL,
	[Project] BigInt NOT NULL,
 CONSTRAINT [PK_ProjectAttachmentMap] PRIMARY KEY CLUSTERED (
	[Attachment] ASC,
	[Project] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[ProjectTagMap](
	[Project] BigInt NOT NULL,
	[Tag] BigInt NOT NULL,
 CONSTRAINT [PK_ProjectTagMap] PRIMARY KEY CLUSTERED (
	[Project] ASC,
	[Tag] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[TagCompanyMap](
	[Company] BigInt NOT NULL,
	[Tag] BigInt NOT NULL,
 CONSTRAINT [PK_TagCompanyMap] PRIMARY KEY CLUSTERED (
	[Company] ASC,
	[Tag] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[AccountAlert]  WITH CHECK ADD  CONSTRAINT [FK_AccountAlert_Account_Account_Alerts] FOREIGN KEY([Account])
REFERENCES [dbo].[Account] ([Id])
GO

ALTER TABLE [dbo].[AccountAlert] CHECK CONSTRAINT [FK_AccountAlert_Account_Account_Alerts]
GO

ALTER TABLE [dbo].[Employer]  WITH CHECK ADD  CONSTRAINT [FK_Employer_Account_Account_Employer_OneToOne_OneToOne] FOREIGN KEY([Account])
REFERENCES [dbo].[Account] ([Id])
GO

ALTER TABLE [dbo].[Employer] CHECK CONSTRAINT [FK_Employer_Account_Account_Employer_OneToOne_OneToOne]
GO

ALTER TABLE [dbo].[Company]  WITH CHECK ADD  CONSTRAINT [FK_Company_Account_Account_Company_OneToOne_OneToOne] FOREIGN KEY([Account])
REFERENCES [dbo].[Account] ([Id])
GO

ALTER TABLE [dbo].[Company] CHECK CONSTRAINT [FK_Company_Account_Account_Company_OneToOne_OneToOne]
GO

ALTER TABLE [dbo].[ProjectInvite]  WITH CHECK ADD  CONSTRAINT [FK_ProjectInvite_Company_Company_ProjectInvites] FOREIGN KEY([Company])
REFERENCES [dbo].[Company] ([Id])
GO

ALTER TABLE [dbo].[ProjectInvite] CHECK CONSTRAINT [FK_ProjectInvite_Company_Company_ProjectInvites]
GO

ALTER TABLE [dbo].[Consultant]  WITH CHECK ADD  CONSTRAINT [FK_Consultant_Company_Company_Consultants] FOREIGN KEY([Company])
REFERENCES [dbo].[Company] ([Id])
GO

ALTER TABLE [dbo].[Consultant] CHECK CONSTRAINT [FK_Consultant_Company_Company_Consultants]
GO

ALTER TABLE [dbo].[ProfileFeedback]  WITH CHECK ADD  CONSTRAINT [FK_ProfileFeedback_Company_Company_FeedbackGiven] FOREIGN KEY([Company])
REFERENCES [dbo].[Company] ([Id])
GO

ALTER TABLE [dbo].[ProfileFeedback] CHECK CONSTRAINT [FK_ProfileFeedback_Company_Company_FeedbackGiven]
GO

ALTER TABLE [dbo].[EmployerFeedback]  WITH CHECK ADD  CONSTRAINT [FK_EmployerFeedback_Company_Company_FeedbackReceived] FOREIGN KEY([Company])
REFERENCES [dbo].[Company] ([Id])
GO

ALTER TABLE [dbo].[EmployerFeedback] CHECK CONSTRAINT [FK_EmployerFeedback_Company_Company_FeedbackReceived]
GO

ALTER TABLE [dbo].[Bid]  WITH CHECK ADD  CONSTRAINT [FK_Bid_Company_Company_Bids] FOREIGN KEY([Company])
REFERENCES [dbo].[Company] ([Id])
GO

ALTER TABLE [dbo].[Bid] CHECK CONSTRAINT [FK_Bid_Company_Company_Bids]
GO

ALTER TABLE [dbo].[PortfolioEntry]  WITH CHECK ADD  CONSTRAINT [FK_PortfolioEntry_Company_Company_Portfolio] FOREIGN KEY([Company])
REFERENCES [dbo].[Company] ([Id])
GO

ALTER TABLE [dbo].[PortfolioEntry] CHECK CONSTRAINT [FK_PortfolioEntry_Company_Company_Portfolio]
GO

ALTER TABLE [dbo].[Credential]  WITH CHECK ADD  CONSTRAINT [FK_Credential_Consultant_Consultant_Credentials] FOREIGN KEY([Consultant])
REFERENCES [dbo].[Consultant] ([Id])
GO

ALTER TABLE [dbo].[Credential] CHECK CONSTRAINT [FK_Credential_Consultant_Consultant_Credentials]
GO

ALTER TABLE [dbo].[Project]  WITH CHECK ADD  CONSTRAINT [FK_Project_Employer_Employer_Projects] FOREIGN KEY([Employer])
REFERENCES [dbo].[Employer] ([Id])
GO

ALTER TABLE [dbo].[Project] CHECK CONSTRAINT [FK_Project_Employer_Employer_Projects]
GO

ALTER TABLE [dbo].[EmployerFeedback]  WITH CHECK ADD  CONSTRAINT [FK_EmployerFeedback_Employer_Employer_FeedbackGiven] FOREIGN KEY([Employer])
REFERENCES [dbo].[Employer] ([Id])
GO

ALTER TABLE [dbo].[EmployerFeedback] CHECK CONSTRAINT [FK_EmployerFeedback_Employer_Employer_FeedbackGiven]
GO

ALTER TABLE [dbo].[ProfileFeedback]  WITH CHECK ADD  CONSTRAINT [FK_ProfileFeedback_Employer_Employer_FeedbackReceived] FOREIGN KEY([Employer])
REFERENCES [dbo].[Employer] ([Id])
GO

ALTER TABLE [dbo].[ProfileFeedback] CHECK CONSTRAINT [FK_ProfileFeedback_Employer_Employer_FeedbackReceived]
GO

ALTER TABLE [dbo].[ProjectAttachmentMap]  WITH CHECK ADD  CONSTRAINT [FK_ProjectAttachmentMap_Attachment_Project_Attachments] FOREIGN KEY([Attachment])
REFERENCES [dbo].[Project] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ProjectAttachmentMap] CHECK CONSTRAINT [FK_ProjectAttachmentMap_Attachment_Project_Attachments]
GO

ALTER TABLE [dbo].[ProjectAttachmentMap]  WITH CHECK ADD  CONSTRAINT [FK_ProjectAttachmentMap_Project_Attachment_Projects] FOREIGN KEY([Project])
REFERENCES [dbo].[Attachment] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ProjectAttachmentMap] CHECK CONSTRAINT [FK_ProjectAttachmentMap_Project_Attachment_Projects]
GO

ALTER TABLE [dbo].[EmployerFeedback]  WITH CHECK ADD  CONSTRAINT [FK_EmployerFeedback_Project_Project_EmployerFeedback_OneToOne_OneToOne] FOREIGN KEY([Project])
REFERENCES [dbo].[Project] ([Id])
GO

ALTER TABLE [dbo].[EmployerFeedback] CHECK CONSTRAINT [FK_EmployerFeedback_Project_Project_EmployerFeedback_OneToOne_OneToOne]
GO

ALTER TABLE [dbo].[ProfileFeedback]  WITH CHECK ADD  CONSTRAINT [FK_ProfileFeedback_Project_Project_ProfileFeedback_OneToOne_OneToOne] FOREIGN KEY([Project])
REFERENCES [dbo].[Project] ([Id])
GO

ALTER TABLE [dbo].[ProfileFeedback] CHECK CONSTRAINT [FK_ProfileFeedback_Project_Project_ProfileFeedback_OneToOne_OneToOne]
GO

ALTER TABLE [dbo].[ProjectInvite]  WITH CHECK ADD  CONSTRAINT [FK_ProjectInvite_Project_Project_ProjectInvites] FOREIGN KEY([Project])
REFERENCES [dbo].[Project] ([Id])
GO

ALTER TABLE [dbo].[ProjectInvite] CHECK CONSTRAINT [FK_ProjectInvite_Project_Project_ProjectInvites]
GO

ALTER TABLE [dbo].[Bid]  WITH CHECK ADD  CONSTRAINT [FK_Bid_Project_Project_Bids] FOREIGN KEY([Project])
REFERENCES [dbo].[Project] ([Id])
GO

ALTER TABLE [dbo].[Bid] CHECK CONSTRAINT [FK_Bid_Project_Project_Bids]
GO

ALTER TABLE [dbo].[PortfolioEntry]  WITH CHECK ADD  CONSTRAINT [FK_PortfolioEntry_Project_Project_PortfolioEntry_ZeroOneToZeroOne_ZeroOneToZeroOne] FOREIGN KEY([Project])
REFERENCES [dbo].[Project] ([Id])
GO

ALTER TABLE [dbo].[PortfolioEntry] CHECK CONSTRAINT [FK_PortfolioEntry_Project_Project_PortfolioEntry_ZeroOneToZeroOne_ZeroOneToZeroOne]
GO

ALTER TABLE [dbo].[ProjectTagMap]  WITH CHECK ADD  CONSTRAINT [FK_ProjectTagMap_Project_Tag_Projects] FOREIGN KEY([Project])
REFERENCES [dbo].[Tag] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ProjectTagMap] CHECK CONSTRAINT [FK_ProjectTagMap_Project_Tag_Projects]
GO

ALTER TABLE [dbo].[ProjectTagMap]  WITH CHECK ADD  CONSTRAINT [FK_ProjectTagMap_Tag_Project_Tags] FOREIGN KEY([Tag])
REFERENCES [dbo].[Project] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ProjectTagMap] CHECK CONSTRAINT [FK_ProjectTagMap_Tag_Project_Tags]
GO

ALTER TABLE [dbo].[TagCompanyMap]  WITH CHECK ADD  CONSTRAINT [FK_TagCompanyMap_Company_Tag_Companies] FOREIGN KEY([Company])
REFERENCES [dbo].[Tag] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[TagCompanyMap] CHECK CONSTRAINT [FK_TagCompanyMap_Company_Tag_Companies]
GO

ALTER TABLE [dbo].[TagCompanyMap]  WITH CHECK ADD  CONSTRAINT [FK_TagCompanyMap_Tag_Company_Tags] FOREIGN KEY([Tag])
REFERENCES [dbo].[Company] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[TagCompanyMap] CHECK CONSTRAINT [FK_TagCompanyMap_Tag_Company_Tags]
GO

