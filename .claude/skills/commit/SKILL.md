---
name: commit
description: Stage changed files, generate a conventional commit message matching this project's git history style, commit, and push. No PRs needed — solo project.
---

# Commit & Push

## 1. Context

Run in parallel:

```bash
git status
git diff HEAD
```

## 2. Stage

- Add only files relevant to the change.
- Never stage `.env*`, credentials, or unrelated experimental files.
- Prefer `git add <specific files>` over `git add -A`.

## 3. Message

```
<type>: <short imperative description>
```

- Types used in this project: `feat`, `fix`, `refactor`, `test`, `docs`, `chore` (check `git log --oneline -20` if unsure).
- Lowercase, no trailing period, ~72-char subject. Optional blank-line body with bullets for larger changes.
- No `Co-Authored-By` trailer (solo project).

Use a HEREDOC for multi-line messages:

```bash
git commit -m "$(cat <<'EOF'
feat: add foo

- detail one
- detail two
EOF
)"
```

## 4. Push

```bash
git push
```

Then `git log --oneline -3` to confirm.

## GPG troubleshooting

If the commit fails with `gpg-agent` not running, reload the agent:

- Git Bash / WSL: `gpgconf --kill gpg-agent && gpgconf --launch gpg-agent`
- Windows (PS/cmd): `gpg-connect-agent reloadagent /bye`

Retry normally — do not use `--no-gpg-sign` unless the user explicitly allows it.
