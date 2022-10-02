create database QuizApp

use QuizApp

create table tbl_admin
(
	ad_id int primary key identity,
	ad_name nvarchar(20) not null unique,
	ad_pass nvarchar(20) not null
)

create table tbl_questions
(
	q_id int primary key identity,
	q_text nvarchar(max) not null,
	QA nvarchar(20) not null unique,

	QB nvarchar(20) not null unique,
	QC nvarchar(20) not null unique,
	QD nvarchar(20) not null unique,
	QCorrectAns nvarchar(20) not null,
)

create table student
(
	std_id int primary key identity,
	std_name nvarchar(20) not null unique,
	std_password nvarchar(20) not null,
	std_image nvarchar(max) not null ,
)


create table tbl_setExam
(
	examId int primary key identity,
	exam_date datetime,
	exam_fk_stu int foreign key references student(std_id),
	exam_name nvarchar(20) not null,
	std_score int
)

select *from sys.tables

insert into tbl_admin (ad_name, ad_pass)
values ('trungnghia', 'abc123')

select *from tbl_admin

create table tbl_category
(
	cat_id int primary key identity,
	cat_name nvarchar(50) not null,
	cat_fk_ad_id int foreign key references tbl_admin(ad_id)
)

alter table tbl_questions 
add q_fk_catid int foreign key references tbl_category (cat_id)

select *from tbl_questions

select *from tbl_category


select *from student
insert into student(std_name, std_password)
values ('guest', 'abc123')