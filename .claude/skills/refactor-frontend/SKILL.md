---
name: refactor-frontend
description: >
  Refactors the KanjiKa React/TypeScript frontend. Scans client/src/ for code smells,
  then delegates targeted fixes to the frontend-dev agent. Use when asked to "refactor
  the frontend", "clean up React code", "apply React best practices", or "fix TypeScript issues".
---

# Frontend Refactor — React 18 + TypeScript

**Plan — don't implement.** Produce a risk-ordered plan; delegate each item to `frontend-dev`.

## Phase 0 — Scan

Read `client/src/App.tsx`, `types/`, `services/`, `context/`, `hooks/`, `pages/`, and `components/`. Record each smell per file.

## Phase 1 — Smells

### Architecture

- `fetch` called from a page or component (must go through `services/`).
- Props drilled through 3+ levels unused by intermediaries.
- Business rules / data transforms in JSX (belong in a hook or service).
- Loading / error / data triple repeated across pages without a shared hook.
- Inline query keys (`useQuery(['foo', id])`) — must be constant factories.

### TypeScript

- `any`, unsafe `as` casts, `!` non-null suppressions.
- Exported functions without explicit return types.
- API shapes typed inline instead of in `client/src/types/`.
- Mutually-exclusive states as string literals instead of discriminated unions.
- `@ts-ignore` / `@ts-expect-error` without an explanation.

### React

- Premature `useMemo` / `useCallback` without measurement.
- `useQuery` result used without handling loading / error / empty.
- `useEffect` + `fetch` instead of TanStack Query.
- Form state via `useState` per field instead of React Hook Form.
- Components > ~150 lines mixing fetch, logic, and presentation.

## Phase 2 — Plan

```
### F-<N>: <Short title>
Files:     <every file to touch>
Smell:     <from Phase 1>
Pattern:   <custom hook | service function | discriminated union | context | co-location | ...>
Change:    <what to do>
Rationale: <why it's better>
Risk:      Low / Medium / High
```

Order low-risk first.

## Phase 3 — Rules

### TypeScript

- `"strict": true`. No `any` — use `unknown` + type guards at truly unknown boundaries.
- Let TS infer simple types; annotate at public API boundaries only.
- Prop interface for every component; discriminated unions for state variants.
- Use utility types (`Partial`, `Pick`, `Readonly`) over duplicated interfaces.

### React components

- Function components only. One responsibility each. Colocate state with its consumer; lift only when truly shared.
- Avoid prop drilling past 2 levels — use Context or restructure.
- Every data view handles loading, error, and empty explicitly.
- No preemptive memoisation — measure with Profiler first.

### Custom hooks

- `use`-prefixed. One concern per hook (no god hooks).
- Extract duplicated `useQuery` + derived state into shared hooks.
- Hooks return data / callbacks, never JSX.

### TanStack Query

- No `useEffect` + `fetch` — `useQuery` / `useMutation` only.
- Query keys come from a constants module:

```ts
export const QUERY_KEYS = {
  characters: (userId: number) => ['characters', userId] as const,
};
useQuery({ queryKey: QUERY_KEYS.characters(userId), ... });
```

- Set `staleTime` on rarely-changing data. Use `select` to transform.

### React Hook Form

- No manual form state. Define a validation schema (Zod/Yup) and pass via resolver. Use `formState.errors` for display.

### Service layer

- All `fetch` lives in `client/src/services/`, routed through `apiClient` and `routes.ts`.
- Explicit return type on every service function (`Promise<Character[]>`).
- On non-ok response, throw a typed error — never silently return `undefined`.
- One file per API resource group (`kanaService`, `lessonService`, etc.).

## Phase 4 — Delegate & verify

1. Present the plan; wait for approval.
2. Batch low-risk changes into one `frontend-dev` call; isolate high-risk or interdependent items.
3. After each batch: `npm run build` + `npm run test:coverage -- --run`. Coverage must stay above the threshold in `CLAUDE.md` — on failure, spawn `debugger`.
4. Update `MANUAL_TEST.md`; cite external sources in `docs/references.md`.
