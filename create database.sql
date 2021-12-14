use information_schema;

drop database IF EXISTS NyitottKapukRegisztracio ;

Create database NyitottKapukRegisztracio;

use NyitottKapukRegisztracio;

CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(95) NOT NULL,
    `ProductVersion` varchar(32) NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
);

CREATE TABLE `Day` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Date` datetime(6) NOT NULL,
    `MaxVisitors` int NOT NULL,
    CONSTRAINT `PK_Day` PRIMARY KEY (`Id`)
);

CREATE TABLE `VisitorGroup` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `GroupNumber` int NOT NULL,
    `ClassroomNumber` longtext CHARACTER SET utf8mb4 NOT NULL,
    `DayId` int NULL,
    CONSTRAINT `PK_VisitorGroup` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_VisitorGroup_Day_DayId` FOREIGN KEY (`DayId`) REFERENCES `Day` (`Id`) ON DELETE RESTRICT
);

CREATE TABLE `Registrations` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `DayId` int NOT NULL,
    `VisitorGroupId` int NULL,
    `Email` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Password` longtext CHARACTER SET utf8mb4 NOT NULL,
    `ParentName1` longtext CHARACTER SET utf8mb4 NULL,
    `ParentName2` longtext CHARACTER SET utf8mb4 NULL,
    `StudentName1` longtext CHARACTER SET utf8mb4 NULL,
    `StudentName2` longtext CHARACTER SET utf8mb4 NULL,
    `StudentName3` longtext CHARACTER SET utf8mb4 NULL,
    `StudentName4` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_Registrations` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Registrations_Day_DayId` FOREIGN KEY (`DayId`) REFERENCES `Day` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Registrations_VisitorGroup_VisitorGroupId` FOREIGN KEY (`VisitorGroupId`) REFERENCES `VisitorGroup` (`Id`) ON DELETE RESTRICT
);

INSERT INTO `Day` (`Id`, `Date`, `MaxVisitors`)
VALUES (1, '2022-05-10 00:00:00', 192);

INSERT INTO `VisitorGroup` (`Id`, `ClassroomNumber`, `DayId`, `GroupNumber`)
VALUES (1, '110', NULL, 1);
INSERT INTO `VisitorGroup` (`Id`, `ClassroomNumber`, `DayId`, `GroupNumber`)
VALUES (2, '110', NULL, 2);
INSERT INTO `VisitorGroup` (`Id`, `ClassroomNumber`, `DayId`, `GroupNumber`)
VALUES (3, '110', NULL, 3);
INSERT INTO `VisitorGroup` (`Id`, `ClassroomNumber`, `DayId`, `GroupNumber`)
VALUES (4, '110', NULL, 4);
INSERT INTO `VisitorGroup` (`Id`, `ClassroomNumber`, `DayId`, `GroupNumber`)
VALUES (5, '106', NULL, 5);
INSERT INTO `VisitorGroup` (`Id`, `ClassroomNumber`, `DayId`, `GroupNumber`)
VALUES (6, '112', NULL, 6);
INSERT INTO `VisitorGroup` (`Id`, `ClassroomNumber`, `DayId`, `GroupNumber`)
VALUES (7, '205', NULL, 7);
INSERT INTO `VisitorGroup` (`Id`, `ClassroomNumber`, `DayId`, `GroupNumber`)
VALUES (8, '207', NULL, 8);
INSERT INTO `VisitorGroup` (`Id`, `ClassroomNumber`, `DayId`, `GroupNumber`)
VALUES (9, '208', NULL, 9);
INSERT INTO `VisitorGroup` (`Id`, `ClassroomNumber`, `DayId`, `GroupNumber`)
VALUES (10, '209', NULL, 10);
INSERT INTO `VisitorGroup` (`Id`, `ClassroomNumber`, `DayId`, `GroupNumber`)
VALUES (11, '210', NULL, 11);
INSERT INTO `VisitorGroup` (`Id`, `ClassroomNumber`, `DayId`, `GroupNumber`)
VALUES (12, '212', NULL, 12);

CREATE INDEX `IX_Registrations_DayId` ON `Registrations` (`DayId`);

CREATE INDEX `IX_Registrations_VisitorGroupId` ON `Registrations` (`VisitorGroupId`);

CREATE INDEX `IX_VisitorGroup_DayId` ON `VisitorGroup` (`DayId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20211207091543_init', '3.1.19');

