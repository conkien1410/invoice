-- public.customer definition

-- Drop table

-- DROP TABLE public.customer;

CREATE TABLE public.customer (
	id int8 NOT NULL,
	"name" varchar NULL,
	CONSTRAINT customer_pk PRIMARY KEY (id)
);


-- public.package definition

-- Drop table

-- DROP TABLE public.package;

CREATE TABLE public.package (
	id int8 NOT NULL,
	price int8 NULL,
	name varchar NULL,
	"number" int8 NULL,
	CONSTRAINT package_pk PRIMARY KEY (id)
);


-- public.discount definition

-- Drop table

-- DROP TABLE public.discount;

CREATE TABLE public.discount (
	id int8 NOT NULL,
	"name" varchar NULL,
	"configuration" jsonb NULL,
	CONSTRAINT program_pk PRIMARY KEY (id)
);


-- public."order" definition

-- Drop table

-- DROP TABLE public."order";

CREATE TABLE public."order" (
	customer_id int8 NULL,
	package_id int8 NULL,
	"number" int8 NULL,
	status varchar NULL,
	id int8 NOT NULL,
	"date" date NULL,
	CONSTRAINT order_pk PRIMARY KEY (id)
);


-- public."order" foreign keys

ALTER TABLE public."order" ADD CONSTRAINT order_fk FOREIGN KEY (customer_id) REFERENCES public.customer(id);
ALTER TABLE public."order" ADD CONSTRAINT order_fk_1 FOREIGN KEY (package_id) REFERENCES public.package(id);


-- public.promotion definition

-- Drop table

-- DROP TABLE public.promotion;

CREATE TABLE public.promotion (
	customer_id int8 NULL,
	discount_id int8 NULL
);


-- public.promotion foreign keys

ALTER TABLE public.promotion ADD CONSTRAINT bonus_fk FOREIGN KEY (customer_id) REFERENCES public.customer(id);
ALTER TABLE public.promotion ADD CONSTRAINT bonus_fk_2 FOREIGN KEY (discount_id) REFERENCES public.discount(id);



----------------------------------------------------------------------------------------------------------------


