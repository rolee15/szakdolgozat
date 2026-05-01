import { useId } from 'react';
import { useTheme } from '@/context/useTheme';

// [16] Heroicons outline sun/moon paths — https://heroicons.com (accessed 2026-05-01)
const SunIcon = () => (
  <svg
    viewBox="0 0 24 24"
    fill="none"
    stroke="currentColor"
    strokeWidth={2}
    strokeLinecap="round"
    strokeLinejoin="round"
    className="w-4 h-4"
    aria-hidden="true"
  >
    <circle cx="12" cy="12" r="4" />
    <path d="M12 2v2M12 20v2M4.93 4.93l1.41 1.41M17.66 17.66l1.41 1.41M2 12h2M20 12h2M4.93 19.07l1.41-1.41M17.66 6.34l1.41-1.41" />
  </svg>
);

const MoonIcon = () => (
  <svg
    viewBox="0 0 24 24"
    fill="none"
    stroke="currentColor"
    strokeWidth={2}
    strokeLinecap="round"
    strokeLinejoin="round"
    className="w-4 h-4"
    aria-hidden="true"
  >
    <path d="M21 12.79A9 9 0 1 1 11.21 3 7 7 0 0 0 21 12.79z" />
  </svg>
);

const optionClass = (isSelected: boolean) =>
  `flex items-center justify-center w-7 h-7 rounded transition-colors cursor-pointer ${
    isSelected
      ? 'bg-primary text-on-primary'
      : 'text-on-bar/60 hover:text-on-bar'
  }`;

const ThemeToggle = () => {
  const { theme, setTheme } = useTheme();
  const groupName = `theme-${useId()}`;

  return (
    <div
      role="radiogroup"
      aria-label="Theme"
      className="flex items-center gap-1 p-1 rounded-md border border-primary/30"
    >
      <label className={optionClass(theme === 'light')}>
        <input
          type="radio"
          name={groupName}
          value="light"
          checked={theme === 'light'}
          onChange={() => setTheme('light')}
          className="sr-only"
        />
        <SunIcon />
        <span className="sr-only">Light mode</span>
      </label>
      <label className={optionClass(theme === 'dark')}>
        <input
          type="radio"
          name={groupName}
          value="dark"
          checked={theme === 'dark'}
          onChange={() => setTheme('dark')}
          className="sr-only"
        />
        <MoonIcon />
        <span className="sr-only">Dark mode</span>
      </label>
    </div>
  );
};

export default ThemeToggle;
