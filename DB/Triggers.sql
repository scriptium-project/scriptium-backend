CREATE TRIGGER trg_CreateCollectionOnUserInsert 
ON dbo.[User]
AFTER INSERT
AS
BEGIN

    INSERT INTO dbo.Collection ([id], [name], [user_id], [created_at])
    SELECT NEWID(), 'Default collection' , i.Id, GETUTCDATE()
    FROM inserted i
    WHERE NOT EXISTS (
        SELECT 1 
        FROM dbo.Collection c
        WHERE c.user_id = i.Id
    );
END;
