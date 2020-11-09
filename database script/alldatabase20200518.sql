-- MySQL dump 10.13  Distrib 8.0.19, for Win64 (x86_64)
--
-- Host: localhost    Database: football_league
-- ------------------------------------------------------
-- Server version	8.0.19

CREATE DATABASE IF NOT EXISTS football_league;
USE football_league;

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `ages`
--

DROP TABLE IF EXISTS `ages`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ages` (
  `yearOfBirth` year NOT NULL,
  PRIMARY KEY (`yearOfBirth`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ages`
--

LOCK TABLES `ages` WRITE;
/*!40000 ALTER TABLE `ages` DISABLE KEYS */;
INSERT INTO `ages` VALUES (2003),(2004),(2005),(2006),(2007),(2008),(2009),(2010);
/*!40000 ALTER TABLE `ages` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `club_teams`
--

DROP TABLE IF EXISTS `club_teams`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `club_teams` (
  `teamID` int NOT NULL AUTO_INCREMENT,
  `clubID` int NOT NULL,
  `yearOfBirth` year NOT NULL,
  PRIMARY KEY (`teamID`),
  KEY `club_team_yearOfBirth_FK_idx` (`yearOfBirth`),
  KEY `club_team_clubID_FK_idx` (`clubID`),
  CONSTRAINT `club_team_clubID_FK` FOREIGN KEY (`clubID`) REFERENCES `clubs` (`clubID`),
  CONSTRAINT `club_team_yearOfBirth_FK` FOREIGN KEY (`yearOfBirth`) REFERENCES `ages` (`yearOfBirth`)
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `club_teams`
--

LOCK TABLES `club_teams` WRITE;
/*!40000 ALTER TABLE `club_teams` DISABLE KEYS */;
INSERT INTO `club_teams` VALUES (1,10,2003),(2,12,2003),(3,11,2003),(4,9,2003),(5,8,2003),(6,6,2003),(7,5,2003),(8,4,2003),(9,3,2003),(10,2,2003),(11,1,2003),(12,7,2003),(14,10,2004),(15,12,2004),(16,11,2004),(17,9,2004),(18,8,2004),(19,6,2004),(20,5,2004),(21,4,2004),(22,3,2004),(23,2,2004),(24,1,2004),(25,7,2004),(26,10,2005),(27,13,2004);
/*!40000 ALTER TABLE `club_teams` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `club_teams_tournaments`
--

DROP TABLE IF EXISTS `club_teams_tournaments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `club_teams_tournaments` (
  `tournamentID` int NOT NULL,
  `teamID` int NOT NULL,
  PRIMARY KEY (`tournamentID`,`teamID`),
  KEY `club_teams_tournaments_teamID_idx` (`teamID`),
  CONSTRAINT `club_teams_tournaments_teamID` FOREIGN KEY (`teamID`) REFERENCES `club_teams` (`teamID`),
  CONSTRAINT `club_teams_tournaments_tournamentID_FK` FOREIGN KEY (`tournamentID`) REFERENCES `tournaments` (`tournamentID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `club_teams_tournaments`
--

LOCK TABLES `club_teams_tournaments` WRITE;
/*!40000 ALTER TABLE `club_teams_tournaments` DISABLE KEYS */;
INSERT INTO `club_teams_tournaments` VALUES (2,1),(2,2),(2,3),(2,4),(2,5),(2,6),(2,7),(2,8),(2,9),(2,10),(2,11),(2,12),(3,14),(3,15),(3,16),(3,17),(3,18),(3,19),(3,20),(3,21),(3,22),(3,23),(3,24),(3,25);
/*!40000 ALTER TABLE `club_teams_tournaments` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `clubs`
--

DROP TABLE IF EXISTS `clubs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `clubs` (
  `clubID` int NOT NULL AUTO_INCREMENT,
  `clubName` varchar(30) NOT NULL,
  PRIMARY KEY (`clubID`,`clubName`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `clubs`
--

LOCK TABLES `clubs` WRITE;
/*!40000 ALTER TABLE `clubs` DISABLE KEYS */;
INSERT INTO `clubs` VALUES (1,'Знамя Труда'),(2,'Керамик'),(3,'КСШОР Зоркий'),(4,'СШ Виктория'),(5,'СШ Витязь'),(6,'СШ Клин'),(7,'СШ ЦДЮС'),(8,'СШОР'),(9,'СШОР Метеор'),(10,'УОР №5'),(11,'ФСК Долгопрудный'),(12,'ФСШ Восток-Электросталь'),(13,'ЦПК'),(14,'Медвежьи Озёра');
/*!40000 ALTER TABLE `clubs` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `matches`
--

DROP TABLE IF EXISTS `matches`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `matches` (
  `tournamentID` int NOT NULL,
  `homeTeamID` int NOT NULL,
  `guestTeamID` int NOT NULL,
  `goalsHomeTeam` int DEFAULT NULL,
  `goalsGuestTeam` int DEFAULT NULL,
  `date` date NOT NULL,
  `tour` int NOT NULL,
  PRIMARY KEY (`tournamentID`,`homeTeamID`,`guestTeamID`),
  KEY `match_homeTeamID_FK_idx` (`homeTeamID`),
  KEY `match_guestTeamID_FK_idx` (`guestTeamID`),
  KEY `match_tour_FK_idx` (`tour`),
  CONSTRAINT `match_guestTeamID_FK` FOREIGN KEY (`guestTeamID`) REFERENCES `club_teams` (`teamID`),
  CONSTRAINT `match_homeTeamID_FK` FOREIGN KEY (`homeTeamID`) REFERENCES `club_teams` (`teamID`),
  CONSTRAINT `match_tour_FK` FOREIGN KEY (`tour`) REFERENCES `tours` (`tour`),
  CONSTRAINT `match_tournamentID_FK` FOREIGN KEY (`tournamentID`) REFERENCES `tournaments` (`tournamentID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `matches`
--

LOCK TABLES `matches` WRITE;
/*!40000 ALTER TABLE `matches` DISABLE KEYS */;
INSERT INTO `matches` VALUES (2,1,2,2,1,'2020-04-12',1),(2,2,1,NULL,NULL,'2020-04-19',2),(2,3,4,3,0,'2020-04-12',1),(2,4,10,NULL,NULL,'2020-04-19',2),(2,5,6,1,1,'2020-04-12',1),(2,6,12,NULL,NULL,'2020-04-19',2),(2,7,8,2,2,'2020-04-12',1),(2,8,7,NULL,NULL,'2020-04-19',2),(2,9,5,NULL,NULL,'2020-04-19',2),(2,9,10,3,4,'2020-04-12',1),(2,11,3,3,4,'2020-04-19',2),(2,11,12,0,0,'2020-04-12',1),(3,17,15,NULL,NULL,'2020-04-14',1),(3,21,14,NULL,NULL,'2020-04-05',1);
/*!40000 ALTER TABLE `matches` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `players`
--

DROP TABLE IF EXISTS `players`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `players` (
  `playerID` int NOT NULL AUTO_INCREMENT,
  `name` varchar(30) NOT NULL,
  `surname` varchar(30) NOT NULL,
  `yearOfBirth` year NOT NULL,
  `teamID` int NOT NULL,
  `role` varchar(20) NOT NULL,
  PRIMARY KEY (`playerID`),
  UNIQUE KEY `playerID_UNIQUE` (`playerID`),
  KEY `role_FK_idx` (`role`),
  KEY `player_yearOfBirth_FK_idx` (`yearOfBirth`),
  KEY `player_teamID_FK_idx` (`teamID`),
  CONSTRAINT `player_teamID_FK` FOREIGN KEY (`teamID`) REFERENCES `club_teams` (`teamID`),
  CONSTRAINT `player_yearOfBirth_FK` FOREIGN KEY (`yearOfBirth`) REFERENCES `ages` (`yearOfBirth`),
  CONSTRAINT `role_FK` FOREIGN KEY (`role`) REFERENCES `roles` (`role`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `players`
--

LOCK TABLES `players` WRITE;
/*!40000 ALTER TABLE `players` DISABLE KEYS */;
INSERT INTO `players` VALUES (1,'Матвей','Богатов',2003,1,'нападающий'),(2,'Тимур','Галимзянов',2003,1,'нападающий'),(3,'Александр','Дьячков',2003,1,'вратарь'),(4,'Егор','Завьялов',2003,1,'полузащитник'),(5,'Ахмед','Исмаилов',2003,1,'защитник'),(6,'Матвей','Каяшов',2003,1,'полузащитник'),(7,'Дэвид','Кокоев',2003,1,'защитник'),(8,'Илья','Коротких',2003,1,'защитник'),(9,'Юрий','Кудреватый',2003,1,'защитник'),(10,'Никита','Лебедев',2003,1,'защитник'),(11,'Владимир','Марухин',2003,1,'полузащитник'),(13,'Егор','Мосин',2003,1,'полузащитник');
/*!40000 ALTER TABLE `players` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `roles`
--

DROP TABLE IF EXISTS `roles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `roles` (
  `role` varchar(20) NOT NULL,
  PRIMARY KEY (`role`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `roles`
--

LOCK TABLES `roles` WRITE;
/*!40000 ALTER TABLE `roles` DISABLE KEYS */;
INSERT INTO `roles` VALUES ('вратарь'),('защитник'),('нападающий'),('полузащитник');
/*!40000 ALTER TABLE `roles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tournament_statuses`
--

DROP TABLE IF EXISTS `tournament_statuses`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tournament_statuses` (
  `statusName` varchar(8) NOT NULL,
  PRIMARY KEY (`statusName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tournament_statuses`
--

LOCK TABLES `tournament_statuses` WRITE;
/*!40000 ALTER TABLE `tournament_statuses` DISABLE KEYS */;
INSERT INTO `tournament_statuses` VALUES ('идёт'),('не начат'),('окончен');
/*!40000 ALTER TABLE `tournament_statuses` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tournament_years`
--

DROP TABLE IF EXISTS `tournament_years`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tournament_years` (
  `tournamentYear` year NOT NULL,
  PRIMARY KEY (`tournamentYear`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tournament_years`
--

LOCK TABLES `tournament_years` WRITE;
/*!40000 ALTER TABLE `tournament_years` DISABLE KEYS */;
INSERT INTO `tournament_years` VALUES (2019),(2020);
/*!40000 ALTER TABLE `tournament_years` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tournaments`
--

DROP TABLE IF EXISTS `tournaments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tournaments` (
  `tournamentID` int NOT NULL AUTO_INCREMENT,
  `tournamentName` varchar(100) NOT NULL,
  `yearOfBirth` year NOT NULL,
  `year` year NOT NULL,
  `status` varchar(8) NOT NULL,
  PRIMARY KEY (`tournamentID`),
  KEY `tournaments_tournamentYears_FK_idx` (`year`),
  KEY `tournaments_tournamentStatus_FK_idx` (`status`),
  KEY `tournament_yearOfBirth_FK_idx` (`yearOfBirth`),
  CONSTRAINT `tournaments_tournamentStatus_FK` FOREIGN KEY (`status`) REFERENCES `tournament_statuses` (`statusName`),
  CONSTRAINT `tournaments_tournamentYears_FK` FOREIGN KEY (`year`) REFERENCES `tournament_years` (`tournamentYear`),
  CONSTRAINT `tournaments_yearOfBirth_FK` FOREIGN KEY (`yearOfBirth`) REFERENCES `ages` (`yearOfBirth`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tournaments`
--

LOCK TABLES `tournaments` WRITE;
/*!40000 ALTER TABLE `tournaments` DISABLE KEYS */;
INSERT INTO `tournaments` VALUES (1,'Первенство Московской области среди юношеских команд 2003 г.р.',2003,2019,'окончен'),(2,'Первенство Московской области среди юношеских команд 2003 г.р.',2003,2020,'идёт'),(3,'Первенство Московской области среди юношеских команд 2004 г.р.',2004,2020,'не начат'),(4,'Первенство Московской области среди юношеских команд 2005 г.р.',2005,2020,'не начат'),(6,'Первенство Московской области среди юношеских команд 2006 г.р.',2006,2020,'не начат');
/*!40000 ALTER TABLE `tournaments` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tours`
--

DROP TABLE IF EXISTS `tours`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tours` (
  `tour` int NOT NULL,
  PRIMARY KEY (`tour`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tours`
--

LOCK TABLES `tours` WRITE;
/*!40000 ALTER TABLE `tours` DISABLE KEYS */;
INSERT INTO `tours` VALUES (1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12),(13),(14),(15),(16),(17),(18),(19),(20),(21),(22);
/*!40000 ALTER TABLE `tours` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2020-06-23 11:50:10
