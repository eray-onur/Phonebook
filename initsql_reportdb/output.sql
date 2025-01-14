CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    migration_id character varying(150) NOT NULL,
    product_version character varying(32) NOT NULL,
    CONSTRAINT pk___ef_migrations_history PRIMARY KEY (migration_id)
);

START TRANSACTION;
CREATE TABLE reports (
    id uuid NOT NULL,
    request_date timestamp with time zone NOT NULL,
    report_status integer NOT NULL,
    location text NOT NULL,
    person_count bigint,
    phone_count bigint,
    CONSTRAINT pk_reports PRIMARY KEY (id)
);

INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
VALUES ('20250114231113_InitialCreate', '9.0.0');

COMMIT;

