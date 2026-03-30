# KanjiKa - 24-Hour Implementation Plan

**Date**: 2026-03-29
**Deadline**: Present "almost complete" work to teacher on 2026-03-30
**Final thesis deadline**: 2026-04-10 (show to advisor), 2026-05-01 (submit)

---

## Thesis Summary

A web-based Japanese language learning app where lessons are tailored to the unique aspects of the Japanese language. Users can practice reading/writing kana and kanji, learn grammar, and stay motivated through varied exercises and a structured learning path.

## Current State

**Working**: Hiragana/Katakana grids, character details with examples, basic lesson system (learn new + review), user registration/login (backend only), proficiency tracking (0-100 scale)

**Broken/Stubbed**: Auth uses dummy tokens, frontend hardcodes user ID to `'1'`, register page doesn't call backend, forgot password is stub, no route protection

**Missing entirely**: Kanji, dictionary, SRS algorithm, games (Shiritori, Match Madness), flash cards, TTS, settings page, learning path/units

---

## Teacher's Last Feedback

> "Login form + multiple user support, remove hardcoded stuff. Shiritori with websocket."

---

## Features (Prioritized for 24 Hours)

### PRIORITY 1 — Must Do (Teacher's explicit requests + core thesis value)

#### 1. Fix Authentication & Multi-User Support (~3-4 hours)
> *Teacher specifically asked for this. Currently the #1 blocker.*

- [ ] **Backend**: Implement real JWT token generation in `TokenService` (replace dummy "test12345")
- [ ] **Backend**: Add JWT validation middleware in `Program.cs`
- [ ] **Backend**: Add `[Authorize]` to lessons/kana endpoints, extract user ID from token claims
- [ ] **Frontend**: Make `RegisterPage` actually call `userService.register()`
- [ ] **Frontend**: Store JWT token properly, add to API request headers (Axios interceptor)
- [ ] **Frontend**: Remove hardcoded `MOCK_USER_ID = '1'` from `kanaService.ts` and `lessonService.ts`
- [ ] **Frontend**: Add auth context/provider so logged-in user ID flows through the app
- [ ] **Frontend**: Add route guards (redirect to `/login` if not authenticated)
- [ ] **Frontend**: Fix login page to store token and redirect to home on success

#### 2. Spaced Repetition System (SRS) Upgrade (~2-3 hours)
> *Core thesis feature — transforms the app from "quiz app" to "learning platform"*

- [ ] **Backend**: Add SRS fields to `Proficiency` entity: `SrsStage` (0-9), `NextReviewDate`, `CorrectCount`, `IncorrectCount`
- [ ] **Backend**: Implement SRS intervals (Apprentice → Guru → Master → Enlightened → Burned)
  - Stage 1: 4h, Stage 2: 8h, Stage 3: 1d, Stage 4: 2d, Stage 5: 1w, Stage 6: 2w, Stage 7: 1mo, Stage 8: 4mo, Stage 9: Burned
- [ ] **Backend**: Update `CheckLessonReviewAnswerAsync` to advance/regress SRS stage instead of flat +10/-5
- [ ] **Backend**: Update review query to only return items where `NextReviewDate <= now`
- [ ] **Frontend**: Show SRS stage on character detail page (Apprentice/Guru/Master/etc.)
- [ ] **DB Migration**: Add new columns

#### 3. Kanji Data & Basic Kanji Pages (~3-4 hours)
> *Central thesis feature — "gradually phasing in kanji"*

**Data source**: KANJIDIC2 (XML, CC BY-SA 4.0, ~13,108 kanji from EDRDG)
- Download from: http://www.edrdg.org/wiki/index.php/KANJIDIC_Project

- [ ] **Parse KANJIDIC2**: Write a seed script/tool to parse the XML and extract ~300 most common kanji (by frequency rank)
  - Fields: literal, meanings, on'yomi, kun'yomi, stroke count, JLPT level, grade, frequency rank
- [ ] **Backend**: Create `Kanji` entity with fields: `Id`, `Literal`, `Meanings`, `OnReadings`, `KunReadings`, `StrokeCount`, `JlptLevel`, `Grade`, `FrequencyRank`
- [ ] **Backend**: Create `KanjiController` with endpoints: GET `/api/kanji` (list, filterable by JLPT/grade), GET `/api/kanji/{literal}` (detail)
- [ ] **Backend**: Seed kanji data (at least JLPT N5 ~100 kanji for demo)
- [ ] **Frontend**: Create `KanjiListPage` — grid/table of kanji, filterable by JLPT level
- [ ] **Frontend**: Create `KanjiDetailPage` — shows meanings, readings, stroke count, examples
- [ ] **Frontend**: Add routes: `/kanji` and `/kanji/:character`
- [ ] **Frontend**: Add kanji link to navigation

### PRIORITY 2 — Should Do (High thesis value, impressive in presentation)

#### 4. Flash Cards (~2 hours)
> *Listed in thesis plan, reuses existing data*

