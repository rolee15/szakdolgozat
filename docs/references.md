# External Source References

This file tracks all external sources whose code, algorithms, or data have been used or adapted in the KanjiKa project. It serves as the bibliography for the BSc thesis submitted to ELTE university.

**Citation style**: IEEE
**Format**: `[N] Author(s) or Organization, "Title or Page Name," *Site/Platform*, <URL>. Accessed: YYYY-MM-DD. License: <license>.`

Every entry must also appear as an inline comment (`// [N] ...`) directly above the relevant code in the source file.

---

## How to Add an Entry

1. Assign the next sequential number `[N]`.
2. Add the IEEE entry below under the appropriate category.
3. Fill in the `Used in:` field with the relative file path(s).
4. Add `// [N] <Short description> — <URL> (accessed YYYY-MM-DD)` above the code in the source file.

---

## Data Sources

[1] The Electronic Dictionary Research and Development Group (EDRDG), "KANJIDIC2 Project," *edrdg.org*, <http://www.edrdg.org/wiki/index.php/KANJIDIC_Project>. Accessed: 2025-01-01. License: CC BY-SA 4.0.
Used in: `server/src/KanjiKa.Data/Kanjidic2Parser.cs`

[2] T. Breen and the EDRDG, "JMdict/EDICT Dictionary Project," *edrdg.org*, <http://www.edrdg.org/wiki/index.php/JMdict-EDICT_Dictionary_Project>. Accessed: 2025-01-01. License: CC BY-SA 4.0.
Used in: *(not yet integrated into source files)*

[3] U. Apel, "KanjiVG," *kanjivg.tagaini.net*, <https://kanjivg.tagaini.net/>. Accessed: 2025-01-01. License: CC BY-SA 3.0.
Used in: *(not yet integrated into source files)*

[4] The Electronic Dictionary Research and Development Group (EDRDG), "RADKFILE/KRADFILE," *edrdg.org*, <http://www.edrdg.org/krad/kradinf.html>. Accessed: 2025-01-01. License: CC BY-SA 4.0.
Used in: *(not yet integrated into source files)*

---

## Code References

[5] MDN Web Docs, "Intersection Observer API," *Mozilla Developer Network*, <https://developer.mozilla.org/en-US/docs/Web/API/Intersection_Observer_API>. Accessed: 2026-03-30. License: CC BY-SA 2.5.
Used in: `client/src/pages/KanjiListPage.tsx`

[6] WanaKana Contributors, "WanaKana: JavaScript/TypeScript utility library for detecting and transliterating Hiragana, Katakana, and Romaji," *GitHub*, <https://github.com/WaniKani/WanaKana>. Accessed: 2026-04-02. License: MIT.
Used in: `client/src/components/lessons/WritingInput.tsx`

---

## Notes

- All EDRDG resources require the attribution statement: *"This application uses data from the Electronic Dictionary Research and Development Group (<http://www.edrdg.org>)."*
- CC BY-SA licenses require derivative works to be distributed under the same license terms.
