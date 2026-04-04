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
- [ ] Page renders with email input (UI-only, no backend integration yet)

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

- [ ] Navbar links work: Hiragana, Katakana, Lessons, Kanji, Flash Cards, Writing
- [ ] Admin link visible in navbar for admin users only
- [ ] Admin link hidden for regular users
- [ ] Logo/brand link navigates to home or lessons
- [ ] 404 page shown for unknown routes (e.g. `/nonexistent`)

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
- [ ] SRS stage name shown (e.g. "Locked", "Apprentice 1")
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

## 10. Error Handling

- [ ] API failure (stop backend) -> frontend shows error state, does not crash
- [ ] Refresh any protected page while logged in -> page loads correctly
- [ ] Non-admin accessing `/admin/*` routes -> redirected to home
