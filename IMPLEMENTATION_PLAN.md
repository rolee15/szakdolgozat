# KanjiKa — Implementation Plan

**Date**: 2026-03-29
**Last updated**: 2026-04-14
**Next consultation**: 2026-04-15 (most implementation done + documentation started)
**Final thesis deadline**: 2026-05-01

---

## Thesis Promises vs Current State

| # | Promise (THESIS.md paragraph 2) | Status | Plan |
| --- | --- | --- | --- |
| 1 | Practicing **reading** Japanese characters | Done | Kana grids, flashcards, lesson reviews |
| 2 | Practicing **writing** Japanese characters | Done | `WritingPracticePage` with wanakana romaji→kana input |
| 3 | **Understanding texts** | Done | `ReadingListPage`, `ReadingDetailPage`, passages + comprehension questions |
| 4 | Acquiring Japanese **grammar rules** | Done | 12 N5 grammar points with fill-in-the-blank exercises |
| 5 | **Various difficulty levels** | Partial | Day 8: user JLPT level setting, all content tagged |
| 6 | **Various practice exercises** | Partial | Days 2-4 add writing, grammar, reading exercises |
| 7 | **Structured learning path** | Done | `LearningPathPage`, `UnitDetailPage`, 6 seeded units with unlock logic |
| 8 | **Verification tests** at end of lessons/modules | Done | `UnitTestPage`, 70% pass threshold, `UnitTest` entity |
| 9 | **Customizable** lessons/learning path | Partial | Day 8: settings page + data-driven architecture |
| 10 | **Interesting and challenging** (motivation) | Partial | SRS + varied exercises |

