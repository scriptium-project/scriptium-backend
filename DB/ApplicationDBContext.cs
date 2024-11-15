using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using writings_backend_dotnet.DTOs;
using writings_backend_dotnet.Models;
using writings_backend_dotnet.Models.Util;

namespace writings_backend_dotnet.DB
{
      public class ApplicationDBContext(DbContextOptions dbContextOptions) : DbContext(dbContextOptions)
      {

            public required DbSet<Language> Language { get; set; }

            public required DbSet<Scripture> Scripture { get; set; }

            public required DbSet<ScriptureMeaning> ScriptureMeaning { get; set; }

            public required DbSet<Section> Section { get; set; }

            public required DbSet<SectionMeaning> SectionMeaning { get; set; }

            public required DbSet<Chapter> Chapter { get; set; }

            public required DbSet<ChapterMeaning> ChapterMeaning { get; set; }

            public required DbSet<Root> Root { get; set; }

            public required DbSet<Verse> Verse { get; set; }

            public required DbSet<Word> Word { get; set; }

            public required DbSet<WordMeaning> WordMeaning { get; set; }

            public required DbSet<Transliteration> Transliteration { get; set; }

            public required DbSet<Translator> Translator { get; set; }

            public required DbSet<Translation> Translation { get; set; }

            public required DbSet<TranslatorTranslation> TranslatorTranslation { get; set; }

            public required DbSet<TranslationText> TranslationText { get; set; }

            public required DbSet<FootNoteText> FootNoteText { get; set; }

            public required DbSet<FootNote> FootNote { get; set; }

            public required DbSet<Role> Role { get; set; }

            public required DbSet<User> User { get; set; }

            public required DbSet<Session> Session { get; set; }

            public required DbSet<Collection> Collection { get; set; }

            public required DbSet<CollectionVerse> CollectionVerse { get; set; }

            public required DbSet<Note> Note { get; set; }

            public required DbSet<Comment> Comment { get; set; }

            public required DbSet<CommentVerse> CommentVerse { get; set; }

            public required DbSet<CommentNote> CommentNotes { get; set; }

            public required DbSet<Follow> Follow { get; set; }

            public required DbSet<Block> Block { get; set; }

            public required DbSet<FreezeR> FreezeR { get; set; }

            public required DbSet<Like> Like { get; set; }

            public required DbSet<LikeComment> LikeNote { get; set; }

            public required DbSet<Notification> Notification { get; set; }

            public required DbSet<Cache> Cache { get; set; }

            public required DbSet<Suggestion> Suggestion { get; set; }

            public required DbSet<RequestLog> RequestLog { get; set; }


            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                  base.OnModelCreating(modelBuilder);

                  modelBuilder.Entity<Language>(Language =>
                  {
                        Language.ToTable("language");

                        Language.HasKey(e => e.Id);

                        Language.Property(e => e.Id)
                            .HasColumnName("id")
                            .HasColumnType("smallint")
                            .IsRequired()
                            .ValueGeneratedOnAdd();

                        Language.HasIndex(e => e.LangCode)
                              .IsUnique();

                        Language.HasData(
                                          new Language { Id = 1, LangCode = "en", LangOwn = "English", LangEnglish = "English" },
                                          new Language { Id = 2, LangCode = "de", LangOwn = "Deutsch", LangEnglish = "German" }
                                      );


                  });

                  modelBuilder.Entity<Scripture>(Scripture =>

                  {
                        Scripture.ToTable("scripture");
                        Scripture.HasKey(s => s.Id);

                        Scripture.HasIndex(e => e.Name)
                        .IsUnique();

                        Scripture.HasIndex(e => e.Code)
                        .IsUnique();
                  });

                  modelBuilder.Entity<ScriptureMeaning>(ScriptureMeaning =>
                  {

                        ScriptureMeaning.ToTable("scripture_meaning");

                        ScriptureMeaning.HasKey(s => s.Id);

                        ScriptureMeaning.HasIndex(e => new { e.LanguageId, e.ScriptureId })
                        .IsUnique();

                        ScriptureMeaning.HasOne(c => c.Language)
                        .WithMany(p => p.ScriptureMeanings)
                        .HasForeignKey(c => c.LanguageId)
                        .OnDelete(DeleteBehavior.Cascade);

                        ScriptureMeaning.HasOne(c => c.Scripture)
                        .WithMany(p => p.Meanings)
                        .HasForeignKey(c => c.ScriptureId)
                        .OnDelete(DeleteBehavior.Cascade);
                  });

                  modelBuilder.Entity<Section>(Section =>
                  {

                        Section.ToTable("section");

                        Section.HasKey(s => s.Id);

                        Section.HasIndex(e => e.Name)
                        .IsUnique();

                        Section.HasIndex(e => new { e.ScriptureId, e.Number })
                        .IsUnique();

                        Section.HasOne(c => c.Scripture)
                        .WithMany(scr => scr.Sections)
                        .HasForeignKey(c => c.ScriptureId)
                        .OnDelete(DeleteBehavior.Restrict);
                  });

