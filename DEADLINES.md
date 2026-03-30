# KanjiKa - Thesis Deadlines & Info

## Deadlines (June Final Exam Period)

| Date | Milestone | Status |
|------|-----------|--------|
| Oct 15 | Topic registration, work plan | Done |
| Dec 1 | Design phase, software requirements | Done |
| Feb 10 | Software architecture, enroll in Thesis Consultation course | Done |
| Mar 1 | Implementation (60-70%) presentation | Done |
| **Mar 30** | **Consultation with advisor (near-complete demo)** | **TODAY** |
| **Apr 1** | **Final exam registration** | Upcoming |
| Apr 10 | Present finished thesis to advisor | Upcoming |
| Apr 15 | Late final exam registration (with penalty fee) | Fallback |
| **May 1** | **Thesis submission** | Upcoming |

---

## Consultation Talking Points (Mar 30)

### 1. Progress Since Last Meeting

- **Authentication fixed**: Real JWT tokens, proper login/register flow, removed hardcoded user ID — multiple users now work independently
- **Spaced Repetition System (SRS)**: Reviews now use a WaniKani-inspired 9-stage SRS algorithm (Apprentice → Guru → Master → Enlightened → Burned) instead of flat +10/-5 scoring
- **Kanji support started**: Integrated KANJIDIC2 open database (EDRDG, CC BY-SA 4.0) — the same data source Jisho.org uses
- **Flash cards**: New study mode with flip animations, feeds into SRS

### 2. Addressing Previous Feedback

- **Login + multi-user**: Done — JWT auth, route guards, user-specific data
- **Shiritori with WebSocket**: Planned for next sprint (SignalR hub designed, not yet implemented) — discuss priority vs. other features

### 3. Current Feature Set

- Hiragana & Katakana grids with character details and examples
- Lesson system: learn new characters (15/day limit) + SRS-based reviews
- User accounts with registration, login, JWT authentication
- Proficiency tracking per user per character
- Kanji browsing (filterable by JLPT level) *(in progress)*
- Flash card study mode *(in progress)*

### 4. Remaining Work (Before Apr 10)

- **Kanji detail pages** with meanings, readings, stroke count
- **Dictionary/search** feature (search across all characters and kanji)
- **Shiritori game** with WebSocket (SignalR)
- **Dashboard/progress page** showing SRS stats and learning progress
- **Settings page** (change password, preferences)
- **Text-to-speech** using Web Speech API for pronunciation
- **Polish & bug fixes** for presentation

### 5. Questions for Advisor

- Is the current feature scope sufficient for the thesis, or should I prioritize depth (fewer features, more polished) vs. breadth?
- Should I focus on Shiritori (WebSocket demo) or Dictionary (more practical) for the remaining time?
- Any specific requirements for the thesis document structure/format?
- Feedback on the SRS implementation — is the algorithm worth discussing in detail in the thesis?
- Timeline for thesis document writing — should I start the written part now in parallel?

### 6. Data Sources & Attribution

- **KANJIDIC2** (13,108 kanji) — CC BY-SA 4.0, from EDRDG (Jim Breen, Monash University)
- **KanjiVG** (stroke order SVGs) — CC BY-SA 3.0
- **RADKFILE** (radical mappings) — CC BY-SA 4.0
- All properly cited in `docs/references.md` per ELTE thesis requirements

### 7. Technical Architecture (if asked)

- Frontend: React 18 + TypeScript + Vite + TanStack React Query
- Backend: .NET 8 Web API + EF Core + PostgreSQL
- Auth: JWT with refresh tokens
- Real-time (planned): SignalR for Shiritori
- CI: GitHub Actions — 82+ backend tests, 94%+ frontend coverage

---

## Final Exam Info

### Exam Topics

- ELTE Faculty of Informatics, BSc in Computer Science (2018 curriculum)
- Worked-out topics: <https://github.com/TMD44/elte-ik-pti-bsc-zarovizsga>

### Study Materials

- Lecture notes: <https://github.com/Petrosz007/uni>
- Object-Oriented Programming: <https://people.inf.elte.hu/gt/oep/>

---

## Thesis Topic Summary

A web-based Japanese language learning application tailored to the unique aspects of the Japanese language. Unlike generic language apps, KanjiKa provides lessons and exercises specifically designed for Japanese character reading/writing, text comprehension, and grammar — all in one place, with structured learning paths at various difficulty levels.

---

## Key Links

| Resource | URL |
|----------|-----|
| Final exam topics (GitHub) | <https://github.com/TMD44/elte-ik-pti-bsc-zarovizsga> |
| Lecture notes (Petrosz007) | <https://github.com/Petrosz007/uni> |
| OOP materials | <https://people.inf.elte.hu/gt/oep/> |
| KANJIDIC2 (kanji data) | <http://www.edrdg.org/wiki/index.php/KANJIDIC_Project> |
| JMdict (dictionary data) | <http://www.edrdg.org/wiki/index.php/JMdict-EDICT_Dictionary_Project> |
| KanjiVG (stroke order) | <https://kanjivg.tagaini.net/> |
| Jisho.org (reference) | <https://jisho.org/> |
| WaniKani (SRS reference) | <https://www.wanikani.com/> |
