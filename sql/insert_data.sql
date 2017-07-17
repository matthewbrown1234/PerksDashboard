-----Create Data----
begin transaction
	USE [perksdb]
	GO  
	ALTER TABLE dbo.activity  
	NOCHECK CONSTRAINT FK_ActivityType;
	ALTER TABLE dbo.activity  
	NOCHECK CONSTRAINT FK_ActivityHandle;
	ALTER TABLE dbo.sales  
	NOCHECK CONSTRAINT FK_SalesActivity;
	ALTER TABLE dbo.recognition  
	NOCHECK CONSTRAINT FK_RecognitionActivity;
	ALTER TABLE dbo.recognition  
	NOCHECK CONSTRAINT FK_RecognitionReasonId;
	GO
	INSERT INTO dbo.activity_type (type, description) VALUES
		('r','Recognition activity Type.'),
		('s','Sale activity Type.')
	INSERT INTO dbo.handle (id, name) VALUES
		(1,'gwashington'),
		(2,'bobama'),
		(3,'bclinton'),
		(4,'rnixon'),
		(5,'jcarter')
	INSERT INTO dbo.recognition_reason (id, reason) VALUES
		(1, 'Going above and beyond'),
		(2, 'Creative ideas'),
		(3, 'Cost savings'),
		(4, 'Teamwork'),
		(5, 'Other')
	INSERT INTO dbo.activity (id, type, handle_id, description, date_time) VALUES
		(1,'s', 1,'Tea was sold to some villagers in Boston.', SYSDATETIMEOFFSET()),
		(2,'s', 2,'Tacos were sold to the Canadians.', SYSDATETIMEOFFSET()),
		(3,'r', 3,'Recognized for having a great picture between two Bushes.', SYSDATETIMEOFFSET()),
		(4,'r', 4,'Recognized for having an interesting face.', SYSDATETIMEOFFSET()),
		(5,'s', 5,'Sold twice as many tacos to the Canadians as Obama.', SYSDATETIMEOFFSET()),
		(6,'r', 1,'Recognized for bringing together Obama and Carter for their taco selling rivalry.', SYSDATETIMEOFFSET()),
		(7,'s', 2,'More Tea was sold to some villagers in Boston.', SYSDATETIMEOFFSET()),
		(8,'r', 2,'Recognized for best hand gestures during speeches.', SYSDATETIMEOFFSET()),
		(9,'s', 3,'Sold some firecrackes to children.', SYSDATETIMEOFFSET()),
		(10,'s', 4,'Sold some signed headshots.', SYSDATETIMEOFFSET()),
		(11,'s', 5,'Sold 2 boxes of sox.', SYSDATETIMEOFFSET()),
		(12,'s', 1,'Sold some signed headshots.', SYSDATETIMEOFFSET())
	INSERT INTO dbo.sales (id, invoice_id, activity_id) VALUES
		(1, '111', 1),
		(2, 'asdf1234', 2),
		(3, 'aaaa', 5),
		(4, 'abc', 7),
		(5, 'bbb', 9),
		(6, '2', 10),
		(7, '3', 11),
		(8, '40', 12)
	INSERT INTO dbo.recognition (id, activity_id, reason_id) VALUES
		(1, 3, 3),
		(2, 4, 5),
		(3, 6, 4),
		(4,8,1)
	GO
	ALTER TABLE dbo.activity  
	CHECK CONSTRAINT FK_ActivityType;
	ALTER TABLE dbo.activity  
	CHECK CONSTRAINT FK_ActivityHandle;
	ALTER TABLE dbo.sales  
	CHECK CONSTRAINT FK_SalesActivity;
	ALTER TABLE dbo.recognition  
	CHECK CONSTRAINT FK_RecognitionActivity;
	ALTER TABLE dbo.recognition  
	CHECK CONSTRAINT FK_RecognitionReasonId;
	GO
commit