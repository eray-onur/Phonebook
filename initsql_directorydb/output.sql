CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    migration_id character varying(150) NOT NULL,
    product_version character varying(32) NOT NULL,
    CONSTRAINT pk___ef_migrations_history PRIMARY KEY (migration_id)
);

START TRANSACTION;
CREATE TABLE contact_infos (
    id uuid NOT NULL,
    person_id uuid NOT NULL,
    phone_number text NOT NULL,
    email text NOT NULL,
    location text NOT NULL,
    description text,
    CONSTRAINT pk_contact_infos PRIMARY KEY (id)
);

CREATE TABLE people (
    id uuid NOT NULL,
    name text NOT NULL,
    last_name text NOT NULL,
    company_name text,
    CONSTRAINT pk_people PRIMARY KEY (id)
);

INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
VALUES ('20250114231041_InitialCreate', '9.0.0');

COMMIT;

