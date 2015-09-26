CREATE TABLE catalogs (
		id INTEGER PRIMARY KEY,
		name VARCHAR(50),
		short_description VARCHAR(50),
		long_description TEXT,
		image TEXT,
		weight INTEGER,
		table_name VARCHAR(50),
		view INTEGER DEFAULT 1,
		zoom FLOAT DEFAULT 1,
		order_by VARCHAR (50),
		sort VARCHAR (20)
		);

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
		language VARCHAR(100),
		distributor TEXT,
		medium TEXT,
		comments TEXT
		);
		
CREATE TABLE items_films_columns (
        aux VARCHAR(1) DEFAULT '1',
		id VARCHAR(1),
		image VARCHAR(1),
		rating VARCHAR(1),
		title VARCHAR(1),
		original_title VARCHAR(1),
		director VARCHAR(1),
		starring VARCHAR(1),
		date VARCHAR(1),
		genre VARCHAR(1),
		runtime VARCHAR(1),
		country VARCHAR(1),
		language VARCHAR(1),
		distributor VARCHAR(1),
		medium VARCHAR(1),
		comments VARCHAR(1)
		);

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
		language VARCHAR(100),
		comments TEXT
		);
		
CREATE TABLE items_books_columns (
        aux VARCHAR(1) DEFAULT '1',
		id VARCHAR(1),
		image VARCHAR(1),
		rating VARCHAR(1),
		title VARCHAR(1),
		original_title VARCHAR(1),
		author VARCHAR(1),
		date VARCHAR(1),
		genre VARCHAR(1),
		pages VARCHAR(1),
		publisher VARCHAR(1),
		isbn VARCHAR(1),
		country VARCHAR(1),
		language VARCHAR(1),
		comments VARCHAR(1)
		);

CREATE TABLE items_albums (
		id INTEGER PRIMARY KEY,
		image TEXT,
		rating INTEGER,
		title TEXT,
		author TEXT,
		label TEXT,
		date VARCHAR(100),
		style VARCHAR(100),
		asin VARCHAR(10), 
		tracks TEXT,
		medium TEXT,
		runtime INTEGER,
		comments TEXT
		);
		
CREATE TABLE items_albums_columns (
	        aux VARCHAR(1) DEFAULT '1',
		id VARCHAR(1),
		image VARCHAR(1),
		rating VARCHAR(1),
		title VARCHAR(1),
		author VARCHAR(1),
		label VARCHAR(1),
		date VARCHAR(1),
		style VARCHAR(1),
		asin VARCHAR(1),
		tracks VARCHAR(1),
		medium VARCHAR(1),
		runtime VARCHAR(1),
		comments VARCHAR(1)
		);

CREATE TABLE borrowers (
		id INTEGER PRIMARY KEY,
		name TEXT
		);

CREATE TABLE lends (
		id INTEGER PRIMARY KEY,
		borrower INTEGER,
		table_name VARCHAR (50),
		item_id INTEGER
		);

INSERT INTO catalogs (name, short_description, long_description, image, weight, table_name, view, zoom, order_by, sort)
	VALUES ('films',
		'Films',
		'DVDs, DivX and all kind of videos',
		'films.png',
		0,
		'items_films',
		0,
		1,
		'title',
		'ascending');

INSERT INTO items_films_columns (id, image, rating, title, original_title, director, starring, date, genre, runtime,
		country, language, distributor, medium, comments)
	VALUES ('Y','Y','Y','Y','Y','Y','Y','Y','Y','Y','Y','Y', 'Y', 'Y', 'Y'); 

INSERT INTO catalogs (name, short_description, long_description, image, weight, table_name, view, zoom, order_by, sort)
	VALUES ('books',
		'Books',
		'Books',
		'books.png',
		0,
		'items_books',
		0,
		1,
		'title',
		'ascending');
			
INSERT INTO items_books_columns (id, image, rating, title, original_title, author, date, genre, pages, publisher,
		isbn, country, language, comments)
	VALUES ('Y','Y','Y','Y','Y','Y','Y','Y','Y','Y','Y','Y','Y', 'Y');

INSERT INTO catalogs (name, short_description, long_description, image, weight, table_name, view, zoom, order_by, sort)
	VALUES ('albums',
		'Albums',
		'Music albums',
		'albums.png',
		0,
		'items_albums',
		0,
		1,
		'title',
		'ascending');
			
INSERT INTO items_albums_columns (id, image, rating, title, author, label, date, style, asin, tracks, medium, runtime, comments)
	VALUES ('Y','Y','Y','Y','Y','Y','Y','Y','Y','Y','Y','Y', 'Y');

