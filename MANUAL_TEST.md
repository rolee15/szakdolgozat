# Manual Regression Test Checklist

Run through these cases after every code change to verify nothing is broken.
Precondition: dev DB running, backend started, frontend started (see CLAUDE.md for setup).

**Test accounts** (seeded on every dev startup):

| Account | Email | Password | Profile |
| --- | --- | --- | --- |
| Admin | `admin@kanjika.com` | `Admin123!` | Admin role, forced password change on first login |
| Beginner | `beginner@test.com` | `almafa123` | No proficiencies, no progress — fresh user |
| Mid-learner | `midlearner@test.com` | `almafa123` | ~30 kana at mixed SRS stages (Apprentice1–Guru1) |
| Advanced | `advanced@test.com` | `almafa123` | All kana at Guru1, no items currently due |
| Reviewer | `reviewer@test.com` | `almafa123` | 40 kana + 10 kanji all overdue for review |

---

## 1. Authentication

### 1.1 Registration

- [ ] Navigate to `/register`
- [ ] Submit with invalid email -> validation error shown
- [ ] Submit with password < 8 chars -> validation error shown
- [ ] Submit with mismatched passwords -> validation error shown
- [ ] Submit with valid data -> account created, redirected to `/lessons`

### 1.2 Login

- [ ] Navigate to `/login`
- [ ] Submit with wrong credentials -> error message shown
- [ ] Submit with correct credentials -> redirected to `/lessons`
- [ ] Navbar shows username and Logout button

### 1.3 Logout

- [ ] Click Logout -> redirected to `/` or `/login`
- [ ] Visiting a protected route (e.g. `/lessons`) redirects to login

### 1.4 Forgot Password

- [ ] Navigate to `/forgot-password`
- [ ] Page renders with email input and "Send reset code" button
- [ ] Submit with valid email -> success message "コードをメールに送信しました" shown, step 2 form appears with reset code, new password, and confirm password fields
- [ ] Submit with API failure in step 1 -> error message shown, stays on step 1
- [ ] In step 2: enter correct code and matching passwords -> success message "パスワードがリセットされました" shown with link to login
- [ ] In step 2: enter mismatched passwords -> validation error "Passwords do not match" shown, no API call made
- [ ] In step 2: enter invalid/expired code -> error message shown

### 1.5 Forced Password Change

- [ ] Login as `admin@kanjika.com` / `Admin123!` -> redirected to `/change-password`
- [ ] Warning message "You must change your password before continuing" is displayed
- [ ] Submit with mismatched passwords -> validation error shown
- [ ] Submit with new password < 8 chars -> validation error shown
- [ ] Submit with wrong current password -> error message shown
- [ ] Submit with valid data -> password changed, redirected to `/lessons`
- [ ] Visiting any protected route while `mustChangePassword` is true -> redirected to `/change-password`
- [ ] After password change, normal navigation works

---

## 2. Navigation

- [ ] Desktop navbar shows three groups: Study, Practice, Path
- [ ] Study group contains: Hiragana, Katakana, Kanji, Grammar, Reading links
- [ ] Practice group contains: Lessons, Writing, Flash Cards links
- [ ] Path group contains: Learning Path link to `/path`
- [ ] Admin link visible in navbar for admin users only
- [ ] Admin link hidden for regular users
- [ ] Mobile: hamburger button (☰) appears on small screens
- [ ] Mobile: clicking hamburger opens dropdown nav with all links grouped by section
- [ ] Mobile: clicking a nav link closes the mobile menu
- [ ] Logo/brand link navigates to home or lessons
- [ ] 404 page shown for unknown routes (e.g. `/nonexistent`)

## 2a. Home Page Dashboard (`/`)

- [ ] Three stat cards render: "Due Reviews", "New Lessons", "Path Progress"
- [ ] Due Reviews card shows count and links to `/lessons/reviews`
- [ ] New Lessons card shows count and links to `/lessons`
- [ ] Path Progress card shows "X / Y units completed" and links to `/path`
- [ ] Loading skeleton (pulsing placeholder) shown while data fetches
- [ ] If an API call fails, card shows "–"

---

## 3. Hiragana / Katakana Grids

### 3.1 Hiragana (`/hiragana`)

- [ ] Grid of hiragana characters loads
- [ ] Each character shows a proficiency bar
- [ ] Click a character -> navigates to detail page (`/hiragana/{char}`)

### 3.2 Katakana (`/katakana`)

- [ ] Grid of katakana characters loads
- [ ] Each character shows a proficiency bar
- [ ] Click a character -> navigates to detail page (`/katakana/{char}`)

### 3.3 Character Detail (`/:type/:character`)

- [ ] Character symbol and romanization displayed
- [ ] Example words listed (word, reading, meaning)
- [ ] Proficiency percentage shown
- [ ] SRS stage badge shown with correct color (pink = Apprentice, purple = Guru, blue = Master, cyan = Enlightened, amber = Burned, gray = Locked)
- [ ] Back navigation works

---

## 4. Lessons

### 4.1 Lessons Hub (`/lessons`)

