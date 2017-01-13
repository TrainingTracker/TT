

INSERT INTO CourseUserMapping(UserId,StartedOn,CourseId)

SELECT * FROM 
(SELECT A.UserId,A.CreatedOn,D.Id AS CourseId FROM
(select SubtopicContentId,UserId,CreatedOn FROM  SubtopicContentUserMap 
ORDER BY CreatedOn
) AS A
LEFT JOIN SUbTopicContent B ON B.Id=A.SubtopicContentId
LEFT JOIN CourseSubTopic C ON C.Id=B.CourseSubtopicId
LEFT JOIN Course D ON D.Id=C.CourseId
Group BY A.USERId,d.Id ) AS E
ORDER BY CreatedOn

select * from CourseUserMapping