                  modelBuilder.Entity<SectionMeaning>(SectionMeaning =>
                  {

                        SectionMeaning.ToTable("section_meaning");

                        SectionMeaning.HasKey(s => s.Id);

                        SectionMeaning.HasIndex(e => new { e.LanguageId, e.SectionId })
                                  .IsUnique();

                        SectionMeaning.HasOne(c => c.Language)
                                  .WithMany(e => e.SectionMeanings)
                                  .HasForeignKey(e => e.LanguageId)
                                  .OnDelete(DeleteBehavior.Restrict);

                        SectionMeaning.HasOne(c => c.Section)
                                  .WithMany(e => e.Meanings)
                                  .HasForeignKey(e => e.SectionId)
                                  .OnDelete(DeleteBehavior.Restrict);
                  });

                  modelBuilder.Entity<Chapter>(Chapter =>
                  {
                        Chapter.ToTable("chapter");

                        Chapter.HasKey(c => c.Id);

                        Chapter.HasIndex(c => c.Name)
                                .IsUnique();

                        Chapter.HasIndex(c => new { c.SectionId, c.Number })
                                .IsUnique();

                        Chapter.HasOne(c => c.Section)
                            .WithMany(s => s.Chapters)
                            .HasForeignKey(c => c.SectionId)
                            .OnDelete(DeleteBehavior.Restrict);

                  });

                  modelBuilder.Entity<ChapterMeaning>(ChapterMeaning =>
                  {
                        ChapterMeaning.ToTable("chapter_meaning");

                        ChapterMeaning.HasKey(cm => cm.Id);

                        ChapterMeaning.HasIndex(cm => new { cm.ChapterId, cm.LanguageId })
                        .IsUnique();

                        ChapterMeaning.HasOne(cm => cm.Language)
                        .WithMany(l => l.ChapterMeaning)
                        .HasForeignKey(cm => cm.LanguageId)
                        .OnDelete(DeleteBehavior.Restrict);

                        ChapterMeaning.HasOne(c => c.Chapter)
                        .WithMany(p => p.Meanings)
                        .HasForeignKey(c => c.ChapterId)
                        .OnDelete(DeleteBehavior.Restrict);

                  });

                  modelBuilder.Entity<Root>(Root =>
                  {
                        Root.ToTable("root");

                        Root.HasKey(r => r.Id);

                        Root.HasIndex(r => new { r.Latin, r.ScriptureId })
                        .IsUnique();

                        Root.HasOne(r => r.Scripture)
                        .WithMany(sc => sc.Roots)
                        .HasForeignKey(r => r.ScriptureId)
                        .OnDelete(DeleteBehavior.Restrict);
                  });

                  modelBuilder.Entity<Verse>(Verse =>
                  {
                        Verse.ToTable("verse");

                        Verse.HasKey(v => v.Id);

                        Verse.HasIndex(v => new { v.ChapterId, v.Number })
                        .IsUnique();

                        Verse.HasOne(v => v.Chapter)
                        .WithMany(c => c.Verses)
                        .HasForeignKey(v => v.ChapterId)
                        .OnDelete(DeleteBehavior.Restrict);

                  });

                  modelBuilder.Entity<Word>(Word =>
                  {
                        Word.ToTable("word");

                        Word.HasKey(w => w.Id);

                        Word.HasOne(w => w.Verse)
                        .WithMany(v => v.Words)
                        .HasForeignKey(w => w.VerseId)
                        .OnDelete(DeleteBehavior.Restrict);


                        Word.HasOne(w => w.Root)
                        .WithMany(r => r.Words)
                        .HasForeignKey(w => w.RootId)
                        .OnDelete(DeleteBehavior.Restrict);


                        Word.HasIndex(w => new { w.SequenceNumber, w.VerseId })
                        .IsUnique();
                  });

                  modelBuilder.Entity<WordMeaning>(WordMeaning =>
                  {
                        WordMeaning.ToTable("word_meanings");

                        WordMeaning.HasKey(wm => wm.Id);

                        WordMeaning.Property(wm => wm.Id)
                        .HasColumnName("id")
                        .HasColumnType("bigint")
                        .ValueGeneratedOnAdd()
                        .IsRequired();

                        WordMeaning.Property(wm => wm.Meaning)
                        .HasColumnName("word_meaning")
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(100)
                        .IsRequired();

                        WordMeaning.Property(wm => wm.WordId)
                        .IsRequired();

                        WordMeaning.Property(wm => wm.LanguageId)
                        .IsRequired();

                        WordMeaning.HasOne(wm => wm.Word)
                        .WithMany(w => w.WordMeanings)
                        .HasForeignKey(wm => wm.WordId)
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                        WordMeaning.HasOne(wm => wm.Language)
                        .WithMany(l => l.WordMeanings)
                        .HasForeignKey(wm => wm.LanguageId)
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                  });

                  modelBuilder.Entity<Transliteration>(Transliteration =>
                  {
                        Transliteration.ToTable("transliterations");

                        Transliteration.HasKey(t => t.Id);

                        Transliteration.Property(t => t.Id)
                        .HasColumnName("id")
                        .IsRequired()
                        .ValueGeneratedOnAdd();

                        Transliteration.Property(t => t.Text)
                        .HasColumnType("varchar(1500)")
                        .HasMaxLength(1500)
                        .IsRequired();

                        Transliteration.Property(t => t.LanguageId)
                        .IsRequired();

                        Transliteration.HasOne(t => t.Language)
                        .WithMany(l => l.Transliterations)
                        .HasForeignKey(t => t.LanguageId)
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                        Transliteration.HasOne(t => t.Verse)
                        .WithMany(v => v.Transliterations)
                        .HasForeignKey(t => t.VerseId)
                        .OnDelete(DeleteBehavior.Restrict);
                  });

