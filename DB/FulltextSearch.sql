CREATE FULLTEXT CATALOG ftCatalog AS DEFAULT;

CREATE FULLTEXT INDEX ON translation_text(text)
    KEY INDEX PK_translation_text 
    ON ftCatalog
    WITH STOPLIST = OFF;
