1> USE band_tracker;
2> GO
1> CREATE TABLE bands (id INT IDENTITY(1,1), name VARCHAR(255));
2> CREATE TABLE venues (id INT IDENTITY(1,1), name VARCHAR(255));
3> CREATE TABLE bands_venues (id INT IDENTITY(1,1), band_id INT, vanue_id INT);
4> GO
