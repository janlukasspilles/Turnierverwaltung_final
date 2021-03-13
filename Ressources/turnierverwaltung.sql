-- phpMyAdmin SQL Dump
-- version 5.0.4
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Erstellungszeit: 17. Feb 2021 um 09:50
-- Server-Version: 10.4.17-MariaDB
-- PHP-Version: 8.0.2

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";
DROP DATABASE turnierverwaltung;
CREATE DATABASE turnierverwaltung;

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Datenbank: `turnierverwaltung`
--

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `mannschaft`
--

CREATE TABLE `mannschaft` (
  `ID` int(11) NOT NULL,
  `NAME` varchar(50) NOT NULL,
  `STADT` varchar(50) NOT NULL,
  `GRUENDUNGSJAHR` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `schiedsrichter`
--

CREATE TABLE `schiedsrichter` (
  `ID` int(11) NOT NULL,
  `NACHNAME` varchar(35) NOT NULL,
  `VORNAME` varchar(35) NOT NULL,
  `GEBURTSTAG` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `spieler`
--

CREATE TABLE `spieler` (
  `ID` int(11) NOT NULL,
  `VORNAME` varchar(25) NOT NULL,
  `NACHNAME` varchar(25) NOT NULL,
  `MANNSCHAFT_ID` int(11),
  `GEBURTSTAG` date
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `trainer`
--

CREATE TABLE `trainer` (
  `ID` int(11) NOT NULL,
  `VORNAME` varchar(40) NOT NULL,
  `NACHNAME` varchar(40) NOT NULL,
  `MANNSCHAFT_ID` int(11),
  `JAHRE_ERFAHRUNG` int(11),
  `GEBURTSTAG` date
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `FUSSBALLSPIELER`
--

CREATE TABLE `FUSSBALLSPIELER` (
  `ID` int(11) NOT NULL,
  `SPIELER_ID` int(11) NOT NULL,
  `ANZAHL_TORE` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `HANDBALLSPIELER`
--

CREATE TABLE `HANDBALLSPIELER` (
  `ID` int(11) NOT NULL,
  `SPIELER_ID` int(11) NOT NULL,
  `ANZAHL_TORE` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `TENNISSPIELER`
--

CREATE TABLE `TENNISSPIELER` (
  `ID` int(11) NOT NULL,
  `SPIELER_ID` int(11) NOT NULL,
  `GEWONNENE_SPIELE` int(11) NOT NULL,
  `ANZAHL_SPIELE` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `PHYSIO`
--

CREATE TABLE `PHYSIO` (
  `ID` int(11) NOT NULL,
  `VORNAME` varchar(25) NOT NULL,
  `NACHNAME` varchar(25) NOT NULL,
  `MANNSCHAFT_ID` int(11),
  `GEBURTSTAG` date
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `SPIEL`
--

CREATE TABLE `SPIEL` (
  `ID` int(11) NOT NULL,
  `SPORTART_ID` int(11) NOT NULL,
  `BEGINN` TIME NOT NULL,
  `ENDE` TIME NOT NULL,
  `MANNSCHAFT1_ID` int(11) NOT NULL,
  `MANNSCHAFT2_ID` int(11) NOT NULL,
  `SCHIEDSRICHTER_ID` int(11) NOT NULL,
  `PUNKTE_MANNSCHAFT1` int(11) NOT NULL,
  `PUNKTE_MANNSCHAFT2` int(11) NOT NULL,
  `TURNIER_ID` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `TURNIER`
--

CREATE TABLE `TURNIER` (
  `ID` int(11) NOT NULL,
  `DATUM` DATE NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `SPORTART`
--

CREATE TABLE `SPORTART` (
  `ID` int(11) NOT NULL,
  `NAME` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Indizes der exportierten Tabellen
--

--
-- Indizes für die Tabelle `mannschaft`
--
ALTER TABLE `mannschaft`
  ADD PRIMARY KEY (`ID`);

--
-- Indizes für die Tabelle `schiedsrichter`
--
ALTER TABLE `schiedsrichter`
  ADD PRIMARY KEY (`ID`);

--
-- Indizes für die Tabelle `spieler`
--
ALTER TABLE `spieler`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `MANNSCHAFT_ID` (`MANNSCHAFT_ID`);

--
-- Indizes für die Tabelle `trainer`
--
ALTER TABLE `trainer`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `MANNSCHAFT_ID` (`MANNSCHAFT_ID`);

--
-- Indizes für die Tabelle `FUSSBALLSPIELER`
--
ALTER TABLE `FUSSBALLSPIELER`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `SPIELER_ID` (`SPIELER_ID`);

--
-- Indizes für die Tabelle `HANDBALLSPIELER`
--
ALTER TABLE `HANDBALLSPIELER`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `SPIELER_ID` (`SPIELER_ID`);

--
-- Indizes für die Tabelle `TENNISSPIELER`
--
ALTER TABLE `TENNISSPIELER`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `SPIELER_ID` (`SPIELER_ID`);

--
-- Indizes für die Tabelle `PHYSIO`
--
ALTER TABLE `PHYSIO`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `MANNSCHAFT_ID` (`MANNSCHAFT_ID`);

--
-- Indizes für die Tabelle `TURNIER`
--
ALTER TABLE `TURNIER`
  ADD PRIMARY KEY (`ID`);
  
--
-- Indizes für die Tabelle `SPIEL`
--
ALTER TABLE `SPIEL`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `MANNSCHAFT1_ID` (`MANNSCHAFT1_ID`),
  ADD KEY `MANNSCHAFT2_ID` (`MANNSCHAFT2_ID`),
  ADD KEY `SCHIEDSRICHTER_ID` (`SCHIEDSRICHTER_ID`),
  ADD KEY `TURNIER_ID` (`TURNIER_ID`),
  ADD KEY `SPORTART_ID` (`SPORTART_ID`);

--
-- Indizes für die Tabelle `SPORTART`
--
ALTER TABLE `SPORTART`
  ADD PRIMARY KEY (`ID`);

--
-- AUTO_INCREMENT für exportierte Tabellen
--

--
-- AUTO_INCREMENT für Tabelle `mannschaft`
--
ALTER TABLE `mannschaft`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT für Tabelle `schiedsrichter`
--
ALTER TABLE `schiedsrichter`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT für Tabelle `spieler`
--
ALTER TABLE `spieler`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT für Tabelle `trainer`
--
ALTER TABLE `trainer`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT für Tabelle `FUSSBALLSPIELER`
--
ALTER TABLE `FUSSBALLSPIELER`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;
  
--
-- AUTO_INCREMENT für Tabelle `HANDBALLSPIELER`
--
ALTER TABLE `HANDBALLSPIELER`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;
  
--
-- AUTO_INCREMENT für Tabelle `TENNISSPIELER`
--
ALTER TABLE `TENNISSPIELER`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT für Tabelle `PHYSIO`
--
ALTER TABLE `PHYSIO`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;  
  
--
-- AUTO_INCREMENT für Tabelle `SPIEL`
--
ALTER TABLE `SPIEL`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT für Tabelle `SPORTART`
--
ALTER TABLE `SPORTART`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT für Tabelle `TURNIER`
--
ALTER TABLE `TURNIER`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- Constraints der exportierten Tabellen
--

--
-- Constraints der Tabelle `trainer`
--
ALTER TABLE `trainer`
  ADD CONSTRAINT `FK_TRAINER_MANNSCHAFT_ID` FOREIGN KEY (`MANNSCHAFT_ID`) REFERENCES `mannschaft` (`ID`) ON DELETE SET NULL ON UPDATE CASCADE;
  
--
-- Constraints der Tabelle `SPIELER`
--
ALTER TABLE `SPIELER`
  ADD CONSTRAINT `FK_SPIELER_MANNSCHAFT_ID` FOREIGN KEY (`MANNSCHAFT_ID`) REFERENCES `mannschaft` (`ID`) ON DELETE SET NULL ON UPDATE CASCADE;
    
--
-- Constraints der Tabelle `FUSSBALLSPIELER`
--
ALTER TABLE `FUSSBALLSPIELER`
  ADD CONSTRAINT `FK_FUSSBALL_SPIELER_ID` FOREIGN KEY (`SPIELER_ID`) REFERENCES `SPIELER` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE;
      
--
-- Constraints der Tabelle `HANDBALLSPIELER`
--
ALTER TABLE `HANDBALLSPIELER`
  ADD CONSTRAINT `FK_HANDBALL_SPIELER_ID` FOREIGN KEY (`SPIELER_ID`) REFERENCES `SPIELER` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE;
        
--
-- Constraints der Tabelle `TENNISSPIELER`
--
ALTER TABLE `TENNISSPIELER`
  ADD CONSTRAINT `FK_TENNIS_SPIELER_ID` FOREIGN KEY (`SPIELER_ID`) REFERENCES `SPIELER` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints der Tabelle `PHYSIO`
--
ALTER TABLE `PHYSIO`
  ADD CONSTRAINT `FK_PHYSIO_MANNSCHAFT_ID` FOREIGN KEY (`MANNSCHAFT_ID`) REFERENCES `MANNSCHAFT` (`ID`) ON DELETE SET NULL ON UPDATE CASCADE;

--
-- Constraints der Tabelle `SPIEL`
--
ALTER TABLE `SPIEL`
  ADD CONSTRAINT `FK_MANNSCHAFT1_ID` FOREIGN KEY (`MANNSCHAFT1_ID`) REFERENCES `MANNSCHAFT` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_MANNSCHAFT2_ID` FOREIGN KEY (`MANNSCHAFT2_ID`) REFERENCES `MANNSCHAFT` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_SPORTART_ID` FOREIGN KEY (`SPORTART_ID`) REFERENCES `SPORTART` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_SCHIEDSRICHTER_ID` FOREIGN KEY (`SCHIEDSRICHTER_ID`) REFERENCES `SCHIEDSRICHTER` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE,  
  ADD CONSTRAINT `FK_TURNIER_ID` FOREIGN KEY (`TURNIER_ID`) REFERENCES `TURNIER` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE;  
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
