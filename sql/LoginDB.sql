CREATE DATABASE IF NOT EXISTS `{0}` DEFAULT CHARACTER SET latin1 COLLATE latin1_swedish_ci;
USE `{0}`;

DROP TABLE IF EXISTS `accounts`;
CREATE TABLE `accounts` (
  `ID` int(10) NOT NULL AUTO_INCREMENT,
  `Username` varchar(12) NOT NULL,
  `Password` varchar(128) NOT NULL,
  `Salt` varchar(32) NOT NULL,
  `EULA` tinyint(1) UNSIGNED NOT NULL DEFAULT '0',
  `Gender` tinyint(3) UNSIGNED NOT NULL DEFAULT '10',
  `Pin` varchar(64) NOT NULL DEFAULT '',
  `Pic` varchar(64) NOT NULL DEFAULT '',
  `IsBanned` tinyint(1) UNSIGNED NOT NULL DEFAULT '0',
  `IsMaster` tinyint(1) UNSIGNED NOT NULL DEFAULT '0',
  `Birthday` date NOT NULL,
  `Creation` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `MaxCharacters` int(11) NOT NULL DEFAULT '3',
   PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

  DROP TABLE IF EXISTS `banned_ip`;
CREATE TABLE `banned_ip` (
  `Address` varchar(15) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

	 DROP TABLE IF EXISTS `banned_mac`;
CREATE TABLE `banned_mac` (
  `Address` varchar(17) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS `master_ip`;
CREATE TABLE `master_ip` (
  `IP` varchar(15) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

ALTER TABLE `accounts`
  ADD KEY `username` (`Username`) USING BTREE;

ALTER TABLE `banned_ip`
  ADD PRIMARY KEY (`Address`);

ALTER TABLE `banned_mac`
  ADD PRIMARY KEY (`Address`);

ALTER TABLE `master_ip`
  ADD PRIMARY KEY (`IP`);

INSERT INTO master_ip VALUES ('127.0.0.1');