- [ ] "Learn" card shows count of new lessons available
- [ ] "Review" card shows count of items due for review
- [ ] Both counts load without error

### 4.2 New Lessons (`/lessons/new`)

- [ ] First character displayed with symbol and romanization
- [ ] Character type indicator shown (Hiragana/Katakana)
- [ ] Click "Next" -> advances to next character, progress counter updates
- [ ] After last character -> redirected back to `/lessons`
- [ ] Lesson count on hub decreases after completing lessons

### 4.3 Reviews (`/lessons/review`)

- [ ] Review item displayed with large character
- [ ] Type correct answer -> "Correct" feedback, advances to next item
- [ ] Type wrong answer -> "Incorrect" feedback with correct answer shown
- [ ] Wrong answers re-appear in the queue
- [ ] After all items reviewed -> "No more items to review" message
- [ ] Review count on hub decreases after completing reviews

---

## 5. Writing Practice (`/writing`)

### 5.1 Lessons Hub writing card (`/lessons`)

- [ ] "Writing" card shows count of items due for writing practice
- [ ] Click the Writing card -> navigates to `/writing`

### 5.2 Writing Practice page (`/writing`)

- [ ] Page shows romanization prompt in large text
- [ ] Character type label ("Hiragana" / "Katakana") shown above the romanization
- [ ] Input accepts romaji and auto-converts to hiragana when character type is hiragana
- [ ] Input accepts romaji and auto-converts to katakana when character type is katakana
- [ ] Pressing Enter submits the answer
- [ ] Clicking the submit button submits the answer
- [ ] Correct answer -> green feedback banner, input is disabled
- [ ] Incorrect answer -> red feedback banner with user's answer and correct kana shown, input is disabled
- [ ] Click "Continue" (or press Enter on feedback banner) -> advances to next item
- [ ] Correct items are removed from the queue; incorrect items re-appear later
- [ ] After all items reviewed -> "Writing practice complete!" message shown
- [ ] When no items are due -> "No items to review." message shown
- [ ] "Writing" link in navbar navigates to `/writing`
- [ ] API failure -> error state shown, page does not crash

---

## 6. Flashcards (`/flashcards`)

- [ ] Mode selector visible (Hiragana, Katakana, Kanji)
- [ ] Select Hiragana or Katakana -> cards load, romanization shown on back
- [ ] Click card -> flips with animation
- [ ] "Know it" button -> advances to next card
- [ ] "Don't know it" button -> advances to next card
- [ ] Progress indicator updates (X / total)
- [ ] After all cards -> session complete summary shown
- [ ] Can restart session
- [ ] Select Kanji mode with no due reviews (e.g. `beginner@test.com`) -> empty state "No kanji due for review" shown
- [ ] Select Kanji mode as `reviewer@test.com` -> kanji cards load (character on front, meaning on back)
- [ ] "Know it" / "Don't know it" on kanji cards submits kanji review and advances

---

## 6. Kanji

### 6.1 Kanji List (`/kanji`)

- [ ] JLPT level filter buttons visible (All, N5-N1); "All" is selected by default
- [ ] Click a level -> grid of kanji loads for that level
- [ ] "Showing X of Y kanji" count displayed
- [ ] Scrolling to the bottom of the list automatically loads the next page
- [ ] Loading spinner appears at the bottom while the next page is fetching
- [ ] Switching JLPT level tab resets the list and loads from page 1
- [ ] Each card shows: kanji character, English meaning, SRS stage badge
- [ ] SRS badges are color-coded (Apprentice/Guru/Master/Enlightened/Burned)

### 6.2 Kanji Detail (`/kanji/:character`)

- [ ] Large kanji character and meaning displayed
- [ ] On'yomi and Kun'yomi readings shown
- [ ] Stroke count displayed
- [ ] JLPT level shown
- [ ] SRS stage indicator present
- [ ] Proficiency bar (0-100%) displayed
- [ ] Example words table: Japanese word, reading, English meaning
- [ ] Back button returns to kanji list

---

## 7. Admin Dashboard

### 7.1 Admin Dashboard (`/admin`)

- [ ] Only accessible when logged in as admin
- [ ] Non-admin users redirected to `/`
- [ ] Shows "User Management" link to `/admin/users`

### 7.2 User Management (`/admin/users`)

- [ ] Paginated user table loads with columns: ID, Username, Role, Proficiencies, Lessons, Actions
- [ ] Search by username works (type a name, click Search)
- [ ] Pagination: Previous/Next buttons work, page counter updates
- [ ] "Showing X of Y users" count displayed
- [ ] Delete button visible for non-admin users, hidden for admin users
- [ ] Click Delete -> confirmation dialog -> user removed from list

### 7.3 User Detail (`/admin/users/:id`)

- [ ] User info displayed: ID, username, role, must change password status
- [ ] Proficiency table shown with character, learned date, last practiced date
- [ ] Lesson completions table shown with character and completion date
- [ ] Delete button visible for non-admin users, hidden for admin
- [ ] Click Delete -> confirmation dialog -> redirected to user list

---

## 8. Data Seeding

