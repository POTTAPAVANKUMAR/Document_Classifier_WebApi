
CREATE DATABASE document_classifier;
CREATE USER doc_user WITH PASSWORD 'Password123';
GRANT ALL PRIVILEGES ON DATABASE document_classifier TO doc_user;

CREATE TABLE processed_documents (
    id SERIAL PRIMARY KEY,
    file_name TEXT NOT NULL,
    ocr_text TEXT,
    ai_result_json JSONB,
    processed_at TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP
);

GRANT INSERT ON TABLE processed_documents TO doc_user;

GRANT SELECT, UPDATE, DELETE ON TABLE processed_documents TO doc_user;

GRANT USAGE ON SCHEMA public TO doc_user;

GRANT USAGE, SELECT ON SEQUENCE processed_documents_id_seq TO doc_user;
