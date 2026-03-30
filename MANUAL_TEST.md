# Manual Regression Test Checklist

Run through these cases after every code change to verify nothing is broken.
Precondition: dev DB running, backend started, frontend started (see CLAUDE.md for setup).

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

---

## 2. Navigation

- [ ] Navbar links work: Hiragana, Katakana, Lessons, Kanji, Flash Cards
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

- [ ] JLPT level filter buttons visible (N5-N1)
- [ ] Click a level -> grid of kanji loads
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

## 7. Error Handling

- [ ] API failure (stop backend) -> frontend shows error state, does not crash
- [ ] Refresh any protected page while logged in -> page loads correctly
