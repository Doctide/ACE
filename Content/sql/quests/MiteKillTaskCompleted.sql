DELETE FROM `quest` WHERE `name` = 'MiteKillTaskCompleted';

INSERT INTO `quest` (`name`, `min_Delta`, `max_Solves`, `message`, `last_Modified`)
VALUES ('MiteKillTaskCompleted', 72000, -1, 'Rabid Mite Kill Task', '2021-11-01 00:00:00');
