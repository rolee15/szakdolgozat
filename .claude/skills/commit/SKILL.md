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
```

## 2. Decide what to stage

- Stage all files relevant to the current change.
- **Never stage**: `.env*`, credentials, secrets, unrelated experimental files.
- Prefer `git add <specific files>` over `git add -A`.

## 3. Write the commit message

Use this project's commit style:

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

## Troubleshooting: GPG signing failure

If `git commit` fails with `gpg: can't connect to the keyboxd` or `gpg-agent` not running, the GPG agent process has died. Fix it by running in a **Windows** terminal (PowerShell or cmd, not Git Bash):

```powershell
gpg-connect-agent reloadagent /bye
```

Or from Git Bash / WSL:
```bash
gpgconf --kill gpg-agent
gpgconf --launch gpg-agent
```

After the agent is back, retry the commit normally. Do **not** use `--no-gpg-sign` unless the user explicitly allows it.
