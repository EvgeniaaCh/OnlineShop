CREATE SCHEMA "dbo";

CREATE TABLE dbo."Categories"
(
  "Id" serial NOT NULL,
  "Name" character varying(16),
  CONSTRAINT "PK_dbo.Categories" PRIMARY KEY ("Id")
)
WITH (
  OIDS=FALSE
);

CREATE TABLE dbo."Products"
(
  "Id" serial NOT NULL,
  "Name" character varying(16),
  "Description" character varying(128),
  "Price" integer NOT NULL,
  "CategoryId" integer NOT NULL DEFAULT 0,
  CONSTRAINT "PK_dbo.Products" PRIMARY KEY ("Id"),
  CONSTRAINT "FK_dbo.Products_dbo.Categories_CategoryId" FOREIGN KEY ("CategoryId")
      REFERENCES dbo."Categories" ("Id") MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE CASCADE
)
WITH (
  OIDS=FALSE
);

CREATE TABLE dbo."Roles"
(
  "Id" serial NOT NULL,
  "Name" character varying(16),
  "Connection" text,
  CONSTRAINT "PK_dbo.Roles" PRIMARY KEY ("Id")
)
WITH (
  OIDS=FALSE
);

CREATE TABLE dbo."Users"
(
  "Id" serial NOT NULL,
  "Guid" text,
  "Email" text,
  "Password" text,
  "FirstName" character varying(16),
  "LastName" character varying(16),
  "Gender" boolean NOT NULL DEFAULT false,
  "RoleId" integer NOT NULL DEFAULT 0,
  "BirthDt" timestamp without time zone,
  "CreateDt" timestamp without time zone,
  CONSTRAINT "PK_dbo.Users" PRIMARY KEY ("Id"),
  CONSTRAINT "FK_dbo.Users_dbo.Roles_RoleId" FOREIGN KEY ("RoleId")
      REFERENCES dbo."Roles" ("Id") MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE CASCADE
)
WITH (
  OIDS=FALSE
);

CREATE TABLE dbo."UserProducts"
(
  "User_Id" integer NOT NULL DEFAULT 0,
  "Product_Id" integer NOT NULL DEFAULT 0,
  CONSTRAINT "FK_dbo.UserProducts_dbo.Products_Product_Id" FOREIGN KEY ("Product_Id")
      REFERENCES dbo."Products" ("Id") MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE CASCADE,
  CONSTRAINT "FK_dbo.UserProducts_dbo.Users_User_Id" FOREIGN KEY ("User_Id")
      REFERENCES dbo."Users" ("Id") MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE CASCADE
)
WITH (
  OIDS=TRUE
);

CREATE TABLE dbo."Orders"
(
  "Id" serial NOT NULL,
  "CreateDt" timestamp(6) with time zone NOT NULL DEFAULT '-infinity'::timestamp with time zone,
  "UserId" integer NOT NULL DEFAULT 0,
  CONSTRAINT "PK_dbo.Orders" PRIMARY KEY ("Id"),
  CONSTRAINT "FK_dbo.Orders_dbo.Users_UserId" FOREIGN KEY ("UserId")
      REFERENCES dbo."Users" ("Id") MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE CASCADE
)
WITH (
  OIDS=FALSE
);

CREATE TABLE dbo."OrderProducts"
(
  "Order_Id" integer NOT NULL DEFAULT 0,
  "Product_Id" integer NOT NULL DEFAULT 0,
  CONSTRAINT "FK_dbo.OrderProducts_dbo.Orders_Order_Id" FOREIGN KEY ("Order_Id")
      REFERENCES dbo."Orders" ("Id") MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE CASCADE,
  CONSTRAINT "FK_dbo.OrderProducts_dbo.Products_Product_Id" FOREIGN KEY ("Product_Id")
      REFERENCES dbo."Products" ("Id") MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE CASCADE
)
WITH (
  OIDS=FALSE
);


CREATE OR REPLACE FUNCTION dbo.clear_basket()
  RETURNS trigger AS
$BODY$
begin
 insert into "dbo"."OrderProducts" select New."Id", "Product_Id" from "dbo"."UserProducts" where "User_Id" = New."UserId";
 delete from "dbo"."UserProducts" where "User_Id" = New."UserId";
 return New;
end;
$BODY$
  LANGUAGE plpgsql VOLATILE COST 100;
  

