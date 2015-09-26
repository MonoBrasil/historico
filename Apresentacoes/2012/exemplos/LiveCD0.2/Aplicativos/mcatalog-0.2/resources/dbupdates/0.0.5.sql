-- ##################
-- # TABLE catalogs #
-- ##################

-- #####################
-- # TABLE items_films #
-- #####################

CREATE TABLE temp AS SELECT * FROM items_films;
DROP TABLE items_films;

CREATE TABLE items_films (
		id INTEGER PRIMARY KEY,
		image TEXT,
		rating INTEGER,
		title TEXT,
		original_title TEXT,
		director TEXT,
		starring TEXT,
		date VARCHAR(100),
		genre VARCHAR(100),
		runtime INTEGER,
		country VARCHAR(100),
		languaje VARCHAR(100),
		distributor TEXT,
		medium TEXT,
		comments TEXT
		);

INSERT INTO items_films (id,
			 image,
			 rating,
			 title,
			 director,
			 starring,
			 date,
			 genre,
			 runtime,
			 country,
			 languaje,
			 distributor,
			 comments)
	SELECT * FROM temp;

DROP TABLE temp;

-- #####################
-- # TABLE items_books #
-- #####################

CREATE TABLE temp AS SELECT * FROM items_books;
DROP TABLE items_books;

CREATE TABLE items_books (
		id INTEGER PRIMARY KEY,
		image TEXT,
		rating INTEGER,
		title TEXT,
		original_title TEXT,
		author TEXT,
		date VARCHAR(100),
		genre VARCHAR(100),
		pages INTEGER,
		publisher TEXT,
		isbn VARCHAR(10),
		country VARCHAR(100),
		languaje VARCHAR(100),
		comments TEXT
		);
INSERT INTO items_books (id, image, rating, title, author, date, genre, pages, publisher, isbn, country, languaje, comments)
	SELECT * FROM temp;

DROP TABLE temp;

-- ###################
-- # TABLE borrowers #
-- ###################

-- ###############
-- # TABLE lends #
-- ###############
