create table Schedule(
	Id int primary key identity,
	ScheduleTime dateTime not null,
	UserId NVARCHAR (450)  NOT NULL,
	Description  NVARCHAR (max) ,
	FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) on delete cascade
);


create index index_UserId on Schedule(Id);