                  modelBuilder.Entity<Translator>(Translator =>
                  {
                        Translator.ToTable("translators");

                        Translator.HasKey(e => e.Id);

                        Translator.Property(e => e.Id)
                        .HasColumnName("id")
                        .HasColumnType("smallint")
                        .IsRequired()
                        .ValueGeneratedOnAdd();

                        Translator.Property(e => e.Name)
                        .HasColumnName("translator_name")
                        .HasColumnType("varchar(250)")
                        .HasMaxLength(250)
                        .IsRequired();

                        Translator.Property(e => e.Description)
                        .HasColumnName("description")
                        .HasColumnType("varchar(1500)")
                        .HasMaxLength(1500)
                        .IsRequired(false);

                        Translator.Property(e => e.Url)
                        .HasColumnType("varchar(1500)")
                        .HasMaxLength(1500)
                        .IsRequired(false);

                        Translator.Property(e => e.LanguageId)
                        .HasColumnName("language_id")
                        .IsRequired();

                        Translator.HasOne(e => e.Language)
                        .WithMany(l => l.Translators)
                        .HasForeignKey(e => e.LanguageId)
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                        Translator.HasMany(e => e.TranslatorTranslations)
                        .WithOne(tt => tt.Translator)
                        .HasForeignKey(tt => tt.TranslatorId)
                        .OnDelete(DeleteBehavior.Restrict);
                  });

                  modelBuilder.Entity<Translation>(Translation =>
                  {
                        Translation.ToTable("translation");

                        Translation.HasKey(t => t.Id);

                        Translation.Property(t => t.Id)
                        .HasColumnName("id")
                        .HasColumnType("smallint")
                        .IsRequired()
                        .ValueGeneratedOnAdd();

                        Translation.Property(t => t.Name)
                        .HasColumnName("translation_name")
                        .HasColumnType("varchar(300)")
                        .HasMaxLength(300)
                        .IsRequired();

                        Translation.Property(t => t.ProductionTime)
                        .HasColumnName("production_year")
                        .HasColumnType("timestamp")
                        .IsRequired(false);

                        Translation.Property(t => t.AddedAt)
                        .HasColumnName("added_at")
                        .HasColumnType("timestamp")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP")
                        .IsRequired();

                        Translation.Property(t => t.EagerFrom)
                        .HasColumnName("eager_from")
                        .HasColumnType("timestamp")
                        .IsRequired(false);

                        Translation.Property(t => t.LanguageId)
                        .HasColumnName("language_id")
                        .IsRequired()
                        .HasDefaultValue(1);


                        Translation.HasOne(t => t.Language)
                        .WithMany(l => l.Translations)
                        .HasForeignKey(e => e.LanguageId)
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                        Translation.HasMany(t => t.TranslatorTranslations)
                        .WithOne(tt => tt.Translation)
                        .HasForeignKey(t => t.TranslationId)
                        .OnDelete(DeleteBehavior.Restrict);

                        Translation.HasMany(e => e.TranslationTexts)
                        .WithOne(ttx => ttx.Translation)
                        .HasForeignKey(ttx => ttx.TranslationId)
                        .OnDelete(DeleteBehavior.Cascade);
                  });

                  modelBuilder.Entity<TranslatorTranslation>(TranslatorTranslation =>
                  {
                        TranslatorTranslation.ToTable("translator_translation");

                        TranslatorTranslation.HasKey(tt => new { tt.TranslatorId, tt.TranslationId });

                        TranslatorTranslation.Property(tt => tt.TranslatorId)
                        .HasColumnName("translator_id")
                        .HasColumnType("smallint")
                        .IsRequired();

                        TranslatorTranslation.Property(tt => tt.TranslationId)
                        .HasColumnName("translation_id")
                        .HasColumnType("smallint")
                        .IsRequired();

                        TranslatorTranslation.Property(tt => tt.AssignedOn)
                        .HasColumnName("assigned_on")
                        .HasColumnType("timestamp")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP")
                        .IsRequired();


                        TranslatorTranslation.HasOne(tt => tt.Translator)
                        .WithMany(tr => tr.TranslatorTranslations)
                        .HasForeignKey(tt => tt.TranslatorId)
                        .OnDelete(DeleteBehavior.Cascade);

                        TranslatorTranslation.HasOne(tt => tt.Translation)
                        .WithMany(tr => tr.TranslatorTranslations)
                        .HasForeignKey(tt => tt.TranslationId)
                        .OnDelete(DeleteBehavior.Cascade);
                  });

