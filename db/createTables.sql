DROP TABLE TESTTABLE1;

CREATE TABLE TESTTABLE1 (
	FIRSTFIELD INTEGER NOT NULL,
	SECONDFIELD CHAR(20) NOT NULL,
	THIRDFIELD TIMESTAMP,
	FOURTHFIELD VARCHAR(10),
	CONSTRAINT INTEG_7 PRIMARY KEY (SECONDFIELD)
) ;
CREATE UNIQUE INDEX "RDB$PRIMARY3" ON TESTTABLE1 (SECONDFIELD) ;
CREATE UNIQUE INDEX TESTINDEX1 ON TESTTABLE1 (FIRSTFIELD,SECONDFIELD) ;
CREATE INDEX TESTTABLE1_IDX ON TESTTABLE1 (FIRSTFIELD,SECONDFIELD,THIRDFIELD,FOURTHFIELD) ;

DROP TABLE TESTTABLE2;

CREATE TABLE TESTTABLE2 (
	FIRSTFIELD TIMESTAMP NOT NULL,
	SECONDFIELD VARCHAR(20) NOT NULL,
	THIRDFIELD DECIMAL(18,4),
	CONSTRAINT INTEG_10 PRIMARY KEY (FIRSTFIELD,SECONDFIELD)
) ;
CREATE UNIQUE INDEX "RDB$PRIMARY4" ON TESTTABLE2 (FIRSTFIELD,SECONDFIELD) ;
CREATE INDEX TESTINDEX2 ON TESTTABLE2 (THIRDFIELD) ;
CREATE INDEX TESTTABLE2_IDX ON TESTTABLE2 (FIRSTFIELD,SECONDFIELD,THIRDFIELD) ;