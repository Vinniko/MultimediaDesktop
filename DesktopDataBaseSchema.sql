-- MySQL dump 10.13  Distrib 8.0.22, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: desktopbd
-- ------------------------------------------------------
-- Server version	8.0.22

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
-- Table structure for table `desktops`
--

DROP TABLE IF EXISTS `desktops`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `desktops` (
  `id` int NOT NULL AUTO_INCREMENT,
  `IDa` int DEFAULT NULL,
  `labelCount` int DEFAULT NULL,
  `wallpaperImage` varchar(333) DEFAULT ' ',
  PRIMARY KEY (`id`),
  KEY `IDa` (`IDa`),
  CONSTRAINT `desktops_ibfk_1` FOREIGN KEY (`IDa`) REFERENCES `settings` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=32 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `font`
--

DROP TABLE IF EXISTS `font`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `font` (
  `id` int NOT NULL AUTO_INCREMENT,
  `font` varchar(30) DEFAULT NULL,
  `fontSize` int DEFAULT NULL,
  `fontA` int DEFAULT NULL,
  `fontR` int DEFAULT NULL,
  `fontG` int DEFAULT NULL,
  `fontB` int DEFAULT NULL,
  `fontBold` tinyint(1) DEFAULT NULL,
  `fontItalic` tinyint(1) DEFAULT NULL,
  `fontUnderline` tinyint(1) DEFAULT NULL,
  `fontStrikeout` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `labels`
--

DROP TABLE IF EXISTS `labels`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `labels` (
  `id` int NOT NULL AUTO_INCREMENT,
  `IDa` int DEFAULT NULL,
  `title` varchar(333) DEFAULT NULL,
  `imagePath` varchar(333) DEFAULT NULL,
  `path` varchar(333) DEFAULT NULL,
  `typeFlag` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `IDa` (`IDa`),
  CONSTRAINT `labels_ibfk_1` FOREIGN KEY (`IDa`) REFERENCES `settings` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=890 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `settings`
--

DROP TABLE IF EXISTS `settings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `settings` (
  `id` int NOT NULL AUTO_INCREMENT,
  `IDn` int DEFAULT NULL,
  `fontID` int DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `IDn` (`IDn`),
  KEY `fontID` (`fontID`),
  CONSTRAINT `settings_ibfk_1` FOREIGN KEY (`IDn`) REFERENCES `users` (`id`),
  CONSTRAINT `settings_ibfk_2` FOREIGN KEY (`fontID`) REFERENCES `font` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `id` int NOT NULL AUTO_INCREMENT,
  `login` varchar(30) DEFAULT NULL,
  `password` varchar(30) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `login` (`login`),
  UNIQUE KEY `login_2` (`login`)
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-07-05 10:13:06