CREATE TRIGGER clear_basket
  AFTER INSERT
  ON dbo."Orders"
  FOR EACH ROW
  EXECUTE PROCEDURE dbo.clear_basket();

CREATE ROLE "user" LOGIN PASSWORD 'user';
 GRANT USAGE ON SCHEMA "dbo" to "user";
 GRANT SELECT ON ALL TABLES IN SCHEMA "dbo" TO "user";
 GRANT UPDATE ON "dbo"."Users" TO "user";
 GRANT INSERT ON "dbo"."Users", "dbo"."UserProducts", "dbo"."OrderProducts", "dbo"."Orders" TO "user";
 GRANT UPDATE ON "dbo"."Users" TO "user";
  
CREATE ROLE "admin" LOGIN PASSWORD 'admin';
 GRANT SELECT ON ALL TABLES IN SCHEMA "dbo" TO "admin";
 GRANT INSERT ON "dbo"."Categories", "dbo"."Products", "dbo"."Users" TO "admin";
 GRANT UPDATE ON "dbo"."Categories", "dbo"."Products", "dbo"."Users" TO "admin";
 GRANT DELETE ON "dbo"."Categories", "dbo"."Products", "dbo"."Users" TO "admin";
 
insert into "dbo"."Roles" ("Name", "Connection") values ('admin', 'admin');
insert into "dbo"."Roles" ("Name", "Connection") values ('user', 'user');
 
 
 create or replace function "dbo"."add_product_to_basket"(user_id int, product_id int) 
returns void as $$ 
begin 
insert into "dbo"."UserProducts" values (user_id, product_id); 
end; 
$$ language plpgsql; 

create or replace function "dbo"."delete_one_product_instance_from_basket"(user_id int, product_id int) 
returns void as $$ 
begin 
delete from "dbo"."UserProducts" where "oid" = (select max(oid) from "dbo"."UserProducts" where "User_Id" = user_id and "Product_Id" = product_id); 
end; 
$$ language plpgsql;

CREATE OR REPLACE FUNCTION dbo.get_product_count_from_basket( 
user_id integer, 
product_id integer) 
RETURNS INT AS $$ 
declare c int; 
begin 
c = COUNT(*) from "dbo"."UserProducts" where "Product_Id"=product_id and "User_Id"=user_id; 
return c; 
end; 
$$ LANGUAGE plpgsql; 

CREATE OR REPLACE FUNCTION dbo.get_product_count_from_order( 
order_id integer, 
product_id integer) 
RETURNS INT AS $$ 
declare c int; 
begin 
c = COUNT(*) from "dbo"."OrderProducts" where "Product_Id"=product_id and "Order_Id"=order_id; 
return c; 
end; 
$$ LANGUAGE plpgsql;

GRANT INSERT ON ALL TABLES IN SCHEMA "dbo" TO "admin" ;
GRANT UPDATE ON ALL SEQUENCES IN SCHEMA "dbo" TO "admin";
GRANT DELETE ON "dbo"."UserProducts" TO "admin";
GRANT DELETE ON "dbo"."UserProducts" TO "user";
GRANT UPDATE ON ALL SEQUENCES IN SCHEMA "dbo" TO "user";

alter table "dbo"."Categories" add constraint "check_name" check (CHAR_LENGTH("Name") < 16);
alter table "dbo"."Categories" add constraint "check_name_1" check (CHAR_LENGTH("Name")>4);
alter table "dbo"."Products" add constraint "check_name_pro" check (CHAR_LENGTH("Name")< 16);
alter table "dbo"."Products" add constraint "check_name_pro_2" check (CHAR_LENGTH("Name")>4);
alter table "dbo"."Products" add constraint "check_name_discr" check (CHAR_LENGTH("Description")<128);
alter table "dbo"."Roles" add constraint "check_name_roles1" check (CHAR_LENGTH("Name")<16);
alter table "dbo"."Roles" add constraint "check_name_roles2" check (CHAR_LENGTH("Name")>=4);
alter table "dbo"."Users" add constraint "check_name_users1" check (CHAR_LENGTH("FirstName")<16);
alter table "dbo"."Users" add constraint "check_name_users2" check (CHAR_LENGTH("FirstName")>2);
alter table "dbo"."Users" add constraint "check_name_users3" check (CHAR_LENGTH("LastName")<16);
alter table "dbo"."Users" add constraint "check_name_users4" check (CHAR_LENGTH("LastName")>2);