- [ ] Dev startup: DB is dropped and recreated with kana, kanji, admin user, and 4 test users
- [ ] Production startup: DB is migrated incrementally, reference data and admin user seeded idempotently
- [ ] Admin user `admin@kanjika.com` exists with `MustChangePassword = true`
- [ ] `beginner@test.com` has no proficiencies
- [ ] `midlearner@test.com` has ~30 kana proficiencies at mixed SRS stages
- [ ] `advanced@test.com` has all kana at Guru1
- [ ] `reviewer@test.com` has 40 kana + 10 kanji all overdue (NextReviewDate in the past)

---

## 9. Writing Practice (API)

### 9.1 `GET /api/lessons/writing-reviews/count`

- [ ] Authenticated request returns `{ "count": N }` where N equals the number of SRS-due characters for the user
- [ ] `testuser2@kanjika.com` (10 due reviews) returns `{ "count": 10 }`
- [ ] `testuser1@kanjika.com` (no due reviews) returns `{ "count": 0 }`
- [ ] Unauthenticated request returns 401

### 9.2 `GET /api/lessons/writing-reviews`

- [ ] Returns an array of `{ characterId, romanization, characterType }` objects for all due characters
- [ ] `characterType` is `"hiragana"` or `"katakana"` (lowercase)
- [ ] Items are ordered by `nextReviewDate` ascending
- [ ] `testuser1@kanjika.com` returns an empty array
- [ ] Unauthenticated request returns 401

### 9.3 `POST /api/lessons/writing-reviews/check`

- [ ] Body `{ "characterId": <id>, "typedCharacter": "<correct symbol>" }` returns `{ "isCorrect": true, "correctAnswer": "<symbol>", "srsStage": N, "srsStageName": "...", "nextReviewDate": "..." }`
- [ ] Body with wrong `typedCharacter` returns `{ "isCorrect": false, "correctAnswer": "<symbol>", ... }` and SRS stage is regressed
- [ ] Correct answer advances SRS stage by one
- [ ] Wrong answer regresses SRS stage by two (floored at Apprentice 1)
- [ ] Request with unknown `characterId` returns 400 or 500 (ArgumentException)
- [ ] Unauthenticated request returns 401

---

## 10. Grammar (`/grammar`)

### 10.1 Grammar List (`/grammar`)

- [ ] "Grammar" link in navbar navigates to `/grammar`
- [ ] Grid of N5 grammar point cards loads
- [ ] Each card shows title, pattern, and completion badge
- [ ] Completed points show green checkmark and "Completed"
- [ ] Incomplete points show "X/3 correct" in yellow
- [ ] Click a card → navigates to `/grammar/:id`

### 10.2 Grammar Detail (`/grammar/:id`)

- [ ] Title, pattern, and explanation text displayed
- [ ] Example sentences shown (Japanese, reading, English)
- [ ] Exercise fill-in-the-blank sentence displayed
- [ ] Four option buttons rendered
- [ ] Click correct answer → green "Correct!" banner, buttons disabled
- [ ] Click wrong answer → red "Incorrect. Correct answer: X" banner, buttons disabled
- [ ] "Next" button advances to next exercise
- [ ] After all exercises → score summary displayed (e.g. "Score: 2/3")
- [ ] When all 3 correct over time → "Grammar point completed!" shown
- [ ] Back button returns to `/grammar`
- [ ] API failure → error state shown, page does not crash

---

## 11. Error Handling

- [ ] API failure (stop backend) -> frontend shows error state, does not crash
- [ ] Refresh any protected page while logged in -> page loads correctly
- [ ] Non-admin accessing `/admin/*` routes -> redirected to home

---

## 12. Learning Path

- [ ] Learning path displays 15 units in order
- [ ] Units 1–14 are kana-only content (hiragana rows 1–8, katakana rows 9–14)
- [ ] Unit 15 is the Mixed N5 Review (grammar + reading)
- [ ] Completing a unit unlocks the next unit
- [ ] Unit tests use multiple-choice kana recognition questions

---

## 13. Shiritori (`/shiritori`)

- [ ] Navigate to `/shiritori` -> game board loads, player can see the initial word
- [ ] Submit a valid word starting with the last character of the previous word -> word accepted, computer responds
- [ ] Submit a word that does not start with the correct character -> rejection message shown, player must try again
- [ ] Submit a word ending in ん -> rejection message shown (ん-ending words are forbidden)
- [ ] Computer selects a word ending in ん -> computer loses, player wins message shown
- [ ] Player submits a word ending in ん and the game ends -> game over, computer wins message shown
- [ ] Disconnect or abandon game (navigate away) -> game is abandoned, next visit starts fresh

---

## 14. User Settings (`/settings`)

- [ ] Navigate to `/settings` -> current settings load (daily lesson limit, review batch size, JLPT level)
- [ ] Change daily lesson limit and save -> updated value persisted, success feedback shown
- [ ] Change review batch size and save -> updated value persisted, success feedback shown
- [ ] Change JLPT level and save -> updated value persisted, success feedback shown
- [ ] API failure on load -> error state shown, page does not crash
- [ ] API failure on save -> error message shown, settings not lost
