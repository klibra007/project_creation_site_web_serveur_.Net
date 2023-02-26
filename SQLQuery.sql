CREATE DATABASE QuizExamen;

USE QuizExamen;
GO

CREATE TABLE Category(
categoryID int PRIMARY KEY IDENTITY(1,1),
description varchar(50),
);

CREATE TABLE Question(
questionID int PRIMARY KEY IDENTITY(1,1),
text varchar(255),
categoryID int FOREIGN KEY REFERENCES Category(categoryID)
);


CREATE TABLE ItemOption(  -- Option
optionID int PRIMARY KEY IDENTITY(1,1),
text varchar(255),
isRight bit not null,
questionID int FOREIGN KEY REFERENCES Question(questionID)
);


CREATE TABLE Quiz(
quizID int PRIMARY KEY IDENTITY(1,1),
userName varchar(50),
email varchar(50)
);


CREATE TABLE Answer(
answerID int PRIMARY KEY IDENTITY(1,1),
optionID int FOREIGN KEY REFERENCES ItemOption(optionID),
quizID int FOREIGN KEY REFERENCES Quiz(quizID)
);


CREATE TABLE QuestionQuiz(
questionID int,
quizID int,
PRIMARY KEY (questionID, quizID),
FOREIGN KEY (questionID) REFERENCES Question(questionID),
FOREIGN KEY (quizID) REFERENCES Quiz(quizID)
);




/****** peupler la base par quelques données  ******/

insert into Category (description) 
values ('easy'),
('medium'),
('hard');



insert into Question (text,categoryID) 
values ('Java is ……..', 1),
('A Java class', 2),
('What is Java inheritance?', 2),
('Polymorphism is the ability of an object to take on many forms.', 3),
('Local variables are declared in methods, constructors, or blocks.', 1),
('…….. stores a fixed-size sequential collection of elements of the same type?', 2)



insert into ItemOption (text,isRight,questionID) 
values ('a coffee', 0, 1),
('a high-level programming language', 1, 1),
('a source code editor', 0, 1),
('is a template that describes the behavior that the object of its type support', 0, 2),
('can have any number of methods', 1, 2),
('the process where one class acquires the properties (methods and fields) of another.', 1, 3),
('a problem that arises during the execution of a program.', 0, 3),
('it mainly used to traverse collection of elements including arrays.', 0, 3),
('true', 1, 4),
('false', 0, 4),
('true', 1, 5),
('false', 0, 5),
('variables', 0, 6),
('arrays', 1, 6),
('methods', 0, 6)



insert into Quiz (userName,email) 
values ('williamo', 'william@gmail.com'), 
('allo21', 'alex.gh@gmail.com')


insert into Answer (optionID,quizID) 
values (1, 1),(5, 1), (9, 1), (14, 1), (1, 2), (7, 2), (9, 2), (11, 2)


insert into QuestionQuiz (questionID,quizID) 
values (1, 1), (4, 1), (6, 1), (1, 2), (3, 2), (4, 2), (5, 2)