                  modelBuilder.Entity<TranslationText>(TranslationText =>
                  {
                        TranslationText.ToTable("translation_text");

                        TranslationText.HasKey(tt => tt.Id);

                        TranslationText.Property(tt => tt.Id)
                        .HasColumnName("id")
                        .HasColumnType("bigint")
                        .IsRequired()
                        .ValueGeneratedOnAdd();

                        TranslationText.Property(tt => tt.Text)
                        .IsRequired()
                        .HasMaxLength(4000);

                        TranslationText.Property(tt => tt.TranslationId)
                        .HasColumnName("translation_id")
                        .HasColumnType("smallint")
                        .IsRequired();

                        TranslationText.Property(tt => tt.VerseId)
                        .HasColumnName("verse_id")
                        .HasColumnType("int")
                        .IsRequired();


                        TranslationText.HasOne(tt => tt.Translation)
                        .WithMany(tr => tr.TranslationTexts)
                        .HasForeignKey(tt => tt.TranslationId)
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                        TranslationText.HasOne(tt => tt.Verse)
                        .WithMany(v => v.TranslationTexts)
                        .HasForeignKey(tt => tt.VerseId)
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                        TranslationText.HasMany(tt => tt.FootNotes)
                        .WithOne(fn => fn.TranslationText)
                        .HasForeignKey(fn => fn.TranslationTextId)
                        .OnDelete(DeleteBehavior.Cascade);
                  });

                  modelBuilder.Entity<FootNoteText>(FootNoteText =>
                  {
                        FootNoteText.ToTable("footnote_text");

                        FootNoteText.HasKey(ft => ft.Id);

                        FootNoteText.Property(ft => ft.Id)
                        .HasColumnName("id")
                        .HasColumnType("bigint")
                        .IsRequired()
                        .ValueGeneratedOnAdd();

                        FootNoteText.Property(ft => ft.Text)
                        .IsRequired()
                        .HasMaxLength(4000);

                  });

                  modelBuilder.Entity<FootNote>(FootNote =>
                  {

                        FootNote.ToTable("footnote");

                        FootNote.HasKey(fn => fn.Id);

                        FootNote.Property(fn => fn.Id)
                        .HasColumnName("id")
                        .HasColumnType("bigint")
                        .IsRequired()
                        .ValueGeneratedOnAdd();

                        FootNote.Property(fn => fn.Number)
                        .HasColumnName("number")
                        .HasColumnType("smallint")
                        .IsRequired();

                        FootNote.Property(fn => fn.Index)
                        .HasColumnName("index")
                        .HasColumnType("smallint")
                        .IsRequired();

                        FootNote.HasOne(fn => fn.TranslationText)
                          .WithMany(tt => tt.FootNotes)
                          .HasForeignKey(fn => fn.TranslationTextId)
                          .OnDelete(DeleteBehavior.Cascade);

                        FootNote.HasOne(fn => fn.FootNoteText)
                          .WithMany(fnt => fnt.FootNotes)
                          .HasForeignKey(fn => fn.FootNoteTextId)
                          .OnDelete(DeleteBehavior.Cascade);

                        FootNote.HasIndex(fn => new { fn.Number, fn.FootNoteTextId, fn.Index });
                  });

                  modelBuilder.Entity<Role>(Role =>
                  {
                        Role.ToTable("role");

                        Role.HasKey(r => r.Id);

                        Role.HasIndex(r => r.RoleName).IsUnique();

                        Role.Property(r => r.RoleName).IsRequired();

                        Role.HasData(
                      new Role { Id = 1, RoleName = "Admin" },
                      new Role { Id = 2, RoleName = "Verified" }
                  );
                  });

                  modelBuilder.Entity<User>(User =>
                  {
                        User.ToTable("user");

                        User.HasKey(e => e.Id);

                        User.Property(e => e.Id)
                        .HasColumnName("id")
                        .HasColumnType("uuid")
                        .IsRequired()
                        .HasDefaultValueSql("gen_random_uuid()");

                        User.Property(e => e.Username)
                              .HasMaxLength(24)
                              .IsRequired(); //TODO: Add case insensitive by LOWER(username)

                        User.Property(e => e.Name)
                        .HasMaxLength(30)
                        .IsRequired();

                        User.Property(e => e.Surname)
                        .HasMaxLength(30)
                        .IsRequired();

                        User.Property(e => e.Gender)
                        .HasMaxLength(1)
                        .IsRequired(false);

                        User.Property(e => e.Biography)
                        .HasMaxLength(200)
                        .IsRequired(false);

                        User.Property(e => e.Email)
                        .HasMaxLength(255)
                        .IsRequired();

                        User.Property(e => e.EmailVerified)
                        .HasColumnType("timestamp")
                        .IsRequired(false);

                        User.Property(e => e.Password)
                        .HasMaxLength(255)
                        .IsRequired();

                        User.Property(e => e.CreatedAt)
                        .HasColumnName("created_at")
                        .HasColumnType("timestamp")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP")
                        .IsRequired();

                        User.Property(e => e.LastActive)
                        .HasColumnName("last_active")
                        .HasColumnType("timestamp")
                        .IsRequired(false);

                        User.Property(e => e.IsFrozen)
                        .HasColumnName("is_frozen")
                        .HasColumnType("timestamp")
                        .IsRequired(false);

                        User.Property(e => e.IsPrivate)
                        .HasColumnName("is_private")
                        .HasColumnType("timestamp")
                        .IsRequired(false);

                        User.Property(e => e.RoleId)
                        .HasColumnName("role_id")
                        .HasColumnType("smallint")
                        .IsRequired(false);

                        User.Property(e => e.PreferredLanguageId)
                        .HasColumnName("preferred_languageId")
                        .HasColumnType("smallint")
                        .HasDefaultValue(1)
                        .IsRequired();

                        User.HasIndex(e => e.Username)
                        .IsUnique();

                        User.HasIndex(e => e.Email)
                        .IsUnique();

                        User.HasOne(e => e.Role)
                          .WithMany(r => r.Users)
                          .HasForeignKey(e => e.RoleId)
                          .OnDelete(DeleteBehavior.Restrict);

                        User.HasOne(e => e.PreferredLanguage)
                        .WithMany(l => l.PreferredUsers)
                        .HasForeignKey(e => e.PreferredLanguageId)
                        .OnDelete(DeleteBehavior.Restrict); // In Database, this behavior has been implemented.

                  });

