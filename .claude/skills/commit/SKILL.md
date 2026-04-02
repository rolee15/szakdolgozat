---
name: commit
description: Stage changed files, generate a conventional commit message matching this project's git history style, commit, and push. No PRs needed — solo project.
---

# Commit & Push

Follow these steps exactly.

## 1. Gather context

Run in parallel:
```bash
git status
git diff HEAD
git log --oneline -6
```

## 2. Decide what to stage

- Stage all files relevant to the current change.
- **Never stage**: `.env*`, credentials, secrets, unrelated experimental files.
- Prefer `git add <specific files>` over `git add -A`.

## 3. Write the commit message

Match the project's existing commit style from `git log`:

```
<type>: <short imperative description>
```

**Types used in this project**: `feat`, `fix`, `bug`, `refactor`, `test`, `docs`, `chore`

Rules:
- Lowercase type and description
- No period at end
- Max ~72 chars for the subject line
- If the change is large, add a blank line and a short body bullet list
- No "Co-Authored-By" trailer needed (solo dev project)

## 4. Commit and push

```bash
git add <relevant files>
git commit -m "type: description"
git push
```

Use a HEREDOC for the message if it has multiple lines:
```bash
git commit -m "$(cat <<'EOF'
feat: add foo feature

- detail one
- detail two
EOF
)"
```

## 5. Confirm

Show the output of `git log --oneline -3` to confirm the commit landed.
