-- ****************** SqlDBM: Microsoft SQL Server ******************
-- ******************************************************************

DROP TABLE [sessions];
GO


DROP TABLE [deck_tag];
GO


DROP TABLE [card_deck];
GO


DROP TABLE [tags];
GO


DROP TABLE [decks];
GO


DROP TABLE [cards];
GO


DROP TABLE [users];
GO


--************************************** [users]

CREATE TABLE [users]
(
 [UserID]      INT IDENTITY (1, 1) NOT NULL ,
 [Email]       NCHAR(50) NOT NULL ,
 [Password]    NCHAR(50) NOT NULL ,
 [DisplayName] NCHAR(50) NOT NULL ,

 CONSTRAINT [PK_users] PRIMARY KEY CLUSTERED ([UserID] ASC)
);
GO



--************************************** [tags]

CREATE TABLE [tags]
(
 [TagID]   INT IDENTITY (1, 1) NOT NULL ,
 [TagName] NCHAR(50) NOT NULL ,
 [UserID]  INT NOT NULL ,

 CONSTRAINT [PK_tags] PRIMARY KEY CLUSTERED ([TagID] ASC),
 CONSTRAINT [FK_109] FOREIGN KEY ([UserID])
  REFERENCES [users]([UserID])
);
GO


--SKIP Index: [fkIdx_109]


--************************************** [decks]

CREATE TABLE [decks]
(
 [DeckID]   INT IDENTITY (1, 1) NOT NULL ,
 [DeckName] NCHAR(50) NOT NULL ,
 [IsPublic] BIT NOT NULL ,
 [UserID]   INT NOT NULL ,

 CONSTRAINT [PK_decks] PRIMARY KEY CLUSTERED ([DeckID] ASC),
 CONSTRAINT [FK_129] FOREIGN KEY ([UserID])
  REFERENCES [users]([UserID])
);
GO


--SKIP Index: [fkIdx_129]


--************************************** [cards]

CREATE TABLE [cards]
(
 [CardID] INT IDENTITY (1, 1) NOT NULL ,
 [Front]  NCHAR(1000) NOT NULL ,
 [Back]   NCHAR(1000) NOT NULL ,
 [UserID] INT NOT NULL ,

 CONSTRAINT [PK_cards] PRIMARY KEY CLUSTERED ([CardID] ASC),
 CONSTRAINT [FK_125] FOREIGN KEY ([UserID])
  REFERENCES [users]([UserID])
);
GO


--SKIP Index: [fkIdx_125]


--************************************** [sessions]

CREATE TABLE [sessions]
(
 [SessionID]     INT IDENTITY (1, 1) NOT NULL ,
 [UserID]        INT NOT NULL ,
 [DeckID]        INT NOT NULL ,
 [TotalScore]    INT NOT NULL ,
 [PossibleScore] INT NOT NULL ,
 [TimeOf]        DATETIME2(7) NOT NULL ,

 CONSTRAINT [PK_sessions] PRIMARY KEY CLUSTERED ([SessionID] ASC),
 CONSTRAINT [FK_137] FOREIGN KEY ([UserID])
  REFERENCES [users]([UserID]),
 CONSTRAINT [FK_141] FOREIGN KEY ([DeckID])
  REFERENCES [decks]([DeckID])
);
GO


--SKIP Index: [fkIdx_137]

--SKIP Index: [fkIdx_141]


--************************************** [deck_tag]

CREATE TABLE [deck_tag]
(
 [DeckID] INT NOT NULL ,
 [TagID]  INT NOT NULL ,

 CONSTRAINT [FK_115] FOREIGN KEY ([DeckID])
  REFERENCES [decks]([DeckID]),
 CONSTRAINT [FK_119] FOREIGN KEY ([TagID])
  REFERENCES [tags]([TagID])
);
GO


--SKIP Index: [fkIdx_115]

--SKIP Index: [fkIdx_119]


--************************************** [card_deck]

CREATE TABLE [card_deck]
(
 [CardID] INT NOT NULL ,
 [DeckID] INT NOT NULL ,

 CONSTRAINT [FK_47] FOREIGN KEY ([CardID])
  REFERENCES [cards]([CardID]),
 CONSTRAINT [FK_51] FOREIGN KEY ([DeckID])
  REFERENCES [decks]([DeckID])
);
GO


--SKIP Index: [fkIdx_47]

--SKIP Index: [fkIdx_51]


