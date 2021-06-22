create database Parking;

use Parking;

create table Users (
id int Identity(1,1) primary key,
[login] varchar(50)
)

create table [Types] (
id int Identity(1,1) primary key,
[name] varchar(30)
)

create table Automobiles (
id int Identity(1,1) primary key,
id_type int references Types(id) on delete cascade,
id_user int references Users(id) on delete cascade,
number varchar(8),
height int,
width int,
[length] int
)

create table Positions (
id int Identity(1,1) primary key,
id_type int references Types(id) on delete cascade,
number int unique,
[status] bit default(0)
)

create table UsersPositions (
id int Identity(1,1) primary key,
id_pos int references Positions(id) on delete cascade,
id_auto int references Automobiles(id)
)
