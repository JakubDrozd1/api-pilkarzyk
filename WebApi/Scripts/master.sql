--CREATE TABLE USERS ( 
--    ID_USER        INTEGER GENERATED BY DEFAULT AS IDENTITY, 
--    LOGIN          VARCHAR(50) CHARACTER SET UTF8 NOT NULL, 
--    USER_PASSWORD  VARCHAR(200) CHARACTER SET UTF8 NOT NULL, 
--    FIRSTNAME      VARCHAR(50) CHARACTER SET UTF8 NOT NULL, 
--    SURNAME        VARCHAR(50) CHARACTER SET UTF8 NOT NULL, 
--    EMAIL          VARCHAR(50) CHARACTER SET UTF8 NOT NULL, 
--    PHONE_NUMBER   INTEGER NOT NULL, 
--    SALT           VARCHAR(100) CHARACTER SET UTF8 NOT NULL, 
--    IS_ADMIN       BOOLEAN DEFAULT false NOT NULL, 
--    AVATAR         BLOB SUB_TYPE 1 SEGMENT SIZE 16384 CHARACTER SET UTF8, 
--    GROUP_COUNTER  SMALLINT DEFAULT 0 NOT NULL 

--); 

--ALTER TABLE USERS ADD CONSTRAINT USERS_PK PRIMARY KEY (ID_USER); 
--CREATE INDEX USERS_EMAIL_IDX ON USERS (EMAIL); 

--CREATE TABLE REFRESH_TOKENS (
--    ID_TOKEN             INTEGER GENERATED BY DEFAULT AS IDENTITY,
--    IDUSER               INTEGER NOT NULL,
--    REFRESH_TOKEN_VALUE  VARCHAR(255) CHARACTER SET UTF8 NOT NULL,
--    DATE_EXPIRE          TIMESTAMP NOT NULL,
--    IDCLIENT             VARCHAR(50) NOT NULL
--);
--ALTER TABLE REFRESH_TOKENS ADD CONSTRAINT REFRESH_TOKENS_PK PRIMARY KEY (ID_TOKEN);
--ALTER TABLE REFRESH_TOKENS ADD CONSTRAINT FK_REFRESH_TOKENS_1 FOREIGN KEY (IDUSER) REFERENCES USERS (ID_USER);

--CREATE TABLE GROUPS ( 
--    ID_GROUP      INTEGER GENERATED BY DEFAULT AS IDENTITY, 
--    NAME          VARCHAR(100) CHARACTER SET UTF8 NOT NULL, 
--    IS_MODERATED  BOOLEAN DEFAULT true NOT NULL, 
--    DESCRIPTION   VARCHAR(300) CHARACTER SET UTF8 
--); 

--ALTER TABLE GROUPS ADD CONSTRAINT GROUPS_PK PRIMARY KEY (ID_GROUP); 
--CREATE INDEX GROUPS_NAME_IDX ON GROUPS (NAME); 

--CREATE TABLE GROUPS_USERS ( 

--    IDUSER        INTEGER NOT NULL, 
--    IDGROUP       INTEGER NOT NULL, 
--    ACCOUNT_TYPE  SMALLINT DEFAULT 0 NOT NULL 

--); 
--ALTER TABLE GROUPS_USERS ADD CONSTRAINT FK_GROUPS_USERS_1 FOREIGN KEY (IDUSER) REFERENCES USERS (ID_USER); 
--ALTER TABLE GROUPS_USERS ADD CONSTRAINT FK_GROUPS_USERS_2 FOREIGN KEY (IDGROUP) REFERENCES GROUPS (ID_GROUP); 

--CREATE TABLE RESET_PASSWORD ( 
--    ID_RESET_PASSWORD  INTEGER GENERATED BY DEFAULT AS IDENTITY, 
--    DATE_ADD           TIMESTAMP DEFAULT current_timestamp NOT NULL, 
--    IDUSER             INTEGER NOT NULL 

--); 