- [ ] **Frontend**: Create `FlashCardPage` with flip animation (front: character, back: meaning/reading)
- [ ] **Frontend**: Support modes: Hiragana, Katakana, Kanji
- [ ] **Frontend**: "Know it" / "Don't know it" buttons that feed into SRS
- [ ] **Frontend**: Add route `/flashcards` and nav link

#### 5. Dictionary/Search Page (~2-3 hours)
> *Listed in thesis plan — "clicking on words brings up definition"*

- [ ] **Backend**: Create search endpoint GET `/api/dictionary/search?q=...` that searches characters + kanji by romanization, meaning, or literal
- [ ] **Frontend**: Create `DictionaryPage` with search bar
- [ ] **Frontend**: Display results as cards (character, readings, meanings)
- [ ] **Frontend**: Clicking a result navigates to character/kanji detail page
- [ ] **Frontend**: Add route `/dictionary` and nav link

#### 6. Shiritori Game with WebSocket (~3-4 hours)
> *Teacher specifically requested this*

- [ ] **Backend**: Add SignalR hub for Shiritori game
- [ ] **Backend**: Game logic: validate word starts with last kana of previous word, no repeats, timer
- [ ] **Backend**: Session management: create room, join via link, track scores
- [ ] **Frontend**: Create `ShiritoriPage` with game lobby (create/join room)
- [ ] **Frontend**: Game UI: word input, timer, word history, scores
- [ ] **Frontend**: Real-time updates via SignalR client
- [ ] **Frontend**: Add route `/shiritori` and nav link

### PRIORITY 3 — Nice to Have (If time permits)

#### 7. Settings Page (~1 hour)
- [ ] User profile display
- [ ] Change password form
- [ ] Daily lesson limit preference

#### 8. Dashboard/Progress Page (~1-2 hours)
- [ ] Show overall progress (kana learned %, kanji learned %)
- [ ] SRS stage distribution chart
- [ ] Review forecast (upcoming reviews)
- [ ] Streak counter

#### 9. Match Madness Game (~2 hours)
- [ ] Timed matching game: match characters to their readings
- [ ] Score tracking, difficulty levels

#### 10. Text-to-Speech (~1 hour)
- [ ] Use Web Speech API (`speechSynthesis`) for Japanese pronunciation
- [ ] Add "play" button on character detail pages and flash cards

---

## Suggested 24-Hour Schedule

Assuming ~12-14 productive hours with breaks and sleep:

| Block | Time | Task | Why |
|-------|------|------|-----|
| 1 | 3-4h | **Auth + Multi-User** (#1) | Teacher's top request, unblocks everything |
| 2 | 2-3h | **SRS Upgrade** (#2) | Core differentiator, makes reviews meaningful |
| 3 | 3-4h | **Kanji Pages** (#3) | Central thesis feature |
| 4 | 2h | **Flash Cards** (#4) | Quick win, high visual impact for demo |
| 5 | 1-2h | **Dictionary** (#5) or **TTS** (#10) | Pick one based on remaining energy |

**Total: ~12-14 hours of focused work**

Shiritori (#6) is impressive but complex (WebSocket/SignalR setup). Consider it if auth goes faster than expected, or save it for the final push before April 10.

---

## Data Sources

| Source | What | License | URL |
|--------|------|---------|-----|
| **KANJIDIC2** | 13,108 kanji (meanings, readings, stroke count, JLPT, grade) | CC BY-SA 4.0 | edrdg.org/wiki/index.php/KANJIDIC_Project |
| **KanjiVG** | Stroke order SVG diagrams | CC BY-SA 3.0 | kanjivg.tagaini.net |
| **RADKFILE** | Radical ↔ kanji mappings | CC BY-SA 4.0 | edrdg.org/krad/kradinf.html |
| **JMdict** | 200K+ word entries (for dictionary feature) | CC BY-SA 4.0 | edrdg.org/wiki/index.php/JMdict-EDICT_Dictionary_Project |

Attribution required: "This application uses data from the EDRDG (Electronic Dictionary Research and Development Group)"

---

## SRS Stage Reference (WaniKani-inspired)

| Stage | Name | Interval | Color suggestion |
|-------|------|----------|-----------------|
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

---

## Architecture Notes for Kanji

Extend the existing clean architecture:

```
KanjiKa.Core/Entities/Kanji/
├── Kanji.cs          # Literal, Meanings (string[]), OnReadings, KunReadings, StrokeCount, JlptLevel, Grade, FrequencyRank
└── KanjiProficiency.cs  # UserId + KanjiId composite key, SrsStage, NextReviewDate

KanjiKa.Api/Controllers/
└── KanjiController.cs   # GET /api/kanji, GET /api/kanji/{literal}

KanjiKa.Api/Services/
└── KanjiService.cs      # Query, search, SRS logic

KanjiKa.Data/
└── Seed/KanjiSeeder.cs  # Parse KANJIDIC2 XML → seed DB
```

The existing `Proficiency` entity should also get SRS fields added (`SrsStage`, `NextReviewDate`) so kana reviews also benefit from proper spaced repetition.
