# Manual Regression Test Checklist

Run through these cases after every code change to verify nothing is broken.
Precondition: dev DB running, backend started, frontend started (see CLAUDE.md for setup).

**Test accounts** (seeded on every dev startup):

- `admin@kanjika.com` / `Admin123!` — admin account, forced password change on first login
- `testuser1@kanjika.com` / `almafa123` — 61 learned characters, no reviews due
- `testuser2@kanjika.com` / `almafa123` — 10 learned characters (あ–こ), all 10 due for review

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

- [ ] Navbar links work: Hiragana, Katakana, Lessons, Kanji, Flash Cards
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

## 5. Flashcards (`/flashcards`)

- [ ] Mode selector visible (Hiragana, Katakana; Kanji disabled)
- [ ] Select a mode -> cards load
- [ ] Click card -> flips with animation, shows romanization on back
- [ ] "Know it" button -> advances to next card
- [ ] "Don't know it" button -> advances to next card
- [ ] Progress indicator updates (X / total)
- [ ] After all cards -> session complete summary shown
- [ ] Can restart session

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

- [ ] Dev startup: DB is dropped and recreated with kana, kanji, admin user, and test users
- [ ] Production startup: DB is migrated incrementally, reference data and admin user seeded idempotently
- [ ] Admin user `admin@kanjika.com` exists with `MustChangePassword = true`

---

## 9. Error Handling

- [ ] API failure (stop backend) -> frontend shows error state, does not crash
- [ ] Refresh any protected page while logged in -> page loads correctly
- [ ] Non-admin accessing `/admin/*` routes -> redirected to home