**Shiritori** (teacher's suggestion) — nice-to-have, Day 11 buffer if time permits.

---

## Content Sourcing Strategy

Grammar exercises, reading passages, and learning path material will be sourced from open-source/free-to-use resources and organized into lessons with exams.

### Primary Source: Hanabira.org

- **Repository**: https://github.com/tristcoil/hanabira.org
- **License**: Code: MIT, Content: Creative Commons
- **What it has**: JLPT N5-N1 grammar points, vocabulary, example sentences as JSON datasets
- **How to use**: Download N5 grammar JSON, extract grammar points with explanations/examples, adapt into exercise format

### Supplementary Sources

- **Tae Kim's Guide to Learning Japanese** (CC BY-NC-SA 4.0) — grammar explanations, sentence patterns
- **JLPT official sample questions** (Japan Foundation) — reading passage format and comprehension questions
- **NHK World Easy Japanese** — simple texts with furigana for reading practice

All sources must be cited in `docs/references.md` per thesis attribution rules.

---

## Data Organization Architecture

**Decision: Extend the existing DB seeding pattern.** All external data gets parsed and seeded into PostgreSQL on dev startup via `KanjiKaDataSeeder`. This is the fastest approach because the pattern already works for kanji (KANJIDIC2 gz → parser → DB).

**Why DB seeding (not a separate data service):**

- All data access goes through EF Core repositories — no mixed patterns
- Grammar, reading, and path data can be filtered, joined, and paginated via LINQ
- Proficiency/progress entities need FK relationships to content — must be in the same DB
- `Kanjidic2Parser.cs` + `KanjiKaDataSeeder.cs` already does this for 3000+ kanji
- No new infrastructure needed

**Data pipeline per content type:**

| Content | Source format | Parser | DB target |
| --- | --- | --- | --- |
| Kana (existing) | Hardcoded `TestData.cs` | Direct | `Characters`, `Examples` |
| Kanji (existing) | KANJIDIC2 `.xml.gz` resource | `Kanjidic2Parser.cs` | `Kanji`, `KanjiExamples` |
| Grammar (new) | Hanabira N5 JSON resource | `GrammarDataParser.cs` | `GrammarPoints`, `GrammarExamples`, `GrammarExercises` |
| Reading (new) | JSON resource or direct seed | Direct seed | `ReadingPassages`, `ComprehensionQuestions` |
| Learning units (new) | Defined in seeder code | Direct seed | `LearningUnits`, `UnitContents` |

---

## Test User Seeding Strategy

Multiple seeded test users with varied proficiency data so every feature is manually testable after a backend restart.

| User | Email | Password | Profile | Purpose |
| --- | --- | --- | --- | --- |
| Beginner | `beginner@test.com` | `almafa123` | No proficiencies, no progress | Fresh user, onboarding, empty path |
| Mid-learner | `midlearner@test.com` | `almafa123` | ~30 kana (mixed SRS), 5 kanji, 3 grammar, 1 reading, Units 1-2 done | Mid-progress, active reviews |
| Advanced | `advanced@test.com` | `almafa123` | All kana (Guru+), 20+ kanji, all grammar, all readings, Units 1-5 done | Near-completion, high proficiency |
| Reviewer | `reviewer@test.com` | `almafa123` | ~40 kana + 10 kanji all with past `NextReviewDate` | Always has items to review |

---

## Completed Features

### Priority 1 — Done

#### 1. Authentication & Multi-User Support

- [x] Real JWT token generation in `TokenService`
- [x] JWT validation middleware in `Program.cs`
- [x] `[Authorize]` on lessons/kana endpoints, user ID from token claims
- [x] `RegisterPage` calls `userService.register()`
- [x] JWT stored properly, added to API headers via `apiFetch`
- [x] Removed hardcoded `MOCK_USER_ID = '1'`
- [x] `AuthContext` provider for logged-in user ID
- [x] `ProtectedRoute` route guards
- [x] Login stores token and redirects to home

#### 2. Spaced Repetition System (SRS)

- [x] SRS fields on `Proficiency`: `SrsStage` (0-9), `NextReviewDate`
- [x] SRS intervals: Apprentice (4h/8h/1d/2d) → Guru (1w/2w) → Master (1mo) → Enlightened (4mo) → Burned
- [x] `CheckLessonReviewAnswerAsync` advances/regresses SRS stage
- [x] Review query returns only items where `NextReviewDate <= now`

#### 3. Kanji Data & Pages

- [x] KANJIDIC2 parsed, ~3000+ kanji seeded
- [x] `Kanji` entity with meanings, readings, stroke count, JLPT level
- [x] `KanjiController` endpoints: by level, detail, paged listing
- [x] `KanjiListPage` with JLPT filter + infinite scroll + SRS badges
- [x] `KanjiDetailPage` with readings, examples, stats
- [x] Routes and nav link

#### 4. Flash Cards

- [x] 3D CSS flip animation
- [x] Hiragana/Katakana modes
- [x] Know/Don't know buttons
- [x] Route and nav link
- [x] Kanji mode (enabled — kanji review DTOs + endpoints added, know/don't know buttons)

---

## Remaining Work — Day-by-Day (April 1–14)

### WEEK 1: Core Features

#### Day 1 (Apr 1) — Bug Fixes + Kanji Proficiency + Test Users + Seeder Refactor (~7h) ✅

**Bugs:**

- [x] `KanjiService.cs`: hardcoded `Proficiency=0, SrsStage="Locked"` → create `KanjiProficiency` entity, query real data
- [x] `LessonService.cs` BUG comment: lesson count ignores unlearned characters → cap at `min(dailyRemaining, unlearnedCount)`
- [x] `UserService.cs`: hardcoded refresh token `"test23456"` → real token rotation
- [x] `UserService.cs`: hardcoded reset code `"12345"` → random code with expiry
- [x] Kana detail page missing SRS stage → add to `KanaCharacterDetailDto` + `CharacterDetail.tsx`

**Kanji proficiency:**

- [x] New `KanjiProficiency` entity (mirrors kana `Proficiency`)
- [x] `DbSet<KanjiProficiency>` in `KanjiKaDbContext`
- [x] `KanjiService` queries real proficiency
- [x] Kanji learn/review endpoints in `KanjiController`
- [x] Enable Kanji mode in `FlashCardPage.tsx`

**Seeder refactor (dev + production):**

- [x] Split `KanjiKaDataSeeder` into `ProductionDataSeeder` (static data, dev+prod) + `DevelopmentDataSeeder` (test users, dev only)
- [x] `ProductionDataSeeder` checks `if (!context.Characters.Any())` before seeding (idempotent for production)
- [x] `Program.cs`: run both seeders in dev, only `ProductionDataSeeder` in production
- [x] Prepares clean slots for grammar, reading, learning path seeding on Days 3-5

**Test user seeding:**

- [x] Seed 4 test users (Beginner, Mid-learner, Advanced, Reviewer) with varied proficiency data
- [x] Each user has different SRS stages and staggered `NextReviewDate` values
- [x] Update `MANUAL_TEST.md` with credentials table

#### Day 2 (Apr 2) — Writing Practice (~5h) ✅

> Thesis: "practicing writing Japanese characters" — reverse-direction exercises using `wanakana` for romaji→kana conversion.

- [x] `GET /api/lessons/writing-reviews` — SRS-due items in reverse (prompt=romanization, expected=character)
- [x] `POST /api/lessons/writing-reviews/check` — validates typed character
- [x] `WritingPracticePage.tsx` — romanization prompt, wanakana-bound input, feedback
- [x] `WritingInput.tsx` component with wanakana `bind()`
- [x] Route `/writing`, nav link, button on `LessonsPage.tsx`

#### Day 3 (Apr 3) — Grammar System + Content Import (~8h) ✅

> Thesis: "acquiring Japanese grammar rules" — content sourced from Hanabira.org (CC BY-SA 4.0).

**Content sourcing (first 2h):**

- [x] Download Hanabira N5 grammar JSON from GitHub (embedded as resource `grammar-n5.json`)
- [x] Write parser/seeder to extract grammar points into entity format (`GrammarDataParser.cs`)
- [x] Generate exercises (fill-in-the-blank) from examples — note: multiple-choice not implemented, fill-blank only
- [x] Target: 12 N5 grammar points with examples and exercises
- [x] Cite Hanabira in `docs/references.md`

**Backend:**

- [x] Entities: `GrammarPoint`, `GrammarExample`, `GrammarExercise`, `GrammarProficiency`
- [x] `IGrammarRepository`/`GrammarRepository`, `IGrammarService`/`GrammarService`
- [x] `GrammarController`: list, detail, check endpoints (`GET /api/grammar`, `GET /api/grammar/:id`, `POST` check)
- [x] Per-user proficiency tracking (completion at 3 correct answers)
- [x] 13 backend unit tests

**Frontend:**

- [x] `GrammarListPage.tsx` — list by JLPT level with proficiency indicators
- [x] `GrammarDetailPage.tsx` — explanation, examples, interactive exercises
- [x] `grammarService.ts`, types, routes `/grammar`, `/grammar/:id`
- [x] 204 frontend tests at 96% coverage

#### Day 4 (Apr 4) — Text Comprehension + Content Import (~7h) ✅

> Thesis: "understanding texts" — passages sourced from JLPT samples and NHK Easy Japanese.

**Content sourcing (first 2h):**

- [x] Collect/adapt 6 N5-level passages from JLPT samples, NHK Easy, or original compositions
- [x] Write 3-4 comprehension questions per passage (multiple-choice)
- [x] Cite sources in `docs/references.md`

**Backend:**

- [x] Entities: `ReadingPassage`, `ComprehensionQuestion`, `ReadingProficiency`
- [x] `IReadingRepository`/`ReadingRepository`, `IReadingService`/`ReadingService`
- [x] `ReadingController`: list, detail, submit endpoints

**Frontend:**

- [x] `ReadingListPage.tsx` — passage list with completion status
- [x] `ReadingDetailPage.tsx` — passage text + furigana toggle + questions + score
- [x] `readingService.ts`, types, routes `/reading`, `/reading/:id`

#### Day 5 (Apr 5) — Learning Path + Verification Tests (~7h) ✅

> Thesis: "structured learning path" + "verification tests at the end of lessons and modules."

**Backend:**

- [x] Entities: `LearningUnit`, `UnitContent`, `UnitProgress`, `UnitTest`
- [x] `IPathService`/`PathService` — path retrieval, test generation, scoring (70% pass), unlock logic
- [x] `PathController`: `GET /api/path`, unit detail, test, submit endpoints

**Seed 6 N5 units:**

1. Hiragana vowels + particle は + self-intro passage
2. Hiragana K-row + particles が/を + daily routine passage
3. Hiragana S-row + です/ます + shopping passage
4. Remaining hiragana + adjectives + directions passage
5. Katakana + 10 N5 kanji + weather passage
6. Mixed review + food passage

**Frontend:**

- [x] `LearningPathPage.tsx` — vertical timeline (locked/in-progress/completed)
- [x] `UnitDetailPage.tsx` — contents + "Take Test" button
- [x] `UnitTestPage.tsx` — quiz, score, pass/fail
- [x] `pathService.ts`, types, routes `/path`, `/path/:unitId`, `/path/:unitId/test`

#### Day 6 (Apr 6) — Testing Catchup (~6h) ✅

- [x] Backend: 198 unit tests (Grammar, Reading, Path, KanaService, KanjiService, UserService, LessonService, PathService, + DTOs + Entities)
- [x] Frontend: tests for all new pages (45 test files, 294 tests)
- [x] Verify: 97.22% frontend coverage (threshold: 94%), 198 backend tests (threshold: 82)
- [x] Clean build: `npm run build` (0 errors) + `dotnet build --configuration Release` (0 warnings, 0 errors)
- [x] Fixed `tsconfig.app.json`: removed deprecated `baseUrl`/`ignoreDeprecations`, fixed TS2698 spread types in 5 test files
- [x] Fixed CS8618 nullable navigation properties in `Example.cs` and `LessionCompletion.cs`

#### Day 7 (Apr 7) — UI Polish + Navigation (~5h) ✅

- [x] Reorganize `Navbar.tsx` with grouped menu (Study / Practice / Path)
- [x] Improve `HomePage.tsx` with summary (reviews count, path progress)
- [x] Shared `SrsBadge.tsx` component — used in `KanjiListPage`, `KanjiDetailPage`, `CharacterDetail`
- [x] Responsive design pass — `KanaGrid` gets `overflow-x-auto` + `min-w-max` for mobile scroll; `GrammarDetailPage` options grid becomes `grid-cols-1 sm:grid-cols-2`

### WEEK 2: Polish + Documentation

#### Day 8 (Apr 8) — Difficulty Levels + Customization (~4h) ✅

- [x] User settings: `jlptLevel`, `dailyLessonLimit`, `reviewBatchSize`
- [x] `GET/PUT /api/users/settings` endpoints
- [x] `SettingsPage.tsx` with form
- [x] `KanjiListPage` initializes JLPT filter from user settings (falls back to All on error; user click takes immediate priority)

#### Day 9 (Apr 9) — Testing + CI Green (~6h) ✅

- [x] Full test suites: 390 frontend tests (98.22% stmt, 94.91% branch coverage), 82+ backend tests
- [x] Tests updated for Day 7-8 changes (SrsBadge usage, settings-driven JLPT filter)
- [x] Both builds pass clean

#### Day 10 (Apr 10) — Edge Cases + Manual Testing (~4h)

- [ ] End-to-end manual testing
- [ ] Update `MANUAL_TEST.md` for all new features
- [ ] Fix edge cases (empty states, loading, errors)

#### Day 11 (Apr 11) — Shiritori ✅

- [x] SignalR hub + service for word validation and room management
- [x] Computer opponent from 90-word hiragana list
- [x] Frontend: lobby + game board with input and history
- [x] Route `/shiritori`, nav link added

#### Days 12–14 (Apr 12–14) — Full Documentation Days

Polish, fill gaps, final review pass. Send to consultant by Apr 13 for review before Apr 15 meeting.

---

## Documentation — Parallel Track

**The thesis documentation runs in parallel with implementation, not after it.** The consultant needs a draft by April 15 (send a few days before for review). The document will be extensive.

Form requirements will be provided later — use a working structure that can be reformatted.

### Thesis outline (Hungarian, code in English)

1. **Bevezetes** (~2-3p) — problem statement, goals, feature overview
2. **Felhasznaloi dokumentacio** (~5-8p) — setup guide, feature walkthrough with screenshots
3. **Fejlesztoi dokumentacio** (~15-20p) — architecture, tech choices, DB schema (ER diagram), SRS algorithm, data pipeline, testing, CI/CD
4. **Tervezesi dontesek** (~3-5p) — why SRS, why open-source content, why clean architecture, data source licensing
5. **Osszefoglalas** (~1-2p) — achievements, future work
6. **Hivatkozasok** — IEEE format from `docs/references.md`
7. **Fuggelek** — API endpoint list, DB schema, screenshots

### Documentation schedule (woven into implementation days, ~1-2h/day)

| Day | Implementation | Documentation |
| --- | --- | --- |
| 1 (Apr 1) | Bug fixes + kanji proficiency | Set up thesis structure, write Introduction draft |
| 2 (Apr 2) | Writing practice | Document existing features: auth, kana grids, SRS |
| 3 (Apr 3) | Grammar system | Document architecture, tech stack justification |
| 4 (Apr 4) | Text comprehension | Document DB schema + ER diagram, data pipeline |
| 5 (Apr 5) | Learning path | Document SRS algorithm, content sourcing decisions |
| 6 (Apr 6) | Testing catchup | Document testing strategy, CI/CD pipeline |
| 7 (Apr 7) | UI polish | Document new features: grammar, reading, writing, path |
| 8 (Apr 8) | Customization | User documentation: setup guide, feature walkthrough |
| 9 (Apr 9) | Testing + CI | Add screenshots to user documentation |
| 10 (Apr 10) | Manual testing | Design decisions chapter, conclusion |
| 11 (Apr 11) | Buffer/Shiritori | References, appendix, final review pass |
| 12-14 | Full documentation | Polish, fill gaps, review, **send to consultant by Apr 13** |

---

## If Falling Behind — Cut in This Order

1. Reduce reading passages from 6 to 2-3
2. Grammar: explanations only, defer interactive exercises
3. Settings/customization page (mention as "architecture supports it" in thesis)
4. Shiritori — already optional, skip entirely if needed

**Cannot cut**: Writing practice, grammar content, reading comprehension, learning path with tests, documentation start.

---

## Dropped from Previous Plan

Not promised in thesis, not required:

- ~~Dictionary/Search page~~
- ~~Match Madness game~~
- ~~Text-to-Speech~~
- ~~Full dashboard/progress page~~
- ~~Real email sending~~

---

## Content Sources — Must Cite

| Source | What to use | License |
| --- | --- | --- |
| Hanabira.org | N5 grammar JSON (points, explanations, examples) | CC (content), MIT (code) |
| Tae Kim's Guide | Grammar explanations, sentence patterns | CC BY-NC-SA 4.0 |
| JLPT Sample Questions | Reading passage format, comprehension questions | Public (Japan Foundation) |
| NHK Easy Japanese | Simple texts for reading practice | Free educational use |
| KANJIDIC2 | Kanji data (already integrated) | CC BY-SA 4.0 |

---

## SRS Stage Reference

| Stage | Name | Interval | Color |
| --- | --- | --- | --- |
| 0 | Locked | — | Gray |
| 1 | Apprentice 1 | 4 hours | Pink |
| 2 | Apprentice 2 | 8 hours | Pink |
| 3 | Apprentice 3 | 1 day | Pink |
| 4 | Apprentice 4 | 2 days | Pink |
| 5 | Guru 1 | 1 week | Purple |
| 6 | Guru 2 | 2 weeks | Purple |
| 7 | Master | 1 month | Blue |
| 8 | Enlightened | 4 months | Cyan |
| 9 | Burned | — (retired) | Gold |