                  modelBuilder.Entity<Session>(Session =>
                  {
                        Session.ToTable("session");

                        Session.HasKey(e => e.Id);

                        Session.Property(e => e.Id)
                        .HasColumnName("id")
                        .HasColumnType("varchar(100)")
                        .IsRequired();

                        Session.Property(e => e.UserId)
                        .HasColumnName("user_id")
                        .HasColumnType("uuid")
                        .IsRequired(false);

                        Session.Property(e => e.ExpiresAt)
                        .HasColumnName("expires_at")
                        .HasColumnType("timestamp")
                        .IsRequired();

                        Session.Property(e => e.SessionData)
                        .HasColumnName("session")
                        .HasColumnType("jsonb")
                        .IsRequired();

                        Session.HasOne(e => e.User)
                        .WithMany(u => u.Sessions)
                        .HasForeignKey(e => e.UserId)
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired(false);
                  });

                  modelBuilder.Entity<Collection>(Collection =>
                  {
                        Collection.ToTable("collection");

                        Collection.HasKey(e => e.Id);

                        Collection.Property(e => e.Id)
                        .HasColumnName("id")
                        .HasColumnType("uuid")
                        .IsRequired()
                        .HasDefaultValueSql("gen_random_uuid()");

                        Collection.Property(e => e.Name)
                        .HasColumnName("name")
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(100)
                        .IsRequired()
                        .HasDefaultValue(string.Empty);

                        Collection.Property(e => e.Description)
                        .HasColumnName("description")
                        .HasColumnType("varchar(250)")
                        .HasMaxLength(250)
                        .IsRequired(false);

                        Collection.Property(e => e.UserId)
                        .HasColumnName("user_id")
                        .HasColumnType("uuid")
                        .IsRequired();

                        Collection.Property(e => e.CreatedAt)
                        .HasColumnName("created_at")
                        .HasColumnType("timestamp")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP")
                        .IsRequired();

                        Collection.HasIndex(e => new { e.UserId, e.Name })
                        .IsUnique();

                        Collection.HasOne(e => e.User)
                        .WithMany(u => u.Collections)
                        .HasForeignKey(e => e.UserId)
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                  });

                  modelBuilder.Entity<CollectionVerse>(CollectionVerse =>
                  {
                        CollectionVerse.ToTable("collection_verse");

                        CollectionVerse.HasKey(e => e.Id);

                        CollectionVerse.Property(e => e.Id)
                        .HasColumnName("id")
                        .HasColumnType("bigint");

                        CollectionVerse.Property(e => e.CollectionId)
                        .HasColumnName("collection_id")
                        .HasColumnType("uuid")
                        .IsRequired();

                        CollectionVerse.Property(e => e.VerseId)
                        .HasColumnName("verse_id")
                        .HasColumnType("integer")
                        .IsRequired();

                        CollectionVerse.Property(e => e.SavedAt)
                        .HasColumnName("saved_at")
                        .HasColumnType("timestamp")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP")
                        .IsRequired();

                        CollectionVerse.Property(e => e.Note)
                        .HasColumnName("note")
                        .HasColumnType("varchar(250)")
                        .HasMaxLength(250)
                        .IsRequired(false);

                        CollectionVerse.HasIndex(e => new { e.CollectionId, e.VerseId })
                        .IsUnique();

                        CollectionVerse.HasOne(e => e.Collection)
                        .WithMany(c => c.Verses)
                        .HasForeignKey(e => e.CollectionId)
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                        CollectionVerse.HasOne(e => e.Verse)
                        .WithMany(v => v.CollectionVerses)
                        .HasForeignKey(e => e.VerseId)
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                  });

