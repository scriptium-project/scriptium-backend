using scriptium_backend_dotnet.Models;

namespace scriptium_backend_dotnet.DTOs
{

    public class RootDTO
    {
        public required string Latin { get; set; }

        public required string Own { get; set; }

        public required List<VerseExpendedWordDTO> Verses { get; set; }

        public required List<TranslationWithMultiTextDTO> Translations { get; set; }
    }

    public class RootSimpleDTO
    {
        public required string Latin { get; set; }
        public required string Own { get; set; }
    }

    public class RootExpandedDTO
    {
        public required string Latin { get; set; }
        public required string Own { get; set; }
        public List<WordRootDTO>? Words { get; set; } = [];
        public required int WordCount { get; set; } = -1;
    }

    public static class RootExtensions
    {
        public static RootSimpleDTO ToRootSimpleDTO(this Root root)
        {
            return new RootSimpleDTO
            {
                Latin = root.Latin,
                Own = root.Own,
            };
        }

        public static RootExpandedDTO ToRootExpandedDTO(this Root root)
        {
            return new RootExpandedDTO
            {
                Latin = root.Latin,
                Own = root.Own,
                Words = root.Words.Select(w => w.ToWordRootDTO()).ToList(),
                WordCount = root.WordCount
            };
        }

        public static RootDTO ToRootDTO(this Root root)
        {

            HashSet<int> verseIds = root.Words.GroupBy(w => w.VerseId).Select(w => w.First()).Select(w => w.VerseId).ToHashSet();


            return new RootDTO
            {
                Own = root.Own,
                Latin = root.Latin,
                Translations = root.Scripture.Translations.Select(t => new TranslationWithMultiTextDTO
                {
                    Translation = new TranslationDTO
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Language = new LanguageDTO
                        {
                            LangCode = t.Language.LangCode,
                            LangOwn = t.Language.LangOwn,
                            LangEnglish = t.Language.LangEnglish
                        },
                        Translators = t.TranslatorTranslations.Select(e => new TranslatorDTO
                        {
                            Name = e.Translator.Name,
                            URL = e.Translator.Url,
                            Language = new LanguageDTO
                            {
                                LangCode = e.Translator.Language.LangCode,
                                LangOwn = e.Translator.Language.LangOwn,
                                LangEnglish = e.Translator.Language.LangEnglish
                            }
                        }).ToList(),
                        IsEager = t.EagerFrom.HasValue
                    },
                    TranslationTexts = t.TranslationTexts
                        .Where(tx => verseIds.Contains(tx.VerseId))
                        .Select(tx => new TranslationTextSimpleDTO
                        {
                            FootNotes = tx.FootNotes.Select(ftn => new FootNoteDTO
                            {
                                Index = ftn.Index,
                                Text = ftn.FootNoteText.Text
                            }).ToList(),
                            Text = tx.Text
                        }).ToList(),
                }).ToList(),
                Verses = root.Words
                   .Where(w => verseIds.Contains(w.VerseId))
                  .Select(w => w.Verse)
                  .Select(verse => new VerseExpendedWordDTO
                  {
                      Id = verse.Id,
                      Text = verse.Text,
                      Chapter = verse.Chapter.ToChapterDTO(),
                      TextSimplified = verse.TextSimplified,
                      TextWithoutVowel = verse.TextWithoutVowel,
                      Number = verse.Number,
                      Transliterations = verse.Transliterations.Select(transliteration => new TransliterationDTO
                      {
                          Transliteration = transliteration.Text,
                          Language = new LanguageDTO
                          {
                              LangCode = transliteration.Language.LangCode,
                              LangOwn = transliteration.Language.LangOwn,
                              LangEnglish = transliteration.Language.LangEnglish
                          }
                      }).ToList(),
                      Words = verse.Words.Select(word => new WordConfinedDTO
                      {
                          SequenceNumber = word.SequenceNumber,
                          Text = word.Text,
                          TextWithoutVowel = word.TextWithoutVowel,
                          TextSimplified = word.TextSimplified,
                          Roots = word.Roots.Select(root => new RootSimpleDTO
                          {
                              Latin = root.Latin,
                              Own = root.Own,
                          }).ToList()
                      }).ToList()

                  })
                      .ToList()
            };
        }
    }
}