--ALTER TABLE RESET_PASSWORD ADD CONSTRAINT RESET_PASSWORD_PK PRIMARY KEY (ID_RESET_PASSWORD); 
--ALTER TABLE RESET_PASSWORD ADD CONSTRAINT FK_RESET_PASSWORD_1 FOREIGN KEY (IDUSER) REFERENCES USERS (ID_USER); 

--CREATE TABLE NOTIFICATION_TOKENS (
--    ID_NOTIFICATION_TOKENS  INTEGER GENERATED BY DEFAULT AS IDENTITY,
--    TOKEN                   VARCHAR(250) CHARACTER SET UTF8 NOT NULL,
--    IDUSER                  INTEGER NOT NULL
--);
--ALTER TABLE NOTIFICATION_TOKENS ADD CONSTRAINT NOTIFICATION_TOKENS_PK PRIMARY KEY (ID_NOTIFICATION_TOKENS);
--ALTER TABLE NOTIFICATION_TOKENS ADD CONSTRAINT FK_NOTIFICATION_TOKENS_1 FOREIGN KEY (IDUSER) REFERENCES USERS (ID_USER);

--CREATE TABLE ACCESS_TOKENS ( 
--    ID_TOKEN            INTEGER GENERATED BY DEFAULT AS IDENTITY, 
--    IDUSER              INTEGER NOT NULL, 
--    DATE_EXPIRE         TIMESTAMP NOT NULL, 
--    IDCLIENT            VARCHAR(50) CHARACTER SET UTF8 NOT NULL, 
--    ACCESS_TOKEN_VALUE  BLOB SUB_TYPE 1 SEGMENT SIZE 16384 CHARACTER SET UTF8 
--); 
--ALTER TABLE ACCESS_TOKENS ADD CONSTRAINT ACCESS_TOKENS_PK PRIMARY KEY (ID_TOKEN); 
--ALTER TABLE ACCESS_TOKENS ADD CONSTRAINT FK_ACCESS_TOKENS_1 FOREIGN KEY (IDUSER) REFERENCES USERS (ID_USER); 

--CREATE TABLE MEETINGS ( 
--    ID_MEETING      INTEGER GENERATED BY DEFAULT AS IDENTITY, 
--    DATE_MEETING    TIMESTAMP NOT NULL, 
--    PLACE           VARCHAR(50) CHARACTER SET UTF8 NOT NULL, 
--    DESCRIPTION     VARCHAR(100) CHARACTER SET UTF8, 
--    IDGROUP         INTEGER NOT NULL, 
--    QUANTITY        INTEGER NOT NULL, 
--    IDAUTHOR        INTEGER NOT NULL, 
--    IS_INDEPENDENT  BOOLEAN DEFAULT false NOT NULL 
--); 

--ALTER TABLE MEETINGS ADD CONSTRAINT MEETINGS_PK PRIMARY KEY (ID_MEETING); 
--ALTER TABLE MEETINGS ADD CONSTRAINT FK_MEETINGS_1 FOREIGN KEY (IDGROUP) REFERENCES GROUPS (ID_GROUP); 
--ALTER TABLE MEETINGS ADD CONSTRAINT FK_MEETINGS_2 FOREIGN KEY (IDAUTHOR) REFERENCES USERS (ID_USER); 

--CREATE TABLE USERS_MEETINGS (
--    IDUSER     INTEGER NOT NULL,
--    IDMEETING  INTEGER NOT NULL
--);
--ALTER TABLE USERS_MEETINGS ADD CONSTRAINT FK_USERS_MEETINGS_1 FOREIGN KEY (IDUSER) REFERENCES USERS (ID_USER);
--ALTER TABLE USERS_MEETINGS ADD CONSTRAINT FK_USERS_MEETINGS_2 FOREIGN KEY (IDMEETING) REFERENCES MEETINGS (ID_MEETING);

