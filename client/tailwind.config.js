/** @type {import('tailwindcss').Config} */
export default {
  content: ["./index.html", "./src/**/*.{js,ts,jsx,tsx}"],
  theme: {
    extend: {
      colors: {
        brand: {
          surface: "#242424",
          primary: {
            light: "#818cf8",
            DEFAULT: "#6366f1",
            dark: "#4f46e5",
            deep: "#312e81",
          },
          text: "#ffffff",
          muted: "#9ca3af",
        },
      },
    },
  },
  plugins: [],
};
