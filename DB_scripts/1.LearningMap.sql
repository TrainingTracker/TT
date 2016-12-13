CREATE TABLE LearningMap
(
 Id INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
 Title NVARCHAR(50) NOT NULL,
 Notes LONGTEXT NOT NULL,
 Duration INT NOT NULL,
 IsCourseRestricted BIT NOT NULL,
 TeamId INT NOT NULL,
 IsDeleted BIT NOT NULL,
 CreatedBy INT NOT NULL,
 DateCreated DATETIME NOT NULL

);

ALTER TABLE LearningMap
ADD CONSTRAINT FK_LearningMap_Team FOREIGN KEY(TeamId)
          REFERENCES Team(TeamId);

ALTER TABLE LearningMap
ADD CONSTRAINT FK_LearningMap_User FOREIGN KEY(CreatedBy)
          REFERENCES `User`(UserId);

CREATE TABLE LearningMapCourseMappping
(
 Id INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
 LearningMapId INT NOT NULL,
 CourseId INT NOT NULL,
 SortOrder INT NOT NULL,
 IsDeleted BIT NOT NULL,
 DateInserted DATETIME NOT NULL
);

ALTER TABLE LearningMapCourseMappping
ADD CONSTRAINT FK_LearningMapCourseMappping_LearningMap FOREIGN KEY(LearningMapId)
              REFERENCES LearningMap(Id);

ALTER TABLE LearningMapCourseMappping
ADD CONSTRAINT FK_LearningMapCourseMappping_Course FOREIGN KEY(CourseId)
               REFERENCES Course(Id);

CREATE TABLE LearningMapUserMappping
(
 Id INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
 LearningMapId INT NOT NULL,
 UserId INT NOT NULL,
 DateInserted DATETIME NOT NULL
);

ALTER TABLE LearningMapUserMappping
ADD CONSTRAINT FK_LearningMapUserMappping_LearningMap FOREIGN KEY(LearningMapId)
  REFERENCES LearningMap(Id);

ALTER TABLE LearningMapUserMappping
ADD CONSTRAINT FK_LearningMapUserMappping_User FOREIGN KEY(UserId)
  REFERENCES `User`(UserId);