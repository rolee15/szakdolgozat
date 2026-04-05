---
name: refactor-frontend
description: >
  Refactors the KanjiKa React/TypeScript frontend. Scans client/src/ for code smells,
  then delegates targeted fixes to the frontend-dev agent. Use when asked to "refactor
  the frontend", "clean up React code", "apply React best practices", or "fix TypeScript issues".
---

# Frontend Refactor — React 18 + TypeScript

You are auditing and planning the refactor of the KanjiKa frontend (`client/`). **Do not write code yourself.** Produce a concrete, risk-ordered plan and then delegate each item to the `frontend-dev` agent.

---

## Phase 0 — Codebase scan

Read every file in this order:

1. `client/src/App.tsx`
2. All files in `client/src/types/`
3. All files in `client/src/services/`
4. All files in `client/src/pages/`
5. All files in `client/src/components/`

For each file, note every instance of the smells catalogued below.

---

## Phase 1 — Smell catalogue

### Architecture smells

| Smell | What to look for |
|---|---|
| Leaky service layer | `fetch` calls inside a page or component instead of a service function |
| Prop drilling | Props passed through 3+ component levels unused by intermediaries |
| Logic in components | Business rules or data transforms inside JSX — belongs in a hook or service |
| Duplicated fetch pattern | Same loading/error/data triple repeated across pages without a shared hook |
| Inline query keys | `useQuery(['foo', id])` — keys must be constant objects, not inline arrays |

### TypeScript smells

| Smell | What to look for |
|---|---|
| `any` usage | Silences type checking — replace with explicit interfaces |
| Missing return types | Exported functions without explicit return type annotations |
| Absent interfaces | API response shapes typed inline or cast with `as` |
| Loose string literals | State variants as raw `string` instead of discriminated unions |
| Suppressed strict flags | `// @ts-ignore` or `// @ts-expect-error` without explanation |

### React smells

| Smell | What to look for |
|---|---|
| Premature `useMemo`/`useCallback` | Memoisation without a measured performance problem — React 18 compiler handles most cases |
| Missing loading / error / empty states | A `useQuery` result used without handling all three branches |
| Manual `useEffect` for fetching | `useEffect` + `fetch` instead of TanStack Query |
| Form state managed manually | `useState` for each field instead of React Hook Form |
| Inconsistent error display | Some pages show a message, others silently fail |
| Large components | Components > ~150 lines mixing data fetching, business logic, and presentation |

---

## Phase 2 — Refactor plan

Produce a numbered list ordered by risk (lowest first). For each item:

```
### F-<N>: <Short title>

Files:     <list every file to touch>
Smell:     <which smell from Phase 1>
Pattern:   <custom hook | service function | discriminated union | context | co-location | ...>
Change:    <what to do, precisely>
Rationale: <why this is better — cite the rule below if applicable>
Risk:      Low / Medium / High
```

---

## Phase 3 — Best-practice checklist

Apply every rule below when reviewing and when briefing `frontend-dev`:

### TypeScript rules
*(Source: [TypeScript Best Practices 2025](https://dev.to/mitu_mariam/typescript-best-practices-in-2025-57hb), [React & TypeScript Guide 2025](https://medium.com/@CodersWorld99/react-19-typescript-best-practices-the-new-rules-every-developer-must-follow-in-2025-3a74f63a0baf))*

- `"strict": true` in `tsconfig.json` — covers `strictNullChecks`, `noImplicitAny`, and 8 other checks
- **No `any`** — use `unknown` with type guards when the shape is truly unknown
- Let TypeScript **infer** simple types (`const x = 3` not `const x: number = 3`); annotate explicitly only at public API boundaries
- Define **prop interfaces** for every component; use generics for reusable ones
- Prefer **discriminated unions** over boolean flags or string literals for mutually exclusive states:
  ```ts
  type QuizState =
    | { status: 'idle' }
    | { status: 'loading' }
    | { status: 'success'; data: Character[] }
    | { status: 'error'; message: string }
  ```
- Use **utility types** (`Partial<T>`, `Pick<T, K>`, `Readonly<T>`) instead of duplicating interfaces
- Explicit return types on all exported functions and hooks

### React component rules
*(Source: [React Design Patterns 2025](https://www.telerik.com/blogs/react-design-patterns-best-practices), [Modern React Best Practices](https://strapi.io/blog/react-and-nextjs-in-2025-modern-best-practices))*

- **Function components only** — no class components
- **Single responsibility**: one component does one thing; if a component fetches data AND renders a list AND handles an empty state AND handles an error — split it
- **Colocate state**: keep state as close as possible to the component that consumes it; lift only when two siblings genuinely share it
- **Avoid prop drilling beyond 2 levels** — use React Context or restructure the component tree
- Every component that renders data must explicitly handle **loading**, **error**, and **empty** states
- Do **not** reach for `useMemo`/`useCallback` preemptively — measure with React DevTools Profiler first; React 18 handles most cases automatically

### Custom hook rules
*(Source: [Mastering Custom React Hooks](https://dev.to/austinwdigital/mastering-custom-react-hooks-best-practices-for-clean-scalable-code-40b1))*

- Prefix every hook with `use` — no exceptions
- **One hook, one concern**: `useCharacters` fetches characters; `useProficiency` fetches proficiency; a single `useEverything` is a god hook
- Extract duplicated `useQuery` + derived state into a shared hook rather than copying the pattern into every page
- Hooks must never return JSX — they return data and callbacks only

### TanStack React Query rules
*(Source: [React & Next.js in 2025](https://strapi.io/blog/react-and-nextjs-in-2025-modern-best-practices))*

- **No `useEffect` for data fetching** — use `useQuery` or `useMutation` exclusively
- Query keys must be **constants**, never inline arrays:
  ```ts
  // ❌ Bad
  useQuery(['characters', userId], ...)

  // ✅ Good
  export const QUERY_KEYS = {
    characters: (userId: number) => ['characters', userId] as const,
  }
  useQuery(QUERY_KEYS.characters(userId), ...)
  ```
- Set `staleTime` on queries where data doesn't change frequently to avoid unnecessary refetches
- Use `select` to transform/filter data inside the query instead of doing it in the component

### React Hook Form rules

- **No manual form state** — no `useState` per field; use `register`, `control`, `watch` from RHF
- Define a **Zod schema** (or `yup`) for every form and pass it to `useForm` via `zodResolver` — validation logic lives in the schema, not in `onSubmit` callbacks
- Use `formState.errors` for display; never write manual `if (!value)` guards in the render

### Service layer rules

- **All `fetch` calls live in `services/`** — no exceptions
- Every service function has an explicit TypeScript return type: `Promise<Character[]>`
- On non-ok response, **throw a typed error** — never silently return `undefined`
- Group related endpoints in one service file (`kanaService`, `lessonService`, `userService`) — one file per API resource group

---

## Phase 4 — Delegation

1. Present the full plan to the user and wait for approval before spawning agents.
2. Group low-risk changes into a single `frontend-dev` call. Keep high-risk or interdependent changes separate.
3. After each agent call, verify:
   ```bash
   cd client && npm run build
   cd client && npm run test:coverage -- --run
   ```
   Coverage must remain ≥ 94%. If it drops, spawn `debugger` before continuing.
4. After all changes: update `MANUAL_TEST.md` for any changed behavior; add citations to `docs/references.md` if any pattern was adapted from external sources.
