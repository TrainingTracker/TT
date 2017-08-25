ALTER TABLE `TT_Capitalized`.`CodeReviewMetaData` 
ADD COLUMN `SystemRating` INT NULL AFTER `CreatedOn`,
ADD INDEX `FK_CodeReviewMetaData_Feedback_idx` (`SystemRating` ASC);
