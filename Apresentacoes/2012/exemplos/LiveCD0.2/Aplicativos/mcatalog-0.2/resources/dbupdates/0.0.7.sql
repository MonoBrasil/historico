-- ##################
-- # TABLE catalogs #
-- ##################

CREATE TABLE temp AS SELECT * FROM catalogs;
DROP TABLE catalogs;

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

INSERT INTO catalogs (id,
		name,
		short_description,
		long_description,
		image,
		weight,
		table_name,
		order_by,
		sort)
	SELECT * FROM temp;

-- #####################
-- # TABLE items_films #
-- #####################

-- #####################
-- # TABLE items_books #
-- #####################

-- ###################
-- # TABLE borrowers #
-- ###################

-- ###############
-- # TABLE lends #
-- ###############
