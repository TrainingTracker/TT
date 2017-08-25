
USE tt_capitalized;

INSERT INTO CrRatingCalcConfig (TeamId)
SELECT TeamId FROM Team WHERE TeamId IS NOT NULL;

CREATE TEMPORARY TABLE configIds
(
Id INT
);
INSERT INTO configIds
SELECT Id FROM CrRatingCalcConfig;

CREATE TEMPORARY TABLE Weights
(
ReviewType INT,Weight float
);
INSERT INTO Weights
VALUES(1,10),(2,6),(3,5.5),(4,0.1),(5,0),(6,5.1);

CREATE TEMPORARY TABLE Ranges
(
FeedbackType INT, RangeMin float, RangeMax float
);
INSERT INTO Ranges
VALUES(1,0,5.1),(2,5.1,5.5),(3,5.5,6.1),(4,6.1,10);

INSERT INTO CrRatingCalcWeightConfig (CrRatingCalcConfigId, ReviewPointTypeId, Weight)
SELECT 
    Id,ReviewType,Weight
FROM
    configIds
       CROSS JOIN
    Weights
ORDER BY Id , ReviewType;
SELECT * FROM CrRatingCalcWeightConfig;

INSERT INTO CrRatingCalcRangeConfig (CrRatingCalcConfigId, FeedbackTypeId, RangeMin, RangeMax)
SELECT 
    Id, FeedbackType, RangeMin, RangeMax
FROM
    configIds
       CROSS JOIN
    Ranges
ORDER BY Id , FeedbackType;
SELECT 
    *
FROM
    CrRatingCalcRangeConfig;