                  modelBuilder.Entity<Note>(Note =>
                  {
                        Note.ToTable("note");

                        Note.HasKey(e => e.Id);

                        Note.Property(e => e.Id)
                        .HasColumnName("id")
                        .HasColumnType("bigint");

                        Note.Property(e => e.UserId)
                        .HasColumnName("user_id")
                        .HasColumnType("uuid")
                        .IsRequired();

                        Note.Property(e => e.Text)
                        .HasColumnName("text")
                        .HasColumnType("text")
                        .IsRequired();

                        Note.Property(e => e.VerseId)
                        .HasColumnName("verse_id")
                        .HasColumnType("integer")
                        .IsRequired();

                        Note.Property(e => e.CreatedAt)
                        .HasColumnName("created_at")
                        .HasColumnType("timestamp")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP")
                        .IsRequired()
                        .ValueGeneratedOnAdd();

                        Note.Property(e => e.UpdatedAt)
                        .HasColumnName("updated_at")
                        .HasColumnType("timestamp")
                        .IsRequired(false);


                        Note.HasOne(e => e.User)
                        .WithMany(u => u.Notes)
                        .HasForeignKey(e => e.UserId)
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                        Note.HasOne(e => e.Verse)
                        .WithMany(v => v.Notes)
                        .HasForeignKey(e => e.VerseId)
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                  });

                  modelBuilder.Entity<Comment>(Comment =>
                  {
                        Comment.ToTable("comment");

                        Comment.HasKey(e => e.Id);

                        Comment.Property(e => e.Id)
                        .HasColumnName("id")
                        .HasColumnType("bigint");

                        Comment.Property(e => e.UserId)
                        .HasColumnName("user_id")
                        .HasColumnType("uuid")
                        .IsRequired();

                        Comment.Property(e => e.Text)
                        .HasColumnName("text")
                        .HasColumnType("varchar(500)")
                        .HasMaxLength(500)
                        .IsRequired();

                        Comment.Property(e => e.CreatedAt)
                        .HasColumnName("created_at")
                        .HasColumnType("timestamp")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP")
                        .IsRequired()
                        .ValueGeneratedOnAdd();

                        Comment.Property(e => e.UpdatedAt)
                        .HasColumnName("updated_at")
                        .HasColumnType("timestamp")
                        .IsRequired(false);

                        Comment.Property(e => e.ParentCommentId)
                        .HasColumnName("parent_comment_id")
                        .HasColumnType("bigint");


                        Comment.HasOne(e => e.User)
                              .WithMany(u => u.Comments)
                              .HasForeignKey(e => e.UserId)
                              .OnDelete(DeleteBehavior.Cascade);

                        Comment.HasOne(e => e.ParentComment)
                              .WithMany(c => c.Replies)
                              .HasForeignKey(e => e.ParentCommentId)
                              .OnDelete(DeleteBehavior.Cascade); //TODO: ON DELETE CASCADE;

                        Comment.HasOne(c => c.CommentVerse)
                            .WithOne(cv => cv.Comment)
                            .HasForeignKey<CommentVerse>(cv => cv.CommentId);

                        Comment.HasOne(c => c.CommentNote)
                            .WithOne(cn => cn.Comment)
                            .HasForeignKey<CommentNote>(cn => cn.CommentId);


                  });

                  modelBuilder.Entity<CommentVerse>(CommentVerse =>
                  {
                        CommentVerse.ToTable("comment_verse");

                        CommentVerse.HasKey(e => e.CommentId);

                        CommentVerse.Property(e => e.CommentId)
                        .HasColumnName("comment_id")
                        .HasColumnType("bigint");

                        CommentVerse.Property(e => e.VerseId)
                        .HasColumnName("verse_id")
                        .HasColumnType("integer")
                        .IsRequired();

                        CommentVerse.HasOne(e => e.Verse)
                        .WithMany(v => v.Comments)
                        .HasForeignKey(e => e.VerseId)
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                        CommentVerse.HasOne(c => c.Comment)
                      .WithOne(cn => cn.CommentVerse)
                      .HasForeignKey<CommentVerse>(cn => cn.CommentId);
                  });

                  modelBuilder.Entity<CommentNote>(CommentNote =>
                  {
                        CommentNote.ToTable("comment_note");

                        CommentNote.HasKey(e => e.CommentId);

                        CommentNote.Property(e => e.CommentId)
                        .HasColumnName("comment_id")
                        .HasColumnType("bigint");

                        CommentNote.Property(e => e.NoteId)
                        .HasColumnName("note_id")
                        .HasColumnType("integer")
                        .IsRequired();

                        CommentNote.HasOne(e => e.Note)
                        .WithMany(v => v.Comments)
                        .HasForeignKey(e => e.NoteId)
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                        CommentNote.HasOne(c => c.Comment)
                      .WithOne(cn => cn.CommentNote)
                      .HasForeignKey<CommentNote>(cn => cn.CommentId);
                  });