--CREATE TABLE GROUP_INVITE (
--    ID_GROUP_INVITE  INTEGER GENERATED BY DEFAULT AS IDENTITY,
--    IDUSER           INTEGER,
--    IDGROUP          INTEGER NOT NULL,
--    IDAUTHOR         INTEGER NOT NULL,
--    DATE_ADD         TIMESTAMP DEFAULT current_timestamp NOT NULL,
--    EMAIL            VARCHAR(50) CHARACTER SET UTF8,
--    PHONE_NUMBER     INTEGER
--);

--ALTER TABLE GROUP_INVITE ADD CONSTRAINT GROUP_INVITE_PK PRIMARY KEY (ID_GROUP_INVITE); 
--ALTER TABLE GROUP_INVITE ADD CONSTRAINT FK_GROUP_INVITE_1 FOREIGN KEY (IDUSER) REFERENCES USERS (ID_USER); 
--ALTER TABLE GROUP_INVITE ADD CONSTRAINT FK_GROUP_INVITE_2 FOREIGN KEY (IDGROUP) REFERENCES GROUPS (ID_GROUP); 
--ALTER TABLE GROUP_INVITE ADD CONSTRAINT FK_GROUP_INVITE_3 FOREIGN KEY (IDAUTHOR) REFERENCES USERS (ID_USER); 

--CREATE TABLE TEAMS ( 
--    ID_TEAM    INTEGER GENERATED BY DEFAULT AS IDENTITY, 
--    IDMEETING  INTEGER NOT NULL, 
--    NAME       VARCHAR(30) CHARACTER SET UTF8 NOT NULL, 
--    COLOR      VARCHAR(10) CHARACTER SET UTF8 NOT NULL 

--); 
--ALTER TABLE TEAMS ADD CONSTRAINT TEAMS_PK PRIMARY KEY (ID_TEAM); 
--ALTER TABLE TEAMS ADD CONSTRAINT FK_TEAMS_1 FOREIGN KEY (IDMEETING) REFERENCES MEETINGS (ID_MEETING);

--CREATE TABLE MESSAGES (
--    ID_MESSAGE     INTEGER GENERATED BY DEFAULT AS IDENTITY,
--    IDMEETING      INTEGER NOT NULL,
--    IDUSER         INTEGER NOT NULL,
--    ANSWER         VARCHAR(50) CHARACTER SET UTF8,
--    DATE_ADD       TIMESTAMP DEFAULT current_timestamp NOT NULL,
--    WAITING_TIME   TIMESTAMP,
--    DATE_RESPONSE  TIMESTAMP,
--    IDTEAM         SMALLINT
--);
--ALTER TABLE MESSAGES ADD CONSTRAINT MESSAGES_PK PRIMARY KEY (ID_MESSAGE);
--ALTER TABLE MESSAGES ADD CONSTRAINT FK_MESSAGES_1 FOREIGN KEY (IDMEETING) REFERENCES MEETINGS (ID_MEETING);
--ALTER TABLE MESSAGES ADD CONSTRAINT FK_MESSAGES_2 FOREIGN KEY (IDUSER) REFERENCES USERS (ID_USER);
--ALTER TABLE MESSAGES ADD CONSTRAINT FK_MESSAGES_3 FOREIGN KEY (IDTEAM) REFERENCES TEAMS (ID_TEAM) ON DELETE SET NULL;


--CREATE TABLE GUESTS ( 

--    ID_GUEST   INTEGER GENERATED BY DEFAULT AS IDENTITY, 

--    NAME       VARCHAR(30) CHARACTER SET UTF8 DEFAULT 'Guest' NOT NULL, 

--    IDMEETING  INTEGER NOT NULL, 

--    IDTEAM     INTEGER 

--); 

--ALTER TABLE GUESTS ADD CONSTRAINT GUESTS_PK PRIMARY KEY (ID_GUEST); 

--ALTER TABLE GUESTS ADD CONSTRAINT FK_GUESTS_1 FOREIGN KEY (IDTEAM) REFERENCES TEAMS (ID_TEAM) ON DELETE SET NULL; 

