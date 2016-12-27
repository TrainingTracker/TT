USE `TrainingTracker`;
ALTER TABLE `Team`
DROP FOREIGN KEY `Fk_UserId_TeamManager`;
USE `TrainingTracker`;
ALTER TABLE `Team`
DROP COLUMN `TeamManager`
