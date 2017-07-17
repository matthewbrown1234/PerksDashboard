--CREATE LOGIN perks
USE [master]
If EXISTS (select loginname from master.dbo.syslogins where name = 'perks')
DROP LOGIN [perks]
GO
CREATE LOGIN [perks] WITH PASSWORD=N'perks', DEFAULT_DATABASE=[master], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO
--CREATE DATABASE perksdb
IF EXISTS(select * from sys.databases where name='perksdb')
DROP DATABASE perksdb
GO
CREATE DATABASE perksdb
GO
--CREATE USER perks
IF EXISTS (SELECT * FROM sys.database_principals WHERE name = 'perks')
DROP USER perks
GO
USE [perksdb]
GO
CREATE USER [perks] FOR LOGIN [perks]
GO
--GRANT USER perks permissions
USE [perksdb]
GO
ALTER ROLE [db_datareader] ADD MEMBER [perks]
GO
USE [perksdb]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [perks]
GO
USE [perksdb]
GO
ALTER ROLE [db_ddladmin] ADD MEMBER [perks]
GO
-----------------------------------------------------
-----------------CREATE TABLES-----------------------
USE [perksdb]
GO
IF  NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'handle') AND type in (N'U'))
BEGIN
CREATE TABLE handle(
	id int not null,
	name varchar(24) not null,
	CONSTRAINT PK_Handle PRIMARY KEY (id)
)
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'activity_type') AND type in (N'U'))
BEGIN
CREATE TABLE activity_type(
	type char not null Primary key,
	description varchar(126) not null,
)
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'activity') AND type in (N'U'))
BEGIN
CREATE TABLE activity(
	id int not null Primary key,
	type char not null,
	handle_id int not null,
	description varchar(256),
	date_time datetimeoffset not null,
	verified bit DEFAULT 0 not null,
	CONSTRAINT FK_ActivityHandle FOREIGN KEY (handle_id) REFERENCES handle(id),
	CONSTRAINT FK_ActivityType FOREIGN KEY (type) REFERENCES activity_type(type),
)
END
GO
IF  NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'sales') AND type in (N'U'))
BEGIN
CREATE TABLE sales(
	id int not null,
	invoice_id varchar(256) not null,
	activity_id int not null,
    CONSTRAINT PK_Sales PRIMARY KEY (id, invoice_id),
	CONSTRAINT FK_SalesActivity FOREIGN KEY (activity_id) REFERENCES activity(id)
)
END
GO
IF  NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'recognition_reason') AND type in (N'U'))
BEGIN
CREATE TABLE recognition_reason(
	id int not null Primary key,
	reason varchar(64) not null
)
END
GO
IF  NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'recognition') AND type in (N'U'))
BEGIN
CREATE TABLE recognition(
	id int not null Primary key,
	activity_id int not null,
	reason_id int not null,
	CONSTRAINT FK_RecognitionActivity FOREIGN KEY (activity_id) REFERENCES activity(id),
	CONSTRAINT FK_RecognitionReasonId FOREIGN KEY (reason_id) REFERENCES recognition_reason(id)
)
END
GO
-----------------------------------------------------