--ALTER TABLE GUESTS ADD CONSTRAINT FK_GUESTS_2 FOREIGN KEY (IDMEETING) REFERENCES MEETINGS (ID_MEETING); 

--CREATE TABLE CHAT_MESSAGES ( 

--    ID_CHAT_MESSAGE  INTEGER GENERATED BY DEFAULT AS IDENTITY, 

--    IDMEETING        INTEGER NOT NULL, 

--    IDUSER           INTEGER NOT NULL, 

--    CHAT_MESSAGE     VARCHAR(200) CHARACTER SET UTF8 NOT NULL, 

--    DATE_SEND        TIMESTAMP DEFAULT current_timestamp NOT NULL 

--); 

--ALTER TABLE CHAT_MESSAGES ADD CONSTRAINT CHAT_MESSAGES_PK PRIMARY KEY (ID_CHAT_MESSAGE); 

--ALTER TABLE CHAT_MESSAGES ADD CONSTRAINT FK_CHAT_MESSAGES_1 FOREIGN KEY (IDUSER) REFERENCES USERS (ID_USER); 

--ALTER TABLE CHAT_MESSAGES ADD CONSTRAINT FK_CHAT_MESSAGES_2 FOREIGN KEY (IDMEETING) REFERENCES MEETINGS (ID_MEETING); 

--CREATE TABLE EMAIL_SENDER ( 

--    DISPLAY_NAME    VARCHAR(50) CHARACTER SET UTF8, 

--    EMAIL           VARCHAR(50) CHARACTER SET UTF8, 

--    EMAIL_PASSWORD  VARCHAR(200) CHARACTER SET UTF8, 

--    HOST            VARCHAR(200) CHARACTER SET UTF8, 

--    SALT            VARCHAR(300) CHARACTER SET UTF8, 

--    PORT            INTEGER, 

--    SSL             BOOLEAN, 

--    TLS             BOOLEAN 

--); 

  

--INSERT INTO EMAIL_SENDER (DISPLAY_NAME, EMAIL, EMAIL_PASSWORD, HOST, SALT, PORT, SSL, TLS) 

--VALUES ('JaBall', 'jaball@proman.com.pl', 'ExTKeEGe9MtnM9gAA5pi0emOjsS8aod801SKgqfu8K4=', 'Odqroh8U2KEYDT0OEtXr6pzPCYwPoOPvT4/UBkY7941a1nJDmL3ReDyD3GSSf0eD', 'i1zAjvsQG5Na+uvfq97cTll+6FAzB8DnoY9DYVi9hlQ=', 25, false, false); 

--CREATE TABLE RANKINGS ( 

--    ID_RANKING    INTEGER GENERATED BY DEFAULT AS IDENTITY, 

--    DATE_MEETING  TIMESTAMP NOT NULL, 

--    IDUSER        INTEGER NOT NULL, 

--    IDGROUP       INTEGER NOT NULL, 

--    POINT         INTEGER NOT NULL 

--); 

--CREATE TABLE CLIENT_TOKENS ( 

--    CLIENT_SECRET       VARCHAR(100) CHARACTER SET UTF8 NOT NULL, 

--    IDUSER              INTEGER, 

--    TOKEN_TIME          INTEGER, 

--    REFRESH_TOKEN_TIME  INTEGER, 

--    GRANT_TYPE          VARCHAR(50) CHARACTER SET UTF8 NOT NULL, 

--    ID_CLIENT           VARCHAR(50) CHARACTER SET UTF8 NOT NULL 

--); 

  

--INSERT INTO CLIENT_TOKENS (CLIENT_SECRET, TOKEN_TIME, REFRESH_TOKEN_TIME, GRANT_TYPE, ID_CLIENT) 

--VALUES ( 

--  '@%7fMQSMMmhc5x40M8S4Y%A%h7l7!5Zcfkm!uXKL8nzvYO%ITc4P!hm14ENP08GD*Nh8XWumaL*yEur8', 

--  3600, 

--  1209600, 

--  'UserCredentials,RefreshToken', 

--  'api.pilkarzyk' 

--); 