                  modelBuilder.Entity<Follow>(Follow =>
                  {
                        Follow.ToTable("follow");

                        Follow.HasKey(e => e.Id);

                        Follow.Property(e => e.Id)
                            .HasColumnName("id")
                            .IsRequired()
                            .ValueGeneratedOnAdd();

                        Follow.Property(e => e.FollowerId)
                            .HasColumnName("follower_id")
                            .IsRequired();

                        Follow.Property(e => e.FollowedId)
                            .HasColumnName("followed_id")
                            .IsRequired();

                        Follow.Property(e => e.Status)
                            .HasColumnName("status")
                            .IsRequired()
                            .HasConversion<string>()
                            .HasDefaultValue(FollowStatus.Accepted);

                        Follow.Property(e => e.OccurredAt)
                            .HasColumnName("occurred_at")
                            .IsRequired()
                            .HasColumnType("timestamp")
                            .HasDefaultValueSql("CURRENT_TIMESTAMP");

                        Follow.HasIndex(e => new { e.FollowerId, e.FollowedId })
                            .IsUnique();

                        Follow.HasOne(e => e.Follower)
                            .WithMany(u => u.Following)
                            .HasForeignKey(e => e.FollowerId)
                            .OnDelete(DeleteBehavior.NoAction); //TODO: ON DELETE CASCADE

                        Follow.HasOne(e => e.Followed)
                            .WithMany(u => u.Followers)
                            .HasForeignKey(e => e.FollowedId)
                            .OnDelete(DeleteBehavior.NoAction); //TODO: ON DELETE CASCADE
                  });

                  modelBuilder.Entity<Block>(Block =>
                  {
                        Block.ToTable("block");

                        Block.HasKey(e => e.Id);

                        Block.Property(e => e.Id)
                        .HasColumnName("id")
                        .IsRequired()
                        .ValueGeneratedOnAdd();

                        Block.Property(e => e.BlockerId)
                        .HasColumnName("blocker_id")
                        .IsRequired();

                        Block.Property(e => e.BlockedId)
                        .HasColumnName("blocked_id")
                        .IsRequired();

                        Block.Property(e => e.BlockedAt)
                        .HasColumnName("blocked_at")
                        .HasColumnType("timestamp")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                        Block.Property(e => e.Reason)
                        .HasColumnName("reason")
                        .HasMaxLength(100);

                        Block.HasIndex(e => new { e.BlockerId, e.BlockedId })
                        .IsUnique();

                        Block.HasOne(e => e.Blocker)
                        .WithMany(u => u.BlockedUsers)
                        .HasForeignKey(e => e.BlockerId)
                        .OnDelete(DeleteBehavior.NoAction); //TODO: ON DELETE CASCADE

                        Block.HasOne(e => e.Blocked)
                        .WithMany(u => u.BlockedByUsers)
                        .HasForeignKey(e => e.BlockedId)
                        .OnDelete(DeleteBehavior.NoAction); //TODO: ON DELETE CASCADE

                  });

                  modelBuilder.Entity<FreezeR>(FreezeR =>
                  {
                        FreezeR.ToTable("freeze_r");

                        FreezeR.HasKey(e => e.Id);

                        FreezeR.Property(e => e.Id)
                        .HasColumnName("id")
                        .IsRequired()
                        .ValueGeneratedOnAdd();

                        FreezeR.Property(e => e.Status)
                        .HasColumnName("status")
                        .IsRequired()
                        .HasConversion<string>()
                        .HasDefaultValue(FreezeStatus.Frozen);

                        FreezeR.Property(e => e.UserId)
                        .HasColumnName("user_id")
                        .IsRequired();

                        FreezeR.Property(e => e.ProceedAt)
                        .HasColumnName("proceed_at")
                        .IsRequired()
                        .HasColumnType("timestamp")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                        FreezeR.HasOne(e => e.User)
                        .WithMany(u => u.FreezeRecords)
                        .HasForeignKey(e => e.UserId)
                        .OnDelete(DeleteBehavior.Cascade);
                  });

                  modelBuilder.Entity<Like>(Like =>
                 {
                       Like.ToTable("lke");

                       Like.HasKey(e => e.Id);

                       Like.Property(e => e.Id)
                        .HasColumnName("id")
                        .IsRequired()
                        .ValueGeneratedOnAdd();

                       Like.Property(e => e.UserId)
                        .HasColumnName("user_id")
                        .IsRequired();

                       Like.Property(e => e.CreatedAt)
                        .HasColumnName("created_at")
                        .HasColumnType("timestamp")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                       Like.HasIndex(e => e.UserId);

                       Like.HasOne(e => e.User)
                        .WithMany(u => u.Likes)
                        .HasForeignKey(e => e.UserId)
                        .OnDelete(DeleteBehavior.Cascade);

                       Like.HasOne(e => e.LikeComment)
                        .WithOne(lc => lc.Like)
                        .HasForeignKey<LikeComment>(lc => lc.LikeId)
                        .OnDelete(DeleteBehavior.Cascade);

                       Like.HasOne(e => e.LikeNote)
                        .WithOne(ln => ln.Like)
                        .HasForeignKey<LikeNote>(ln => ln.LikeId)
                        .OnDelete(DeleteBehavior.Cascade);
                 });

                  modelBuilder.Entity<LikeComment>(LikeComment =>
                 {
                       LikeComment.ToTable("like_comment");

                       LikeComment.HasKey(e => e.LikeId);

                       LikeComment.Property(e => e.LikeId)
                        .HasColumnName("likeId")
                        .IsRequired();

                       LikeComment.Property(e => e.CommentId)
                        .HasColumnName("comment_id")
                        .IsRequired();

                       LikeComment.HasIndex(e => e.CommentId);

                       LikeComment.HasOne(e => e.Comment)
                        .WithMany(c => c.LikeComments)
                        .HasForeignKey(e => e.CommentId)
                        .OnDelete(DeleteBehavior.Cascade);

                       LikeComment.HasIndex(e => new { e.LikeId, e.CommentId })
                        .IsUnique();
                 });

