CREATE FUNCTION dbo.GetVerseCommentHierarchy
(
    @UserId UNIQUEIDENTIFIER,
    @VerseId BIGINT
)
RETURNS TABLE
AS
RETURN
(
    WITH comment_users AS (
        SELECT 
            id AS user_id, 
            username, 
            CASE WHEN is_private IS NOT NULL THEN 1 ELSE 0 END AS is_private, 
            is_frozen
        FROM [user]
    ),
    like_counts AS (
        SELECT 
            comment_id, 
            COUNT(*) AS like_count
        FROM [dbo].like_comment
        GROUP BY comment_id
    ),
    comment_hierarchy AS (
        -- Base case
        SELECT
            c.id,
            c.user_id,
            cu.username,
            cu.is_private,
            c.text,
            c.created_at,
            c.updated_at,
            c.parent_comment_id,
            c.CommentVerseId,
            c.CommentNoteId
        FROM
            [dbo].comment c
        INNER JOIN comment_users cu ON c.user_id = cu.user_id
        WHERE
            c.parent_comment_id IS NULL
            AND (
                c.user_id = @UserId
                OR EXISTS (
                    SELECT 1 
                    FROM [dbo].follow f
                    WHERE f.follower_id = @UserId 
                      AND f.followed_id = c.user_id 
                      AND f.status = 1
                )
            )

        UNION ALL

        -- Recursive case
        SELECT
            c.id,
            c.user_id,
            cu.username,
            cu.is_private,
            c.text,
            c.created_at,
            c.updated_at,
            c.parent_comment_id,
            c.CommentVerseId,
            c.CommentNoteId
        FROM
            [dbo].comment c
        INNER JOIN comment_hierarchy ch ON c.parent_comment_id = ch.id
        INNER JOIN comment_users cu ON c.user_id = cu.user_id
        WHERE
            
                -- Condition 1: Comments by the user
                (
                    ch.user_id = @UserId 
                    AND (
                        cu.is_private = 0
                        OR (
                            cu.is_private = 1
                            AND EXISTS (
                                SELECT 1 
                                FROM [dbo].follow f 
                                WHERE f.follower_id = @UserId 
                                  AND f.followed_id = c.user_id 
                                  AND f.status = 1
                            )
                        )
                    )
                    AND cu.is_frozen IS NULL
                )
                OR
                -- Condition 2: Comments on followed users' comments
                (
                    ch.user_id != @UserId
                    AND EXISTS (
                        SELECT 1 
                        FROM [dbo].follow f
                        WHERE f.follower_id = @UserId 
                          AND f.followed_id = ch.user_id 
                          AND f.status = 1
                    )
                    AND EXISTS (
                        SELECT 1
                        FROM [dbo].follow f2
                        WHERE f2.follower_id = @UserId
                          AND f2.followed_id = c.user_id
                          AND f2.status = 1
                    )
                )
                OR
                -- Condition 3: User's own reply
                (
                    ch.id = c.parent_comment_id 
                    AND c.user_id = @UserId
                )
            
    )
    SELECT
        ch.id,
        ch.user_id,
        ch.username,
        ch.is_private,
        ch.text,
        ch.created_at,
        ch.updated_at,
        ch.parent_comment_id,
        ch.CommentVerseId,
        ch.CommentNoteId
    FROM
        comment_hierarchy ch
    LEFT JOIN like_counts lc ON ch.id = lc.comment_id
    LEFT JOIN [dbo].comment_verse cv ON cv.comment_id = ch.id
    WHERE cv.verse_id = @VerseId
);


CREATE FUNCTION dbo.GetNoteCommentHierarchy
(
    @UserId UNIQUEIDENTIFIER,
    @NoteId BIGINT
)
RETURNS TABLE
AS
RETURN
(
    WITH comment_users AS (
        SELECT 
            id AS user_id, 
            username, 
            CASE WHEN is_private IS NOT NULL THEN 1 ELSE 0 END AS is_private, 
            is_frozen
        FROM [user]
    ),
    like_counts AS (
        SELECT 
            comment_id, 
            COUNT(*) AS like_count
        FROM [dbo].like_comment
        GROUP BY comment_id
    ),
    comment_hierarchy AS (
        -- Base case
        SELECT
            c.id,
            c.user_id,
            cu.username,
            cu.is_private,
            c.text,
            c.created_at,
            c.updated_at,
            c.parent_comment_id,
            c.CommentVerseId,
            c.CommentNoteId
        FROM
            [dbo].comment c
        INNER JOIN comment_users cu ON c.user_id = cu.user_id
        WHERE
            c.parent_comment_id IS NULL
            AND (
                c.user_id = @UserId
                OR EXISTS (
                    SELECT 1 
                    FROM [dbo].follow f
                    WHERE f.follower_id = @UserId 
                      AND f.followed_id = c.user_id 
                      AND f.status = 1
                )
            )

        UNION ALL

        -- Recursive case
        SELECT
            c.id,
            c.user_id,
            cu.username,
            cu.is_private,
            c.text,
            c.created_at,
            c.updated_at,
            c.parent_comment_id,
            c.CommentVerseId,
            c.CommentNoteId
        FROM
            [dbo].comment c
        INNER JOIN comment_hierarchy ch ON c.parent_comment_id = ch.id
        INNER JOIN comment_users cu ON c.user_id = cu.user_id
        WHERE
            
                -- Condition 1: Comments by the user
                (
                    ch.user_id = @UserId 
                    AND (
                        cu.is_private = 0
                        OR (
                            cu.is_private = 1
                            AND EXISTS (
                                SELECT 1 
                                FROM [dbo].follow f 
                                WHERE f.follower_id = @UserId 
                                  AND f.followed_id = c.user_id 
                                  AND f.status = 1
                            )
                        )
                    )
                    AND cu.is_frozen IS NULL
                )
                OR
                -- Condition 2: Comments on followed users' comments
                (
                    ch.user_id != @UserId
                    AND EXISTS (
                        SELECT 1 
                        FROM [dbo].follow f
                        WHERE f.follower_id = @UserId 
                          AND f.followed_id = ch.user_id 
                          AND f.status = 1
                    )
                    AND EXISTS (
                        SELECT 1
                        FROM [dbo].follow f2
                        WHERE f2.follower_id = @UserId
                          AND f2.followed_id = c.user_id
                          AND f2.status = 1
                    )
                )
                OR
                -- Condition 3: User's own reply
                (
                    ch.id = c.parent_comment_id 
                    AND c.user_id = @UserId
                )
            
    )
    SELECT
        ch.id,
        ch.user_id,
        ch.username,
        ch.is_private,
        ch.text,
        ch.created_at,
        ch.updated_at,
        ch.parent_comment_id,
        ch.CommentVerseId,
        ch.CommentNoteId
    FROM
        comment_hierarchy ch
    LEFT JOIN like_counts lc ON ch.id = lc.comment_id
    LEFT JOIN [dbo].comment_note cn ON cn.comment_id = ch.id
    WHERE cn.note_id = @NoteId
);
