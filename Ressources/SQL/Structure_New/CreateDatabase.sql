/* Alte Datenbank wird gedroppt, falls vorhanden, damit mit "frischen"
   Daten getestet werden kann. */
DROP DATABASE IF EXISTS Turnierverwaltung;
CREATE DATABASE Turnierverwaltung;
-- Sonst kommt beim Erstellen der Tabellen "no database selected"
USE Turnierverwaltung;