                  modelBuilder.Entity<LikeNote>(LikeNote =>
                  {
                        LikeNote.ToTable("like_note");

                        LikeNote.HasKey(e => e.LikeId);

                        LikeNote.Property(e => e.LikeId)
                        .HasColumnName("likeId")
                        .IsRequired();

                        LikeNote.Property(e => e.NoteId)
                        .HasColumnName("note_id")
                        .IsRequired();

                        LikeNote.HasIndex(e => e.NoteId);

                        LikeNote.HasOne(e => e.Note)
                        .WithMany(n => n.LikeNotes)
                        .HasForeignKey(e => e.NoteId)
                        .OnDelete(DeleteBehavior.Cascade);

                        LikeNote.HasIndex(e => new { e.LikeId, e.NoteId })
                      .IsUnique();
                  });

                  modelBuilder.Entity<Notification>(Notification =>
                  {
                        Notification.ToTable("notification");

                        Notification.HasKey(e => e.Id);

                        Notification.Property(e => e.Id)
                        .HasColumnName("id")
                        .IsRequired()
                        .ValueGeneratedOnAdd();

                        Notification.Property(e => e.RecipientId)
                        .HasColumnName("recipient_id")
                        .IsRequired();

                        Notification.Property(e => e.ActorId)
                        .HasColumnName("actor_id")
                        .IsRequired();

                        Notification.Property(e => e.NotificationType)
                        .HasColumnName("notification_type")
                        .IsRequired()
                        .HasConversion<string>();

                        Notification.Property(e => e.EntityType)
                        .HasColumnName("entity_type")
                        .HasConversion<string>();

                        Notification.Property(e => e.EntityId)
                        .HasColumnName("entity_id");

                        Notification.Property(e => e.CreatedAt)
                        .HasColumnName("created_at")
                        .HasColumnType("timestamp")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                        Notification.Property(e => e.IsRead)
                        .HasColumnName("is_read")
                        .HasDefaultValue(false);

                        Notification.HasOne(e => e.Recipient)
                        .WithMany(u => u.NotificationsReceived)
                        .HasForeignKey(e => e.RecipientId)
                        .OnDelete(DeleteBehavior.NoAction); //TODO: ON DELETE CASCADE

                        Notification.HasOne(e => e.Actor)
                        .WithMany(u => u.NotificationsSent)
                        .HasForeignKey(e => e.ActorId)
                        .OnDelete(DeleteBehavior.NoAction); //TODO: ON DELETE CASCADE

                        //TODO: Add check "recipient_id <> actor_id AND ((entity_type IS NOT NULL AND entity_id IS NOT NULL) OR (entity_type IS NULL AND entity_id IS NULL))");
                  });

                  modelBuilder.Entity<Cache>(Cache =>
                  {
                        Cache.ToTable("cache");

                        Cache.HasKey(e => e.Id);

                        Cache.Property(e => e.Key)
                        .IsRequired()
                        .HasMaxLength(126);

                        Cache.Property(e => e.Data)
                        .HasColumnType("jsonb").IsRequired();

                        Cache.Property(e => e.ExpirationDate)
                        .HasColumnType("timestamp").HasDefaultValueSql("CURRENT_TIMESTAMP").IsRequired();
                  });

                  modelBuilder.Entity<Suggestion>(Suggestion =>
                      {
                            Suggestion.ToTable("suggestion");

                            Suggestion.HasKey(e => e.Id);

                            Suggestion.Property(e => e.UserId)
                              .IsRequired();

                            Suggestion.Property(e => e.TranslationTextId)
                              .IsRequired();

                            Suggestion.Property(e => e.SuggestionText)
                              .IsRequired()
                              .HasMaxLength(500);

                            Suggestion.Property(e => e.CreatedAt)
                              .HasDefaultValueSql("CURRENT_TIMESTAMP");

                            Suggestion.HasIndex(e => new { e.UserId, e.TranslationTextId })
                              .IsUnique();

                            Suggestion.HasOne(e => e.User)
                              .WithMany()
                              .HasForeignKey(e => e.UserId)
                              .OnDelete(DeleteBehavior.Cascade);

                            Suggestion.HasOne(e => e.TranslationText)
                              .WithMany()
                              .HasForeignKey(e => e.TranslationTextId)
                              .OnDelete(DeleteBehavior.Cascade);
                      });

                  modelBuilder.Entity<RequestLog>(RequestLog =>
                  {
                        RequestLog.ToTable("request_logs");

                        RequestLog.HasKey(e => e.Id);

                        RequestLog.Property(e => e.Identifier)
                              .IsRequired()
                              .HasMaxLength(126);

                        RequestLog.Property(e => e.Endpoint)
                              .IsRequired()
                              .HasMaxLength(126);

                        RequestLog.Property(e => e.Method)
                              .IsRequired()
                              .HasMaxLength(10);

                        RequestLog.Property(e => e.StatusCode)
                              .IsRequired();

                        RequestLog.Property(e => e.OccurredAt)
                              .IsRequired()
                              .HasDefaultValueSql("NOW()");
                  });


            }

      }


}